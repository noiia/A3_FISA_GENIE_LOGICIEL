using Config;
using DynamicData.Kernel;
using Job.Config;
using Logger;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ExecSaveJob;

public class ResumeBackup : Backup
{
    private static CompleteBackup instance;

    // private string BackupId;
    public ResumeBackup(SaveJob saveJob) : base(saveJob)
    {

        if (Directory.Exists(saveJob.Source))
        {
            this.SaveJob = saveJob;
            this.RootDir = saveJob.Source;
            this.SavesDir = saveJob.Destination;
            this.setSaveDir();
        }
        else
        {
            Console.WriteLine($"{saveJob.Source} doesn't exist");
        }

    
    }
    
    private static string SaveId;
    public static Counters ResumeCounter { get; private set; }
    
    public static (List<string> filesToSave, List<(string, int)> filesToResume)? GetFilesForResume(Configuration configuration ,int backupId)
    {
        string folderName = "EasySave";
        string fileName = "statefile.log";
        string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), folderName, fileName);
    
        List<(string, int)> filesToResume = new List<(string, int)>();
        List<string> filesToSave = new List<string>();

        
        if (File.Exists(filePath))
        {
            try
            {
                var backupEntries = new List<JObject>();
                string[] lines = File.ReadAllLines(filePath);
    
                foreach (var line in lines)
                {
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        var entry = JsonConvert.DeserializeObject<JObject>(line);
                        backupEntries.Add(entry);
                    }
                }
    
                var filteredEntries = backupEntries
                    .Where(entry => entry["BackupID"].Value<int>() == backupId)
                    .First();
                
                Console.WriteLine($"resume {filteredEntries["Name"]}");
                
                SaveJob saveJob = configuration.GetSaveJob(filteredEntries["Name"].Value<string>());
                if (saveJob.Type == "full")
                {
                    CompleteBackup completeBackup = new CompleteBackup(saveJob);
                    // completeBackup.Save();
                    filesToSave = completeBackup.GetFiles(saveJob.Source, new List<string>());
                }
                else if (saveJob.Type == "diff")
                {
                    DifferentialBackup differentialBackup = new DifferentialBackup(saveJob);
                    // differentialBackup.Save();
                    filesToSave = differentialBackup.GetFiles(saveJob.Source, new List<string>());
                }
                
                List<Logger.RealTimeState.BackupFile> savedFiles = Logger.RealTimeState.GetFilesAdvancementByBackupId(backupId);
                foreach (Logger.RealTimeState.BackupFile savedFile in savedFiles)
                {
                    Console.WriteLine($"{savedFile.Destination} {savedFile.Advancement}");
                }
                // List<string> deletedFiles = new List<string>();

                // foreach (string file in filesToSave)
                // {
                //     if (!savedFiles.Exists(savedFile => savedFile.Source == file))
                //     {
                //         filesToResume.Add((file, 0));
                //         Console.WriteLine("add " + file + " to files to resume");
                //     }
                // }

                foreach (var savedFile in savedFiles.ToList())
                {
                    Console.WriteLine($"{savedFile.Destination} {savedFile.Advancement}");

                    if (savedFile.Advancement.EndsWith("100%"))
                    {
                        filesToSave.Remove(savedFile.Source);
                        Console.WriteLine("remove " + savedFile.Destination + " from files to save");
                    }
                    else if (savedFile.Advancement != "")
                    {
                        filesToResume.Add((savedFile.Destination, int.Parse(savedFile.Advancement)));
                        Console.WriteLine("add " + savedFile.Destination + " to files to resume");
                    }
                    // else if (!filesToSave.Contains(savedFile.Source))
                    // {
                    //     deletedFiles.Add(savedFile.Destination);
                    //     Console.WriteLine("add " + savedFile.Destination + " to deleted files");
                    // }
                }

                
                Console.WriteLine("files to save : " + filesToSave.Count);
                foreach (string file in filesToSave)
                {
                    Console.WriteLine(file);
                }
                Console.WriteLine("files to resume : " + filesToResume.Count);
                foreach (var file in filesToResume)
                {
                    Console.WriteLine(file.Item1 + " " + file.Item2);
                }
                // Console.WriteLine("files deleted : " + deletedFiles.Count);
                // foreach (var file in deletedFiles)
                // {
                //     Console.WriteLine(file);
                // }
                // return new List<Logger.RealTimeState.BackupFile>();
                // return new List<Logger.RealTimeState.BackupFile>(filesToResume);
                
                JObject lastsave = backupEntries.Last();
                ResumeCounter = new Counters(lastsave.Value<double>("DataCount"), lastsave.Value<int>("FileCount"), true, backupId);
                
                Job.Config.SaveJob correspondingSaveJob = configuration.GetSaveJob(lastsave.Value<string>("Name"));
                string basePath = correspondingSaveJob.Destination;
                string fullPath = lastsave.Value<string>("Destination");
                
                string remainingPath = fullPath.Replace(basePath, string.Empty);

                string[] pathParts = remainingPath.Split('\\');

                SaveId = pathParts[1];
                
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., log the error)
                Console.WriteLine($"Error reading or deserializing the file: {ex.Message}");
                return null;
            }
        }
        
        return (filesToSave, filesToResume);
    }
    
    
    
    
    
    
    
    public void Resume(Configuration configuration, int backupId)
    {
        (List<string> filesToSave, List<(string, int)> filesToResume) = GetFilesForResume(configuration, backupId) ?? (new List<string>(), new List<(string, int)>());
        
        Infos infos = new Infos();
        infos.SaveJobName = this.SaveJob.Name;
        infos.Counters = ResumeCounter;
        // Infos.FileInfo = new FileInfo(file);
        infos.SaveDir = SaveDir;
        infos.StateFileName = "statefile.log";
        infos.ID = backupId.ToString();
        
        string exemplefile = filesToSave[0] ?? filesToResume.First().Item1 ;
        // string SaveID = SaveId;
        string suffix = exemplefile.Replace(SaveJob.Source, string.Empty);
        string SourcePrefix = exemplefile.Replace(suffix, string.Empty);


        foreach (string file in filesToSave)
        {
            if (filesToResume.Select(x => x.Item1).Contains(file))
            {
                int saveIDLength = SaveId.Length;
                // string source = SaveJob.Source + "\\" + file.Replace(SourcePrefix, string.Empty).Substring(4+saveIDLength); 
                string source = Path.Combine(SaveJob.Source, file.Replace(SourcePrefix, string.Empty));
                string relativePath = file.Replace(SourcePrefix, string.Empty).TrimStart('\\');
                string Destination = Path.Combine(SaveJob.Destination, SaveId, relativePath);
                // Console.WriteLine($"copy {file} to {Path.Combine(SaveJob.Destination, SaveId, file.Replace(SourcePrefix, string.Empty))}");
                infos.FileInfo = new FileInfo(file);
                // Console.WriteLine($"copy {file} to {SaveJob.Destination}\\{SaveID}\\{}");
                CopyFileWithProgress(file, Destination, infos, filesToResume.Find(x => x.Item1 == file).Item2);
                RealTimeState.WriteState(this.SaveJob.Name, ResumeCounter, new FileInfo(file), file.Replace(RootDir, SaveDir), infos.StateFileName, "", this.ID);
            }
            else
            {
                int saveIDLength = SaveId.Length;
                // string source = SaveJob.Source + "\\" + file.Replace(SourcePrefix, string.Empty).Substring(4+saveIDLength); 
                string source = Path.Combine(SaveJob.Source, file.Replace(SourcePrefix, string.Empty));
                string relativePath = file.Replace(SourcePrefix, string.Empty).TrimStart('\\');
                string Destination = Path.Combine(SaveJob.Destination, SaveId, relativePath);
                // Console.WriteLine($"copy {file} to {Path.Combine(SaveJob.Destination, SaveId, file.Replace(SourcePrefix, string.Empty))}");
                infos.FileInfo = new FileInfo(file);
                // Console.WriteLine($"copy {file} to {SaveJob.Destination}\\{SaveID}\\{}");
                CopyPasteFile(file, Destination, infos);
                RealTimeState.WriteState(this.SaveJob.Name, ResumeCounter, new FileInfo(file), file.Replace(RootDir, SaveDir), infos.StateFileName, "", this.ID);
            }
            

        }

    }



}