using Config;

namespace Job.Services;

public class SaveJobRepo(Configuration config)
{
    private Configuration _configuration = config;

    public void AddSaveJob(string name, string sourcePath, string destinationPath, string saveType)
    {
        ServiceAddSaveJob.Run(_configuration, name, sourcePath, destinationPath, saveType);
    }
    
    public static void ExecSaveJob()
    {
        
    }
    
    public static void DeleteSaveJob()
    {
        
    }
}