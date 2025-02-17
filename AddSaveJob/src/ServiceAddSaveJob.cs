using Config;
using Logger;
using Services;

namespace AddSaveJob;

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
                return ReturnCodes.NAME_ALREADY_USE;
            }

            if (!Directory.Exists(args[1]))
            {
                LoggerUtility.WriteLog(LoggerUtility.Warning, "Source folder does not exist (" + args[1] + ")");
                return ReturnCodes.SOURCE_DOES_NOT_EXIST;
            }

            if (!Directory.Exists(args[2]))
            {
                LoggerUtility.WriteLog(LoggerUtility.Warning, "Destiantion folder does not exist (" + args[2] + ")");
                return ReturnCodes.DESTINANTION_DOES_NOT_EXIST;
            }

            if (!(args[3].ToLower() == ReturnCodes.TYPE_DIFF || args[3].ToLower() == ReturnCodes.TYPE_FULL))
            {
                LoggerUtility.WriteLog(LoggerUtility.Warning, "Type args inst correct (" + args[3] + ")");
                return ReturnCodes.TYPE_DOES_NOT_EXIST;
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
                return ReturnCodes.OK;
            // }
        }
        else
        {
            LoggerUtility.WriteLog(LoggerUtility.Warning, "Some args are missing or incorrect");
            return ReturnCodes.BAD_ARGS;
        }
    }
}