using Config;
using Logger;
using Services;

namespace DeleteSaveJob;

public class ServiceDeleteSaveJob
{
    public static int Run(string[] args, Configuration configuration)
    {
        if (args.Length == 1)
        {
            int id = int.Parse(args[0]);
            SaveJob saveJob = configuration.GetSaveJob(id);
            if (saveJob == null)
            {
                LoggerUtility.WriteLog(LoggerUtility.Warning, "SaveJob does not exist ("+args[0]+")");
                return ReturnCodes.JOB_DOES_NOT_EXIST;
            }
            configuration.DeleteSaveJob(id);
            LoggerUtility.WriteLog(LoggerUtility.Info, "SaveJob has been deleted id: "+args[0]);
            return ReturnCodes.OK;
        }
        else
        {
            LoggerUtility.WriteLog(LoggerUtility.Warning, "Some args are missing or incorect");
            return ReturnCodes.BAD_ARGS;
        }
    }
}