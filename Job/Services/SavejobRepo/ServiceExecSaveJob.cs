﻿using System.Diagnostics;
using Job.Config.i18n;
using ExecSaveJob;
using Job.Config;
using Logger;

namespace Job.Services;

public class ServiceExecSaveJob
{
    public static (int, string) Run(Configuration configuration, int? id, string? name)
    {
        if (id is not null ^ name is not "")
        {
            SaveJob? saveJob = null;
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
                CompleteBackup completeBackup = CompleteBackup.GetInstance(saveJob);
                completeBackup.Save();
            }
            else if (saveJob.Type == "diff")
            {
                DifferentialBackup differentialBackup = DifferentialBackup.GetInstance(saveJob);
                differentialBackup.Save();
            }
            
            stopwatch.Stop();
            
            LoggerUtility.WriteLog(LoggerUtility.Info, $"The savejob took {stopwatch.ElapsedMilliseconds} ms");
            LoggerUtility.WriteLog(LoggerUtility.Info, $"Save : id: {id.ToString()}, name : {saveJob.Name} from ({saveJob.Source}) to ({saveJob.Destination}) is save");
            return (1, $"{Translation.Translator.GetString("SjCreatedSuccesfully") ?? String.Empty}");
        }
        else
        {
            string returnSentence = $"{Translation.Translator.GetString("BadArgsGiven") ?? String.Empty} {id} {name}";
            LoggerUtility.WriteLog(LoggerUtility.Warning, returnSentence);
            return (2, returnSentence);
        }
    }
}