using System;
using System.Diagnostics;
using System.Collections.Generic;
using Job.Config;
using Job.Config.i18n;
using Job.Services;
using Logger;

namespace Job.Controller;

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
                    switch (returnCode)
                    {
                        case 2:
                        {
                            return (returnCode, message + string.Join(", ", listId));
                        }
                        case 3:
                        {
                            return (returnCode, message + string.Join(", ", listId));
                        }
                        case 4:
                        {
                            return (returnCode, message);
                        }
                    }
                    listId += $"{id}, ";
                }
                return (1, $"{Translation.Translator.GetString("SjExecSuccesfully")} {listId}");
            
            case  ",":
                for (int i = ids[0]; i <= ids[1]; i++)
                {
                    (returnCode, message) = await SaveJobRepo.ExecuteSaveJob(i);
                    tracker.AddOrUpdateExecution(i, DateTime.Now, returnCode);
                    switch (returnCode)
                    {
                        case 2:
                        {
                            return (returnCode, $"{message} {ids[0]} - {ids[1]}");
                        }
                        case 3:
                        {
                            return (returnCode, $"{message} {ids[0]} - {ids[1]}");
                        }
                        case 4:
                        {
                            return (returnCode, message);
                        }
                    }
                }
                return (1, $"{Translation.Translator.GetString("SjExecSuccesfully")} : {ids[0]} - {ids[1]}");
            
            case "":
                (returnCode, message) = await SaveJobRepo.ExecuteSaveJob(ids[0]);
                tracker.AddOrUpdateExecution(ids[0], DateTime.Now, returnCode);
                switch (returnCode)
                {
                    case 2:
                    {
                        return (returnCode, $"{message} {ids[0]}");
                    }
                    case 3:
                    {
                        return (returnCode, $"{message} {ids[0]}");
                    }
                    case 4:
                    {
                        return (returnCode, message);
                    }
                }
                return (1, $"{message}");
            
            default:
                return (3, $"{Translation.Translator.GetString("SeparatorNotReco")} {separator}");
        }
    }
}