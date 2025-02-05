using Config;

namespace Services;

public class ServiceAddSaveJob
{
    public const int OK = 1;
    public const int BAD_ARGS = 2;
    public const int NAME_ALREADY_USE = 3;
    public const int SOURCE_DOES_NOT_EXIST = 4;
    public const int DESTINANTION_DOES_NOT_EXIST = 5;
    public const int TYPE_DOES_NOT_EXIST = 6;
    public const string TYPE_FULL = "full";
    public const string TYPE_DIFF = "diff";
    
    public static int Run(string[] args)
    {
        if (args.Length == 4)
        {
            Configuration configuration = new Configuration( Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\EasySave\\" + "config.json");
            configuration.LoadConfiguration();
            SaveJob saveJob = configuration.GetSaveJob(args[0]);
            if (saveJob != null)
            {
                return NAME_ALREADY_USE;
            }
            if (!Directory.Exists(args[1]))
            {
                return SOURCE_DOES_NOT_EXIST;
            }
            if (!Directory.Exists(args[2]))
            {
                return DESTINANTION_DOES_NOT_EXIST;
            }
            if (!(args[3].ToLower() == TYPE_DIFF || args[3].ToLower() == TYPE_FULL))
            {
                return TYPE_DOES_NOT_EXIST;
            }
            int nextId = configuration.FindFirstFreeId();
            configuration.AddSaveJob(nextId,args[0], args[1], args[2], DateTime.Now, DateTime.Now, args[3]);
            return OK;
        }
        else
        {
            return BAD_ARGS;
        }
    }
}