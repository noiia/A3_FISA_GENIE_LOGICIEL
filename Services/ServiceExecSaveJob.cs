﻿using System.Diagnostics;
using Config;
using Logger;

namespace Services;

public class ServiceExecSaveJob
{
    public const int OK = 1;
    public const int BAD_ARGS = 2;
    public const int JOB_DOES_NOT_EXIST = 3;

    public static int Run(string[] args)
    {
        if (args.Length == 1)
        {
            Configuration configuration = new Configuration(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\EasySave\\" + "config.json");
            configuration.LoadConfiguration();
            int id = int.Parse(args[0]);
            SaveJob saveJob = configuration.GetSaveJob(id);
            if (saveJob == null)
            {
                LoggerUtility.WriteLog(LoggerUtility.Warning, "Cant found SaveJob with id: "+args[0].ToString());
                return JOB_DOES_NOT_EXIST;
            }
            else
            {
                LoggerUtility.WriteLog(LoggerUtility.Info, "Saving :  id: "+id.ToString()+", name : "+saveJob.Name+" from ("+saveJob.Source+") to (" + saveJob.Destination + ")");

            }
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            DirCopy dirCopy = new DirCopy();
            dirCopy.CopyDir(saveJob.Source, saveJob.Destination);
            stopwatch.Stop();
            Console.WriteLine($"Execution Time: {stopwatch.ElapsedMilliseconds} ms");
            LoggerUtility.WriteLog(LoggerUtility.Info, "The savejob took "+stopwatch.ElapsedMilliseconds+" ms");
            LoggerUtility.WriteLog(LoggerUtility.Info, "Save :  id: "+id.ToString()+", name : "+saveJob.Name+" from ("+saveJob.Source+") to (" + saveJob.Destination + ") is save");
            return OK;
        }
        else
        {
            LoggerUtility.WriteLog(LoggerUtility.Warning, "Some args are missing or incorect");
            return BAD_ARGS;
        }
    }
}