using Config;

namespace ListSaveJobs;

public class ListSaveJobs
{
    public static SaveJob[] Run(string[] args, Configuration configuration)
    {
        configuration.LoadConfiguration();
        SaveJob[] saveJobs = configuration.GetSaveJobs();
        // foreach (SaveJob saveJob in saveJobs)
        // {
        //     Console.WriteLine(saveJob.Name);
        // }
        return saveJobs;
    }
}