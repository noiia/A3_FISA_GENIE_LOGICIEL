using EasySave;

namespace Services;

public class ServiceListSaveJob
{
    public static SaveJob[] Run(string[] args)
    {
        Configuration configuration = new Configuration( Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\EsaySave\\" + "config.json");
        configuration.LoadConfiguration();
        SaveJob[] saveJobs = configuration.GetSaveJobs();
        return saveJobs;
    }
}