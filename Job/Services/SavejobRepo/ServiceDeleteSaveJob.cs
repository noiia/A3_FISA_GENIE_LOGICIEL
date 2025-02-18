using Config;
using Job.Config.i18n;
using Job.Config;
using Job.Services;
using Logger;

namespace Job.Services;

public class ServiceDeleteSaveJob
{
    public static (int, string) Run(Configuration configuration, int id)
    {
        SaveJob? saveJob = configuration.GetSaveJob(id);

        string returnSentence;
        if (saveJob is null)
        {
            returnSentence = $"{Translation.Translator.GetString("SjExecSuccesfully")}{id})";
            LoggerUtility.WriteLog(LoggerUtility.Warning, returnSentence);
            return (2, returnSentence);
        }
        configuration.DeleteSaveJob(saveJob);
        returnSentence = $"{Translation.Translator.GetString("SjDelSuccesfully")}{id})";
        LoggerUtility.WriteLog(LoggerUtility.Info, returnSentence);
        return (1, returnSentence);
    }
}