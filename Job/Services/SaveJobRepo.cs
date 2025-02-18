using System.Collections.Concurrent;
using Job.Config;

namespace Job.Services;

public class SaveJobRepo
{
    private static Configuration _configuration;
    private static ThreadPoolManager _pool;
    public SaveJobRepo(Configuration config, ThreadPoolManager pool)
    {
        _configuration = config;
        _pool = pool;
    }
    
    public static (int, string) AddSaveJob(string name, string sourcePath, string destinationPath, string saveType)
    {
        var value = _pool.QueueTask(async () => { return ServiceAddSaveJob.Run(_configuration, name, sourcePath, destinationPath, saveType); });
        return (value.Result.Item1, value.Result.Item2);
    }
    
    public static (int,string) ExecuteSaveJob(string name)
    {
        int? id = null;
        var value = _pool.QueueTask(async () => { return ServiceExecSaveJob.Run(_configuration, id, name); });
        return (value.Result.Item1, value.Result.Item2);
    }
    public static (int,string) ExecuteSaveJob(int id)
    {
        string? name = "";
        var value = _pool.QueueTask(async () => { return ServiceExecSaveJob.Run(_configuration, id, name); });
        return (value.Result.Item1, value.Result.Item2);
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