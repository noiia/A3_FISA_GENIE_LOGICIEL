using System.Diagnostics;

using Job.Config.i18n;
using Job.Services;
using Logger;

namespace Job.Controller;

public class ExecuteSaveJob
{
    public static (int, string) Execute(List<int>  ids, string separator)
    {
        LoggerUtility.WriteLog(LoggerUtility.Info, $"{Translation.Translator.GetString("SjExecWith")} {string.Join(" ", ids, separator)}");
        if (ids.Count > 1 && separator is "" or " ")
        {
            return (3, $"{Translation.Translator.GetString("TooMuchIdWithoutSep")} {separator} {ids}");
        }
        
        int returnCode;
        string message;
        switch (separator)
        {
            case ";":
                string listId = "";
                foreach (var id in ids)
                {
                    (returnCode, message) = SaveJobRepo.ExecuteSaveJob(id);
                    if (returnCode is 2)
                    {
                        return (returnCode, message + listId);
                    } 
                    listId += $"{id}, ";
                }
                return (1, $"{Translation.Translator.GetString("SjExecSuccesfully")} {listId}");
            
            case  ",":
                for (int i = ids[0]; i <= ids[1]; i++)
                {
                    (returnCode, message) = SaveJobRepo.ExecuteSaveJob(i);
                    if (returnCode is 2)
                    {
                        return (returnCode, message);
                    }
                }
                return (1, $"{Translation.Translator.GetString("SjExecSuccesfully")} : {ids[0]} - {ids[1]}");
            
            case "":
                (returnCode, message) = SaveJobRepo.ExecuteSaveJob(ids[0]);
                if (returnCode is 2)
                {
                    return (returnCode, message);
                }
                return (1, $"{message}");
            
            default:
                return (3, $"{Translation.Translator.GetString("SeparatorNotReco")} {separator}");
        }
    }
}