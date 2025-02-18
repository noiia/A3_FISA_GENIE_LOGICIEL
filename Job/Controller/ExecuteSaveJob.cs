using System.Diagnostics;

using Config;
using Config.i18n;
using Job.Config;
using Logger;

namespace Controller;

public class ExecuteSaveJob
{
    public static string Execute(int[] ids, string separator)
    {
        LoggerUtility.WriteLog(LoggerUtility.Info, $"{Translation.Translator.GetString("SjExecWith")} {string.Join(" ", args)}");
        int intType;
        if(separator == ";")
        {
            string[] listIds = args[0].Split(';');
            foreach (var id in listIds)
            {
                
                switch ()
                {
                    case 1:
                        return Translation.Translator.GetString("SjExecSuccesfully");
                    case 2:
                        return Translation.Translator.GetString("ExecSjBadArgs");
                    case 3:
                        return $"{Translation.Translator.GetString("SjNotExist")} {id} )";
                    default:
                        return String.Empty;
                }
            }
        }
        else if(args[0].Contains(","))
        {
            int min = int.Parse(args[1].Split(',')[0]);
            int max = int.Parse(args[1].Split(',')[1]);
            for (int i = min; i <= max; i++)
            {
                switch ()
                {
                    case 1:
                        return Translation.Translator.GetString("SjExecSuccesfully");
                    case 2:
                        return Translation.Translator.GetString("ExecSjBadArgs");
                    case 3:
                        return $"{Translation.Translator.GetString("SjNotExist")} {i} )";
                    default:
                        return String.Empty;
                }
            }

        }
        else
        {
            if(int.TryParse(args[0], out intType))
            {
                switch ()
                {
                    case 1:
                        return Translation.Translator.GetString("SjExecSuccesfully");
                    case 2:
                        return Translation.Translator.GetString("ExecSjBadArgs");
                    case 3:
                        return $"{Translation.Translator.GetString("SjNotExist")} {string.Join(' ', args)} )";
                    default:
                        return String.Empty;
                }   
            }
            else
            {
                switch ()
                {
                    case 1:
                        return Translation.Translator.GetString("SjExecSuccesfully");
                    case 2:
                        return Translation.Translator.GetString("ExecSjBadArgs");
                    case 3:
                        return $"{Translation.Translator.GetString("SjNotExist")} {string.Join(' ', args)} )";
                    default:
                        return String.Empty;
                }   
            }
        }

        return String.Empty;
    }
}