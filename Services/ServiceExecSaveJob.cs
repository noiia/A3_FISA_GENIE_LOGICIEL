using Config;

namespace Services;

public class ServiceExecSaveJob
{
    public const int OK = 1;
    public const int BAD_ARGS = 2;
    public const int JOB_DOES_NOT_EXIST = 3;

    public static int Run(string[] args)
    {
        if (args.Length == 1)
        {
            Configuration configuration = new Configuration(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\EasySave\\" + "config.json");
            configuration.LoadConfiguration();
            int id = int.Parse(args[0]);
            SaveJob saveJob = configuration.GetSaveJob(id);
            if (saveJob == null)
            {
                return JOB_DOES_NOT_EXIST;
            }
            DirCopy dirCopy = new DirCopy();
            dirCopy.CopyDir(saveJob.Source, saveJob.Destination);
            return OK;
        }
        else
        {
            return BAD_ARGS;
        }
    }
}