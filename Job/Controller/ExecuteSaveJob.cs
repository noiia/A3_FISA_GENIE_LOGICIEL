using System.Diagnostics;

using Config;
using Config.i18n;
using Job.Config;
using Job.Services;
using Logger;

namespace Controller;

public class ExecuteSaveJob
{
    public static string Execute(int[] ids, string separator)
    {
        LoggerUtility.WriteLog(LoggerUtility.Info, $"{Translation.Translator.GetString("SjExecWith")} {string.Join(" ", ids, separator)}");
        if (separator is ";")
        {
            foreach (var id in ids)
            {
                (int returnCode, string message) = SaveJobRepo.ExecuteSaveJob(id);
                switch (returnCode)
                {
                    case 1:
                        return Translation.Translator.GetString("SjExecSuccesfully");
                    case 2:
                        return message;
                    default:
                        return String.Empty;
                }
            }
        }
        else if(separator is ",")
        {
            for (int i = ids[0]; i <= ids[1]; i++)
            {
                (int returnCode, string message) = SaveJobRepo.ExecuteSaveJob(i);
                switch (returnCode)
                {
                    case 1:
                        return Translation.Translator.GetString("SjExecSuccesfully");
                    case 2:
                        return message;
                    default:
                        return String.Empty;
                }
            }

        }
        else
        {
            (int returnCode, string message) = SaveJobRepo.ExecuteSaveJob(ids[0]);
            switch (returnCode)
            {
                case 1:
                    return Translation.Translator.GetString("SjExecSuccesfully");
                case 2:
                    return message;
                default:
                    return String.Empty;
            }
        }
        return String.Empty;
    }
}