using System.Collections.Concurrent;
using System.Runtime.InteropServices;
using Job.Config;
using Job.Controller;

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
    
    // public static (int,string) ResumeSaveJob(int id)
    // {
        // var value = _pool.QueueTask(async () => { return ServiceResumeSaveJob.Run(_configuration, id); });
        // return (value.Result.Item1, value.Result.Item2);
    // }
    
    public static (int,string) ResumeSaveJob(int id)
    {
        // ServiceResumeSaveJob.GetFilesForResume(_configuration, id);
        var value = _pool.QueueTask(async () => { return ServiceResumeSaveJob.Run(_configuration, id); });
        return (value.Result.Item1, value.Result.Item2);
        return (0,"");
    }

    
}