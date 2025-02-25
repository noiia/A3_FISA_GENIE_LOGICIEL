using System.Diagnostics;
using Job.Config;
using Job.Config.i18n;
using Job.Services;
using Logger;

namespace Job.Controller;

public class DeleteSaveJob
{
    private static Configuration _configuration;
    public static (int, string) Execute(List<int>  ids, string separator)
    {
        _configuration = ConfigSingleton.Instance();
        LoggerUtility.WriteLog(_configuration.GetLogType(), LoggerUtility.Info, Translation.Translator.GetString("DelSjCallWithArgs") + string.Join(" ", ids));
        if (ids.Count > 1 && separator is "" or " ")
        {
            return (3, $"{Translation.Translator.GetString("TooMuchIdWithoutSep")} {separator} {ids}");
        }
        
        int returnCode;
        string message;
        switch (separator)
        {
            case ";":
                foreach (int id in ids)
                {
                    (returnCode, message) = SaveJobRepo.DeleteSaveJob(id);
                    if (returnCode is 2)
                    {
                        return (returnCode, message);
                    }
                }
                return (1, $"{Translation.Translator.GetString("SjDelSuccesfully")} {ids}");
            
            case ",":
                for (int i = ids[0]; i <= ids[1]; i++)
                {
                    (returnCode, message) = SaveJobRepo.DeleteSaveJob(i);
                    if (returnCode is 2)
                    {
                        return (returnCode, message);
                    }
                }
                return (1, $"{Translation.Translator.GetString("SjDelSuccesfully")} {ids}");
            
            case "":
                (returnCode, message) = SaveJobRepo.DeleteSaveJob(ids[0]);
                if (returnCode is 2)
                {
                    return (returnCode, message);
                }
                return (1, $"{Translation.Translator.GetString("SjDelSuccesfully")} {ids[0]}");
            
            default:
                return (3, $"{Translation.Translator.GetString("SeparatorNotReco")} {separator}");
        }
    }

    public static (int, string) Execute(string name)
    {
        (int returnCode, string message) = SaveJobRepo.DeleteSaveJob(name);
        if (returnCode is 2)
        {
            return (returnCode, message);
        }
        return (1, $"{Translation.Translator.GetString("SjDelSuccesfully")} {name}");
    }
}