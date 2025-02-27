namespace Job.Config;

public class SetLogPath
{
    public const int OK = 1;
    public const int NOT_A_DIR = 2;
    public const int BAD_ARGS = 3;

    public static int Run(string[] args, Configuration configuration)
    {
        if (args.Length == 1)
        {
            if (!Directory.Exists(args[0])) return NOT_A_DIR;

            configuration.SetLogPath(args[0]);
            return OK;
        }

        return BAD_ARGS;
    }
}