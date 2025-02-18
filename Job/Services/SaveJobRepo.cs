using Config;
using Job.Config;

namespace Job.Services;

public class SaveJobRepo(Configuration config)
{
    private Configuration _configuration = config;

    public void AddSaveJob(string name, string sourcePath, string destinationPath, string saveType)
    {
        int returnCode;
        returnCode = ServiceAddSaveJob.Run(_configuration, name, sourcePath, destinationPath, saveType);
        Controller.AddSaveJob.Execute(returnCode, name, sourcePath, destinationPath, saveType);
    }
    
    public static void ExecSaveJob()
    {
        
    }
    
    public static void DeleteSaveJob()
    {
        
    }
}