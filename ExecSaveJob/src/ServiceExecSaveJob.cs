using System;
using System.Diagnostics;

using Config;
using Logger;
using Services;

namespace ExecSaveJob;

public class ServiceExecSaveJob
{
    public static int Run(string[] args, Configuration configuration)
    {
        if (args.Length == 1)
        {
            int id;
            SaveJob? saveJob = null;
            if (int.TryParse(args[0], out id))
            {
                saveJob = configuration.GetSaveJob(id);   
            }
            else
            {
                saveJob = configuration.GetSaveJob(args[0]);
            }
            if (saveJob == null)
            {
                LoggerUtility.WriteLog(LoggerUtility.Warning, $"Can't find SaveJob with id: {args[0].ToString()}");
                return ReturnCodes.JOB_DOES_NOT_EXIST;
            }
            else
            {
                LoggerUtility.WriteLog(LoggerUtility.Info, $"Saving :  id: {id.ToString()} name : {saveJob.Name} from ({saveJob.Source}) to ({saveJob.Destination})");

            }
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            // DirCopy dirCopy = new DirCopy();
            // dirCopy.CopyDir(saveJob.Source, saveJob.Destination);
            
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
            return ReturnCodes.OK;
        }
        else
        {
            LoggerUtility.WriteLog(LoggerUtility.Warning, "Some args are missing or incorrect");
            return ReturnCodes.BAD_ARGS;
        }
    }
}