using System.Diagnostics;
using Job.Config.i18n;
using ExecSaveJob;
using Job.Config;
using Logger;

namespace Job.Services;

public class ServiceExecSaveJob
{
    public static (int, string) Run(Configuration configuration, int? id, string? name)
    {
        SaveJob? saveJob = null;
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

            LoggerUtility.WriteLog(LoggerUtility.Info, $"Saving : id: {id.ToString()} name : {saveJob.Name} from ({saveJob.Source}) to ({saveJob.Destination})");

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            
            if (saveJob.Type == "full")
            {
                // CompleteBackup completeBackup = CompleteBackup.GetInstance(saveJob);
                CompleteBackup completeBackup = new CompleteBackup(saveJob);
                completeBackup.Save();
            }
            else if (saveJob.Type == "diff")
            {
                // DifferentialBackup differentialBackup = DifferentialBackup.GetInstance(saveJob);
                DifferentialBackup differentialBackup = new DifferentialBackup(saveJob);
                differentialBackup.Save();
            }
            
            stopwatch.Stop();
            
            LoggerUtility.WriteLog(LoggerUtility.Info, $"The savejob took {stopwatch.ElapsedMilliseconds} ms");
            LoggerUtility.WriteLog(LoggerUtility.Info, $"Save : id: {id.ToString()}, name : {saveJob.Name} from ({saveJob.Source}) to ({saveJob.Destination}) is save");
            return (1, $"{Translation.Translator.GetString("SjExecSuccesfully") ?? String.Empty} - ID : {id.ToString()}");
        }
        else
        {
            string returnSentence = $"{Translation.Translator.GetString("BadArgsGiven") ?? String.Empty} - ID : {id.ToString()}";
            LoggerUtility.WriteLog(LoggerUtility.Warning, returnSentence);
            return (2, returnSentence);
        }
    }
}