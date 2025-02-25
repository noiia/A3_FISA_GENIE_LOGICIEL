using Job.Config;

namespace Job.Controller;

public class DefineSaveJobsHierarchy(int id, int importance, int status)
{
    public int Id { get; set; } = id;
    public int Importance { get; set; } = importance;
    public int Status { get; set; } = status;
}

public class ImportantSaveJobs
{
    private static int Importance;
    private static List<string> GetFiles(string RootDir, List<string> extensions)
    {
        Importance = 0;
        List<string> files = new List<string>();
        foreach (string file in Directory.GetFiles(RootDir))
        {
            Importance = extensions.Contains(Path.GetExtension(file)) ? Importance += 1 : Importance;
        }

        foreach (string Dir in Directory.GetDirectories(RootDir))
        {
            GetFiles(Dir, files);
        }

        return files;
    }
    
    List<DefineSaveJobsHierarchy> fileHierarchy = new List<DefineSaveJobsHierarchy>();

    public void SetSaveJobHierarchies(int[] ids)
    {
        var  configuration = ConfigSingleton.Instance();
        // int1 = save job id
        // int2 = importance, highest is the most important
        // int3 = status, 0 not used in this session, 1 used, 2 waiting for use 
        SaveJob? saveJob = null;
        foreach (int id in ids)
        {
            saveJob = configuration.GetSaveJob(id);
            var test = configuration.GetFileExtension().ToList();
            GetFiles(saveJob.Source, configuration.GetFileExtension().ToList());
            fileHierarchy.Add(new DefineSaveJobsHierarchy(id, Importance, 0));
        }
    }

    public DefineSaveJobsHierarchy? GetMostImportantSaveJobs(List<int> ids)
    {
        var activeJobs = new List<DefineSaveJobsHierarchy>();
        foreach (var id in ids)
        {
            activeJobs.Add(fileHierarchy.First(x => x.Id == id));
        }
        
        return activeJobs
            .Where(x => x.Status == 0)
            .OrderByDescending(x => x.Importance)
            .FirstOrDefault();
    }

    public void SetStatus(int id, int status)
    {
        fileHierarchy.First(x => x.Id == id).Status = status;
    }
}