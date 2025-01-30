namespace ConsoleApp1;

public class ConfigFile
{
    private SaveJob[] saveJobs;

    public ConfigFile(SaveJob[] saveJobs)
    {
        this.saveJobs = saveJobs;
    }

    public SaveJob[] SaveJobs
    {
        get => SaveJobs;
        set => SaveJobs = value ?? throw new ArgumentNullException(nameof(value));
    }
}