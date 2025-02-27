using System.Diagnostics;
using Job.Config;
using Job.Config.i18n;
using Job.Controller;
using Job.Services.ExecSaveJob;
using Logger;

namespace Job.Services;

public class ServiceExecSaveJob
{
    private static Configuration _configuration;


    public static (int, string) Run(Configuration configuration, LockTracker lockTracker, BigFileTracker bigFileTracker,
        int? id, string? name)
    {
        SaveJob? saveJob = null;
        _configuration = ConfigSingleton.Instance();
        var execLock = false;
        List<string> runningBusinessApps = new List<string>();
        foreach (var processus in _configuration.GetBuisnessApp())
        {
            execLock = Process.GetProcessesByName(processus).Any();
            if (execLock) runningBusinessApps.Add(processus);
        }

        if (!execLock)
        {
            if (id is not null ^ name is not "")
            {
                if (id is not null)
                    saveJob = configuration.GetSaveJob(id ?? -1);
                else
                    saveJob = configuration.GetSaveJob(name ?? string.Empty);

                LoggerUtility.WriteLog(_configuration.GetLogType(), LoggerUtility.Info,
                    $"Saving : id: {id.ToString()} name : {saveJob.Name} from ({saveJob.Source}) to ({saveJob.Destination})");

                var stopwatch = new Stopwatch();
                stopwatch.Start();

                if (saveJob.Type == "full")
                {
                    var completeBackup = new CompleteBackup(saveJob);
                    completeBackup.Save(id ?? -1, lockTracker, bigFileTracker);
                }
                else if (saveJob.Type == "diff")
                {
                    var differentialBackup = new DifferentialBackup(saveJob);
                    differentialBackup.Save(id ?? -1, lockTracker, bigFileTracker);
                }

                stopwatch.Stop();

                LoggerUtility.WriteLog(_configuration.GetLogType(), LoggerUtility.Info,
                    $"The savejob took {stopwatch.ElapsedMilliseconds} ms");
                LoggerUtility.WriteLog(_configuration.GetLogType(), LoggerUtility.Info,
                    $"Save : id: {id.ToString()}, name : {saveJob.Name} from ({saveJob.Source}) to ({saveJob.Destination}) is save");
                return (1,
                    $"{Translation.Translator.GetString("SjExecSuccesfully") ?? string.Empty} - ID : {id.ToString()}");
            }

            var returnSentence =
                $"{Translation.Translator.GetString("BadArgsGiven") ?? string.Empty} - ID : {id.ToString()}";
            LoggerUtility.WriteLog(_configuration.GetLogType(), LoggerUtility.Warning, returnSentence);
            return (2, returnSentence);
        }

        return (4,
            $"{Translation.Translator.GetString("BusinessAppLock") ?? string.Empty} : {string.Join(", ", runningBusinessApps)}");
    }
}