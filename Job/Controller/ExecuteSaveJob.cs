using System.Diagnostics;

using Job.Config.i18n;
using Job.Services;
using Logger;

namespace Job.Controller;

public class ExecuteSaveJob
{
    public static (int, string, Dictionary<int, DateTime>) Execute(List<int>  ids, string separator)
    {
        LoggerUtility.WriteLog(LoggerUtility.Info, $"{Translation.Translator.GetString("SjExecWith")} {string.Join(" ", ids, separator)}");
        if (ids.Count > 1 && separator is "" or " ")
        {
            return (3, $"{Translation.Translator.GetString("TooMuchIdWithoutSep")} {separator} {ids}", null)!;
        }
        
        int returnCode;
        string message;
        DateTime taskEndDate;
        Dictionary<int, DateTime> endDates = new Dictionary<int, DateTime>();
        switch (separator)
        {
            case ";":
                string listId = "";
               
                foreach (var id in ids)
                {
                    (returnCode, message, taskEndDate) = SaveJobRepo.ExecuteSaveJob(id);
                    endDates[id] = taskEndDate;
                    if (returnCode is 2)
                    {
                        return (returnCode, message + listId, endDates);
                    } 
                    listId += $"{id}, ";
                }
                return (1, $"{Translation.Translator.GetString("SjExecSuccesfully")} {listId}", endDates);
            
            case  ",":
                for (int i = ids[0]; i <= ids[1]; i++)
                {
                    (returnCode, message, taskEndDate) = SaveJobRepo.ExecuteSaveJob(i);
                    endDates[i] = taskEndDate;
                    if (returnCode is 2)
                    {
                        return (returnCode, message, endDates);
                    }
                }
                return (1, $"{Translation.Translator.GetString("SjExecSuccesfully")} : {ids[0]} - {ids[1]}", endDates);
            
            case "":
                (returnCode, message, taskEndDate) = SaveJobRepo.ExecuteSaveJob(ids[0]);
                endDates[ids[0]] = taskEndDate;
                if (returnCode is 2)
                {
                    return (returnCode, message, endDates);
                }
                return (1, $"{message}", endDates);
            
            default:
                return (3, $"{Translation.Translator.GetString("SeparatorNotReco")} {separator}", endDates);
        }
    }
}