using Job.Config.i18n;
using Job.Config;
using Job.Services;
using Logger;

namespace Job.Services;

public class ServiceAddSaveJob
{
    public static (int, string) Run(Configuration configuration, string name, string sourcePath, string destinationPath, string saveType)
    {
        if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(sourcePath) || string.IsNullOrEmpty(destinationPath) || string.IsNullOrEmpty(saveType))
        {
            return (4, $"{Translation.Translator.GetString("MissingArgs")} {name} {sourcePath} {destinationPath} {saveType}");
        }

        string returnSentence;
        SaveJob saveJob = configuration.GetSaveJob(name);
        if (saveJob != null)
        {
            returnSentence = $"{Translation.Translator.GetString("MissingArgs")} ({name})";
            LoggerUtility.WriteLog(LoggerUtility.Warning, returnSentence);
            return (3, returnSentence);
        }

        if (!Directory.Exists(sourcePath))
        {
            returnSentence = $"{Translation.Translator.GetString("SrcDirNotExists")} ({sourcePath})";
            LoggerUtility.WriteLog(LoggerUtility.Warning, returnSentence);
            return (3, returnSentence);
        }

        if (!Directory.Exists(destinationPath))
        {
            returnSentence = $"{Translation.Translator.GetString("DstDirNotExists")} ({destinationPath})";
            LoggerUtility.WriteLog(LoggerUtility.Warning, returnSentence);
            return (3, returnSentence);
        }

        if (!(saveType.ToLower() == "diff" || saveType.ToLower() == "full"))
        {
            returnSentence = $"{Translation.Translator.GetString("InvalidType")} ({saveType})";
            LoggerUtility.WriteLog(LoggerUtility.Warning, returnSentence);
            return (3, returnSentence);
        }

        int nextId = configuration.FindFirstFreeId();

        configuration.AddSaveJob(nextId, name, sourcePath, destinationPath, DateTime.Now, DateTime.Now, "STOP", saveType);
        returnSentence = $"{Translation.Translator.GetString("SjCreatedSuccesfully")} id : {nextId.ToString()} source : {sourcePath} destination : {destinationPath} save type : {saveType}";

        LoggerUtility.WriteLog(LoggerUtility.Info,
            $"SaveJob is created : {{ id: {nextId.ToString()}, source: {sourcePath}, destination: {destinationPath}, type: {saveType} }}");
        return (1, returnSentence);
    }
}