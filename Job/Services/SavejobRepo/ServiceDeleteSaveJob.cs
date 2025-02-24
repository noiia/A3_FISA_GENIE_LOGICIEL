using Config;
using Job.Config.i18n;
using Job.Config;
using Job.Services;
using Logger;

namespace Job.Services;

public class ServiceDeleteSaveJob
{
    public static (int, string) Run(Configuration configuration, int? id, string? name)
    {
        if (id is not null ^ name is not "")
        {
            SaveJob? saveJob = null;
            if (id is not null)
            {
                saveJob = configuration.GetSaveJob(id ?? -1);
            }
            else
            {
                saveJob = configuration.GetSaveJob(name ?? string.Empty);
            }

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
        else
        {
            string returnSentence = $"{Translation.Translator.GetString("BadArgsGiven") ?? String.Empty} {id} {name}";
            LoggerUtility.WriteLog(LoggerUtility.Warning, returnSentence);
            return (2, returnSentence);
        }
    }
}