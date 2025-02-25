using System.Diagnostics;
using Job.Config.i18n;
using Job.Config;
using Job.Controller;
using Job.Services.ExecSaveJob;
using Logger;

namespace Job.Services;

public class ServiceExecSaveJob
{
    private static Configuration _configuration;
    public static (int, string) Run(Configuration configuration, int? id, string? name)
    {
        SaveJob? saveJob = null;
        _configuration = ConfigSingleton.Instance();
        bool execLock = false;
        List<string> runningBusinessApps = new List<string>();
        foreach (var processus in _configuration.GetBuisnessApp())
        {
            execLock = Process.GetProcessesByName(processus).Any();
            if (execLock)
            {
                runningBusinessApps.Add(processus);
            }
        }

        if (!execLock)
        {
            if (id is not null ^ name is not "")
            {
                if (id is not null)
                {
                    saveJob = configuration.GetSaveJob(id ?? -1);   
                }
                else
                {
                    saveJob = configuration.GetSaveJob(name ?? string.Empty);
                }

                LoggerUtility.WriteLog(_configuration.GetLogType(),LoggerUtility.Info, $"Saving : id: {id.ToString()} name : {saveJob.Name} from ({saveJob.Source}) to ({saveJob.Destination})");

                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                
                if (saveJob.Type == "full")
                { 
                    CompleteBackup completeBackup = new CompleteBackup(saveJob);
                    completeBackup.Save();
                }
                else if (saveJob.Type == "diff")
                {
                    DifferentialBackup differentialBackup = new DifferentialBackup(saveJob);
                    differentialBackup.Save();
                }
                
                stopwatch.Stop();
                
                LoggerUtility.WriteLog(_configuration.GetLogType(),LoggerUtility.Info, $"The savejob took {stopwatch.ElapsedMilliseconds} ms");
                LoggerUtility.WriteLog(_configuration.GetLogType(),LoggerUtility.Info, $"Save : id: {id.ToString()}, name : {saveJob.Name} from ({saveJob.Source}) to ({saveJob.Destination}) is save");
                return (1, $"{Translation.Translator.GetString("SjExecSuccesfully") ?? String.Empty} - ID : {id.ToString()}");
            }
            else
            {
                string returnSentence = $"{Translation.Translator.GetString("BadArgsGiven") ?? String.Empty} - ID : {id.ToString()}";
                LoggerUtility.WriteLog(_configuration.GetLogType(),LoggerUtility.Warning, returnSentence);
                return (2, returnSentence);
            }
        }
        return (4, $"{Translation.Translator.GetString("BusinessAppLock") ?? String.Empty} : {string.Join(", ", runningBusinessApps)}");
    }
}