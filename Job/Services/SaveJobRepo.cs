using System.Collections.Concurrent;
using Job.Config;

namespace Job.Services;

public class SaveJobRepo
{
    private Configuration _configuration;
    private ThreadPoolManager _pool;
    public SaveJobRepo(Configuration config, ThreadPoolManager pool)
    {
        _configuration = config;
        _pool = pool;
    }
    
    public static void AddSaveJob()
    {
        
    }

    public (int,string) ExecuteSaveJob(string? name, int? id)
    {
        var value = _pool.QueueTask(async () => { return ServiceExecSaveJob.Run(_configuration, id, name); });
        return (value.Result.Item1, value.Result.Item2);
    }
    
    public static void DeleteSaveJob()
    {
        
    }
}