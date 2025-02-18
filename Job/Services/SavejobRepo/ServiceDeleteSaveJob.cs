using Config;
using Job.Config;
using Job.Services;
using Logger;

namespace Job.Services;

public class ServiceDeleteSaveJob
{
    //CLI
    public static int Run(Configuration configuration, string[] args)
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
                LoggerUtility.WriteLog(LoggerUtility.Warning, "SaveJob does not exist ("+args[0]+")");
                return 1;
            }
            configuration.DeleteSaveJob(saveJob);
            LoggerUtility.WriteLog(LoggerUtility.Info, "SaveJob has been deleted id: "+args[0]);
            return 1;
        }
        else
        {
            LoggerUtility.WriteLog(LoggerUtility.Warning, "Some args are missing or incorect");
            return 1;
        }
    }
    
    //Avalonia
    public static int Run(Configuration configuration, int id)
    {
        SaveJob? saveJob = null;
        saveJob = configuration.GetSaveJob(id);
        if (saveJob == null)
        {
            LoggerUtility.WriteLog(LoggerUtility.Warning, "SaveJob does not exist ("+ id +")");
            return 1;
        }
        configuration.DeleteSaveJob(saveJob);
        LoggerUtility.WriteLog(LoggerUtility.Info, "SaveJob has been deleted id: "+id);
        return 1;


        // if (args.Length == 1)
        // {
        //     int id;
        //     SaveJob? saveJob = null;
        //     if (int.TryParse(args[0], out id))
        //     {
        //         saveJob = configuration.GetSaveJob(id);   
        //     }
        //     else
        //     {
        //         saveJob = configuration.GetSaveJob(args[0]);
        //     }
        //     if (saveJob == null)
        //     {
        //         LoggerUtility.WriteLog(LoggerUtility.Warning, "SaveJob does not exist ("+args[0]+")");
        //         return 1;
        //     }
        //     configuration.DeleteSaveJob(saveJob);
        //     LoggerUtility.WriteLog(LoggerUtility.Info, "SaveJob has been deleted id: "+args[0]);
        //     return 1;
        // }
        // else
        // {
        //     LoggerUtility.WriteLog(LoggerUtility.Warning, "Some args are missing or incorect");
        //     return 1;
        // }
    }
    
    
    
}