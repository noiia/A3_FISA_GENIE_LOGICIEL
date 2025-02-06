using Config;
using Logger;

namespace Services;

public class ServiceDeleteSaveJob
{
    public const int OK = 1;
    public const int BAD_ARGS = 2;
    public const int JOB_DOES_NOT_EXIST = 3;

    public static int Run(string[] args)
    {
        if (args.Length == 1)
        {
            Configuration configuration =
                new Configuration(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                                  "\\EasySave\\" + "config.json");
            configuration.LoadConfiguration();
            int id = int.Parse(args[0]);
            SaveJob saveJob = configuration.GetSaveJob(id);
            if (saveJob == null)
            {
                LoggerUtility.WriteLog(LoggerUtility.Warning, "SaveJob does not exist ("+args[0]+")");
                return JOB_DOES_NOT_EXIST;
            }
            configuration.DeleteSaveJob(id);
            LoggerUtility.WriteLog(LoggerUtility.Info, "SaveJob has been deleted id: "+args[0]);
            return OK;
        }
        else
        {
            LoggerUtility.WriteLog(LoggerUtility.Warning, "Some args are missing or incorect");
            return BAD_ARGS;
        }
    }
}