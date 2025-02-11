﻿using System;
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
            int id = int.Parse(args[0]);
            SaveJob? saveJob = configuration.GetSaveJob(id);
            if (saveJob == null)
            {
                LoggerUtility.WriteLog(LoggerUtility.Warning, $"Can't found SaveJob with id: {args[0].ToString()}");
                return ReturnCodes.JOB_DOES_NOT_EXIST;
            }
            else
            {
                LoggerUtility.WriteLog(LoggerUtility.Info, $"Saving :  id: {id.ToString()} name : {saveJob.Name} from ({saveJob.Source}) to ({saveJob.Destination})");

            }
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            DirCopy dirCopy = new DirCopy();

            dirCopy.CopyDir(saveJob.Source, saveJob.Destination);
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