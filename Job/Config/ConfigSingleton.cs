using Job.Config;

public class ConfigSingleton
{
    private static ConfigSingleton instance;

    private ConfigSingleton()
    {
        _configuration = new Configuration(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                                           "\\EasySave\\" + "config.json");
    }

    public Configuration _configuration { get; }

    public static Configuration Instance()
    {
        if (instance == null) instance = new ConfigSingleton();
        return instance._configuration;
    }
}