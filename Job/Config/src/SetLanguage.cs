namespace Job.Config;

public class SetLanguage
{
    public const int OK = 1;
    public const int NOT_A_LANGUAGE = 2;
    public const int BAD_ARGS = 3;

    public static int Run(string[] args, Configuration configuration)
    {
        if (args.Length == 1)
        {
            if (!(args[0] == "fr" || args[0] == "en")) return NOT_A_LANGUAGE;

            configuration.SetLanguage(args[0]);
            return OK;
        }

        return BAD_ARGS;
    }
}