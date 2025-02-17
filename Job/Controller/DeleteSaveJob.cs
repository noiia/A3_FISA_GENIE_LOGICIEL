using System.Diagnostics;

using Config;
using Config.i18n;
using Logger;

namespace Controller;

public class DeleteSaveJob
{
    public static string Execute(int[] id)
    {
        LoggerUtility.WriteLog(LoggerUtility.Info, Translation.Translator.GetString("DelSjCallWithArgs") + string.Join(" ", id));
        
        switch ()
        {
            case 1:
                return Translation.Translator.GetString("SjDelSuccesfully");
            case 2:
                return Translation.Translator.GetString("DelSjBadArgs");
            case 3:
                return String.Join(Translation.Translator.GetString("SjNotExist"), id);
            default:
                return String.Empty;
        }
    }
}