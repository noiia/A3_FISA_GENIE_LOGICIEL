using System.Collections.Concurrent;
using System.Runtime.InteropServices;
using Job.Config;

namespace Job.Services;

public class SaveJobRepo
{
    private static Configuration _configuration;
    private static ThreadPoolManager? _pool;
    public SaveJobRepo(Configuration config, int threads)
    {
        _configuration = config;
        // #TODO verif fonctionnel, set une seule fois  
        if (_pool == null)
        {
            _pool = new ThreadPoolManager(threads);
        }
    }
    
    public static (int, string) AddSaveJob(string name, string sourcePath, string destinationPath, string saveType)
    {
        var value = _pool.QueueTask(async () => { return ServiceAddSaveJob.Run(_configuration, name, sourcePath, destinationPath, saveType); });
        return (value.Result.Item1, value.Result.Item2);
    }
    
    public static (int, string, DateTime) ExecuteSaveJob(string name)
    {
        int? id = null;
        var value = _pool.QueueTask(async () => { return ServiceExecSaveJob.Run(_configuration, id, name); });
        return (value.Result.Item1, value.Result.Item2, value.Result.Item3);
    }
    public static (int, string, DateTime) ExecuteSaveJob(int id)
    {
        string? name = "";
        var value = _pool.QueueTask(async () => { return ServiceExecSaveJob.Run(_configuration, id, name); });
        return (value.Result.Item1, value.Result.Item2, value.Result.Item3);
    }
    
    public static (int,string) DeleteSaveJob(int id)
    {
        string? name = "";
        var value = _pool.QueueTask(async () => { return ServiceDeleteSaveJob.Run(_configuration, id, name); });
        return (value.Result.Item1, value.Result.Item2);
    }
    
    public static (int,string) DeleteSaveJob(string name)
    {
        int? id = null;
        var value = _pool.QueueTask(async () => { return ServiceDeleteSaveJob.Run(_configuration, id, name); });
        return (value.Result.Item1, value.Result.Item2);
    }
}