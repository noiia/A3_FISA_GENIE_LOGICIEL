using System.Diagnostics;

using Config;
using Config.i18n;
using Job.Config;

namespace Controller;

public class AddSaveJob
{
    public static string Execute(int errorCode , string name, string srcPath, string destPath, string type)
    {
        switch (errorCode) // #TODO : mettre une valeur valable
        {
            case 1:
                return Translation.Translator.GetString("SjCreatedSuccesfully");
            case 2:
                return $"{Translation.Translator.GetString("AddSjBadArgs")} {name}";
            case 3:
                return $"{Translation.Translator.GetString("SaveAlreadyExist")}";
            case 4:
                return $"{Translation.Translator.GetString("SrcDirNotExists")} {srcPath}";
            case 5:
                return $"{Translation.Translator.GetString("DstDirNotExist")} {destPath}";
            case 6:
                return $"{Translation.Translator.GetString("InvalidType")} {type} {Translation.Translator.GetString("AddSJBadType")}";
            default:
                return String.Empty;
        }
    }
}