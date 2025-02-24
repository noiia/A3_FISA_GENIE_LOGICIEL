using System;
using System.Diagnostics;
using System.Collections.Generic;
using Job.Config;
using Job.Config.i18n;
using Job.Services;
using Logger;

namespace Job.Controller;


public class ExecutionTracker
{
    public event EventHandler<TrackerChangedEventArgs> OnTrackerChanged;

    private Dictionary<int, Dictionary<DateTime, int>> _endGetter = new();

    public Dictionary<int, Dictionary<DateTime, int>> EndGetter
    {
        get => _endGetter;
        private set => _endGetter = value;
    }

    public void AddOrUpdateExecution(int id, DateTime timestamp, int returnCode)
    {
        if (!_endGetter.ContainsKey(id))
        {
            _endGetter[id] = new Dictionary<DateTime, int>();
        }

        _endGetter[id][timestamp] = returnCode;

        OnTrackerChanged?.Invoke(this, new TrackerChangedEventArgs(id, timestamp, returnCode));
    }
}

public class TrackerChangedEventArgs : EventArgs
{
    public int Id { get; }
    public DateTime Timestamp { get; }
    public int ReturnCode { get; }

    public TrackerChangedEventArgs(int id, DateTime timestamp, int returnCode)
    {
        Id = id;
        Timestamp = timestamp;
        ReturnCode = returnCode;
    }
}

public class ExecuteSaveJob
{
    private static Configuration _configuration;
    public static async Task<(int, string)> Execute(List<int>  ids, string separator, ExecutionTracker tracker)
    {
        _configuration = ConfigSingleton.Instance();
        LoggerUtility.WriteLog(_configuration.GetLogType(),LoggerUtility.Info, $"{Translation.Translator.GetString("SjExecWith")} {string.Join(" ", ids, separator)}");
        if (ids.Count > 1 && separator is "" or " ")
        {
            return (3, $"{Translation.Translator.GetString("TooMuchIdWithoutSep")} {separator} {ids}");
        }
        
        int returnCode;
        string message;
        switch (separator)
        {
            case ";":
                string listId = "";
               
                foreach (var id in ids)
                {
                    (returnCode, message) = await SaveJobRepo.ExecuteSaveJob(id);
                    tracker.AddOrUpdateExecution(id, DateTime.Now, returnCode);
                    if (returnCode is 2)
                    {
                        return (returnCode, message + listId);
                    } 
                    listId += $"{id}, ";
                }
                return (1, $"{Translation.Translator.GetString("SjExecSuccesfully")} {listId}");
            
            case  ",":
                for (int i = ids[0]; i <= ids[1]; i++)
                {
                    (returnCode, message) = await SaveJobRepo.ExecuteSaveJob(i);
                    tracker.AddOrUpdateExecution(i, DateTime.Now, returnCode);
                    if (returnCode is 2)
                    {
                        return (returnCode, message);
                    }
                }
                return (1, $"{Translation.Translator.GetString("SjExecSuccesfully")} : {ids[0]} - {ids[1]}");
            
            case "":
                (returnCode, message) = await SaveJobRepo.ExecuteSaveJob(ids[0]);
                tracker.AddOrUpdateExecution(ids[0], DateTime.Now, returnCode);
                if (returnCode is 2)
                {
                    return (returnCode, message);
                }
                return (1, $"{message}");
            
            default:
                return (3, $"{Translation.Translator.GetString("SeparatorNotReco")} {separator}");
        }
    }
}