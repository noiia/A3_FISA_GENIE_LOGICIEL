using Config;

namespace Services;

public class ServiceListSaveJob
{
    public static SaveJob[] Run(string[] args, Configuration configuration)
    {
        SaveJob[] saveJobs = configuration.GetSaveJobs();
        return saveJobs;
    }
}