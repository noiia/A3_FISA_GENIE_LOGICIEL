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
        if (_pool == null)
        {
            _pool = new ThreadPoolManager(threads);
        }
    }

    
    private static List<BigFileTracker> listBigFileTrackers = new List<BigFileTracker>();
    private static Dictionary<int, Dictionary<int, List<string>>> listBigFile = new Dictionary<int, Dictionary<int, List<string>>>();

    private static void GetBigFiles(object sender, TrackerBigFileEventArgs eventArgs)
    {
        int importance = eventArgs.TooBigFiles.Values.SelectMany(files => files).Count(files => _configuration.GetFileExtension().Contains(Path.GetExtension(files)));
        listBigFile[importance] = eventArgs.TooBigFiles;
    }

    public static Dictionary<int, List<string>> SchedulingBigFileTransfert()
    {
        if (listBigFile.Count == 0)
        {
            return new Dictionary<int, List<string>>() ;
        }

        var highestPriorityJob = listBigFile.OrderByDescending(x => x).First();
        
        return new Dictionary<int, List<string>> {{highestPriorityJob.Value.Keys.First(), highestPriorityJob.Value.Values.SelectMany(files => files).ToList()}};
    }

    public static bool RemoveFileTransfered(int id)
    {
        return listBigFile.Remove(listBigFile.Keys.FirstOrDefault(x => x == id));
    }
    
    public static (int, string) AddSaveJob(string name, string sourcePath, string destinationPath, string saveType)
    {
        var value = _pool.QueueTask(async () => { return ServiceAddSaveJob.Run(_configuration, name, sourcePath, destinationPath, saveType); });
        return (value.Result.Item1, value.Result.Item2);
    }
    
    public static async Task<(int, string)> ExecuteSaveJob(string name, LockTracker lockTracker)
    {
        int? id = null;
        BigFileTracker bigFileTracker = new BigFileTracker();
        listBigFileTrackers.Add(bigFileTracker);
        bigFileTracker.OnTrackerChanged += GetBigFiles;
        
        var value = await _pool.QueueTask(async () => { return ServiceExecSaveJob.Run(_configuration, lockTracker, bigFileTracker, id, name); });
        return (value.Item1, value.Item2);
    }
    public static async Task<(int, string)> ExecuteSaveJob(int id, LockTracker lockTracker)
    {
        string? name = "";
        BigFileTracker bigFileTracker = new BigFileTracker();
        listBigFileTrackers.Add(bigFileTracker);
        bigFileTracker.OnTrackerChanged += GetBigFiles;

        var value = await _pool.QueueTask(async () => { return ServiceExecSaveJob.Run(_configuration, lockTracker, bigFileTracker, id, name); });
        return (value.Item1, value.Item2);
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
        try
        {
            var value = _pool.QueueTask(async () => { return ServiceResumeSaveJob.Run(_configuration, id); });
            return (value.Result.Item1, value.Result.Item2);
        }
        catch (Exception e)
        {
            Console.WriteLine("Probably no backup to resume");
            // Console.WriteLine(e);
            // throw;
        }
        
        return (0,"");
    }

    
}