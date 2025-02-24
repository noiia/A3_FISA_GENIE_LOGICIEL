using System;
using Config;
using Job.Config;

public class ConfigSingleton
{
    public Configuration _configuration { get; private set; }
    private static ConfigSingleton instance;
    private ConfigSingleton()
    {
        _configuration = new Configuration(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                                           "\\EasySave\\" + "config.json");
    }
    
    public static Configuration Instance()
    {
        if (instance == null)
        {
            instance = new ConfigSingleton();
        }
        return instance._configuration;
    }
}


