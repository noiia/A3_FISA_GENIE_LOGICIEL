using Config;
using Logger;

namespace Job.Services;

public class ServiceAddSaveJob
{
    public static int Run(Configuration configuration, string name, string sourcePath, string destinationPath, string saveType)
    {
        if (name == String.Empty || sourcePath == String.Empty || destinationPath == String.Empty || saveType == String.Empty)
        {
            return 4;
        }
        
        SaveJob saveJob = configuration.GetSaveJob(name);
        if (saveJob != null)
        {
            LoggerUtility.WriteLog(LoggerUtility.Warning, "SaveJob name is already use (" + name + ")");
            return 1;
        }

        if (!Directory.Exists(sourcePath))
        {
            LoggerUtility.WriteLog(LoggerUtility.Warning, "Source folder does not exist (" + sourcePath + ")");
            return 1;
        }

        if (!Directory.Exists(destinationPath))
        {
            LoggerUtility.WriteLog(LoggerUtility.Warning, "Destiantion folder does not exist (" + destinationPath + ")");
            return 1;
        }

        if (!(saveType.ToLower() == "diff" || saveType.ToLower() == "full"))
        {
            LoggerUtility.WriteLog(LoggerUtility.Warning, "Type args inst correct (" + saveType + ")");
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
            configuration.AddSaveJob(nextId, name, sourcePath, destinationPath, DateTime.Now, DateTime.Now, saveType);
            LoggerUtility.WriteLog(LoggerUtility.Info,
                "SaveJob is created : {" + "id: " + nextId.ToString() + ", source: " + name + ", destination: " +
                sourcePath + ", type: " + saveType + "}");
            return 1;
        // }
    }
}