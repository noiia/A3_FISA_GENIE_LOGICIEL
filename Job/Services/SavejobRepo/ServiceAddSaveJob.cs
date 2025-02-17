using Config;
using Job.Services;
using Logger;
using Services;
using Configuration = Job.Services.Configuration;

namespace Services;

public class ServiceAddSaveJob
{
    public static int Run(string[] args, Configuration configuration)
    {
        if (args.Length == 4)
        {
            SaveJob saveJob = configuration.GetSaveJob(args[0]);
            if (saveJob != null)
            {
                LoggerUtility.WriteLog(LoggerUtility.Warning, "SaveJob name is already use (" + args[0] + ")");
                return 1;
            }

            if (!Directory.Exists(args[1]))
            {
                LoggerUtility.WriteLog(LoggerUtility.Warning, "Source folder does not exist (" + args[1] + ")");
                return 1;
            }

            if (!Directory.Exists(args[2]))
            {
                LoggerUtility.WriteLog(LoggerUtility.Warning, "Destination folder does not exist (" + args[2] + ")");
                return 1;
            }

            if (!(args[3].ToLower() == "diff" || args[3].ToLower() == "full"))
            {
                LoggerUtility.WriteLog(LoggerUtility.Warning, "Type args inst correct (" + args[3] + ")");
                return 1;
            }

            int nextId = configuration.FindFirstFreeId();
            // if (nextId == -1)
            // {
            //     LoggerUtility.WriteLog(LoggerUtility.Info, "Cant add more than 5 SaveJob");
            //     return ReturnCodes.MORE_THAN_5_SAVEJOB;
            // }
            // else
            // {
                configuration.AddSaveJob(nextId, args[0], args[1], args[2], DateTime.Now, DateTime.Now, args[3]);
                LoggerUtility.WriteLog(LoggerUtility.Info,
                    "SaveJob is created : {" + "id: " + nextId.ToString() + ", source: " + args[0] + ", destination: " +
                    args[1] + ", type: " + args[3] + "}");
                return 1;
            // }
        }
        else
        {
            LoggerUtility.WriteLog(LoggerUtility.Warning, "Some args are missing or incorrect");
            return 1;
        }
    }
}