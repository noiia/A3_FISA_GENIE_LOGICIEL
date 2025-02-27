using Job.Config;
using Job.Services.ExecSaveJob;
using Logger;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ExecSaveJob;

public class ResumeBackup : Backup
{
    private static CompleteBackup instance;

    private static string SaveId;

    // private string BackupId;
    public ResumeBackup(SaveJob saveJob) : base(saveJob)
    {
        if (Directory.Exists(saveJob.Source))
        {
            SaveJob = saveJob;
            RootDir = saveJob.Source;
            SavesDir = saveJob.Destination;
            setSaveDir();
        }
        else
        {
            Console.WriteLine($"{saveJob.Source} doesn't exist");
        }
    }

    public static Counters ResumeCounter { get; private set; }

    public static (List<string> filesToSave, List<RealTimeState.BackupFile> filesToResume)? GetFilesForResume(
        Configuration configuration, int backupId)
    {
        var folderName = "EasySave";
        var fileName = "statefile.log";
        var filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), folderName,
            fileName);

        List<RealTimeState.BackupFile> filesToResume = new List<RealTimeState.BackupFile>();
        var filesToSave = new List<string>();


        if (File.Exists(filePath))
            try
            {
                var backupEntries = new List<JObject>();
                var lines = File.ReadAllLines(filePath);

                foreach (var line in lines)
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        var entry = JsonConvert.DeserializeObject<JObject>(line);
                        backupEntries.Add(entry);
                    }

                var filteredEntries = backupEntries
                    .Where(entry => entry["BackupID"].Value<int>() == backupId)
                    .First();

                Console.WriteLine($"resume {filteredEntries["Name"]}");

                var saveJob = configuration.GetSaveJob(filteredEntries["Name"].Value<string>());
                if (saveJob.Type == "full")
                {
                    var completeBackup = new CompleteBackup(saveJob);
                    // completeBackup.Save();
                    filesToSave = completeBackup.GetFiles(saveJob.Source, new List<string>());
                }
                else if (saveJob.Type == "diff")
                {
                    var differentialBackup = new DifferentialBackup(saveJob);
                    // differentialBackup.Save();
                    filesToSave = differentialBackup.GetFiles(saveJob.Source, new List<string>());
                }

                var savedFiles = RealTimeState.GetFilesAdvancementByBackupId(backupId);
                foreach (var savedFile in savedFiles)
                    Console.WriteLine($"{savedFile.Destination} {savedFile.Advancement}");

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
                        filesToResume.Add(savedFile);
                        filesToSave.Remove(savedFile.Source);
                        Console.WriteLine("add " + savedFile.Destination + " to files to resume");
                    }
                    // else if (!filesToSave.Contains(savedFile.Source))
                    // {
                    //     deletedFiles.Add(savedFile.Destination);
                    //     Console.WriteLine("add " + savedFile.Destination + " to deleted files");
                    // }
                }


                Console.WriteLine("files to save : " + filesToSave.Count);
                foreach (var file in filesToSave) Console.WriteLine(file);
                Console.WriteLine("files to resume : " + filesToResume.Count);
                foreach (var file in filesToResume) Console.WriteLine($"{file.Destination} {file.Advancement}");

                // Console.WriteLine("files deleted : " + deletedFiles.Count);
                // foreach (var file in deletedFiles)
                // {
                //     Console.WriteLine(file);
                // }
                // return new List<Logger.RealTimeState.BackupFile>();
                // return new List<Logger.RealTimeState.BackupFile>(filesToResume);
                var lastsave = backupEntries.Last();
                // ResumeCounter = new Counters(lastsave.Value<double>("RemainingData"), lastsave.Value<int>("RemainingFiles"), true, backupId);
                ResumeCounter =
                    new Counters(lastsave.Value<double>("RemainingData") + lastsave.Value<double>("Advancement"),
                        lastsave.Value<int>("RemainingFiles"), true, backupId);

                // ResumeCounter.TransferedFileCount = lastsave.
                var correspondingSaveJob = configuration.GetSaveJob(lastsave.Value<string>("Name"));
                var basePath = correspondingSaveJob.Destination;
                var fullPath = lastsave.Value<string>("Destination");

                var remainingPath = fullPath.Replace(basePath, string.Empty);

                string[] pathParts = remainingPath.Split('\\');

                SaveId = pathParts[1];
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., log the error)
                Console.WriteLine($"Error reading or deserializing the file: {ex.Message}");
                return null;
            }

        return (filesToSave, filesToResume);
    }


    public void Resume(Configuration configuration, int backupId)
    {
        (List<string> filesToSave, List<RealTimeState.BackupFile> filesToResume) =
            GetFilesForResume(configuration, backupId) ??
            (new List<string>(), new List<RealTimeState.BackupFile>());

        var infos = new Infos();
        infos.SaveJobName = SaveJob.Name;
        infos.ID = backupId.ToString();
        infos.Counters = ResumeCounter;
        // Infos.FileInfo = new FileInfo(file);
        infos.SaveDir = SaveDir;
        infos.StateFileName = "statefile.log";
        infos.SaveJobID = SaveJob.Id.ToString();
        infos.lastSave = DateTime.MinValue;

        try
        {
            foreach (var file in filesToResume)
            {
                Console.WriteLine("Resume " + file.Destination);

                var saveIDLength = SaveId.Length;
                // string source = Path.Combine(SaveJob.Source, file.Destination.Replace(SourcePrefix, string.Empty));
                // string relativePath = file.Item1.Replace(SourcePrefix, string.Empty).TrimStart('\\');
                // string Destination = Path.Combine(SaveJob.Destination, SaveId, relativePath);
                // Console.WriteLine($"copy {file} to {Path.Combine(SaveJob.Destination, SaveId, file.Replace(SourcePrefix, string.Empty))}");
                infos.FileInfo = new FileInfo(file.Source);
                // Console.WriteLine($"copy {file} to {SaveJob.Destination}\\{SaveID}\\{}");
                CopyFileWithProgress(configuration, file.Source, file.Destination, infos,
                    Convert.ToInt32(file.Advancement));
                // RealTimeState.WriteState(this.SaveJob.Name, ResumeCounter, new FileInfo(file.Source), file.Destination, infos.StateFileName, "", this.ID);

                RealTimeState.WriteState(infos.SaveJobID, infos.Counters, infos.FileInfo, file.Destination,
                    infos.StateFileName, "", backupId.ToString());
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("No file in filesToResume");
            Console.WriteLine(e);
        }

        try
        {
            var exemplefile = filesToSave[0] ?? filesToResume.First().Destination; // dest ??
            // string SaveID = SaveId;
            var suffix = exemplefile.Replace(SaveJob.Source, string.Empty);
            var SourcePrefix = exemplefile.Replace(suffix, string.Empty);
            SaveId = int.Parse(SaveId).ToString();

            foreach (var file in filesToSave)
            {
                Console.WriteLine("Save " + file);
                var saveIDLength = SaveId.Length;
                // string source = SaveJob.Source + "\\" + file.Replace(SourcePrefix, string.Empty).Substring(4+saveIDLength); 
                var source = Path.Combine(SaveJob.Source, file.Replace(SourcePrefix, string.Empty));
                var relativePath = file.Replace(SourcePrefix, string.Empty).TrimStart('\\');

                var Destination = Path.Combine(SaveJob.Destination, SaveId, relativePath);
                // Console.WriteLine($"copy {file} to {Path.Combine(SaveJob.Destination, SaveId, file.Replace(SourcePrefix, string.Empty))}");
                infos.FileInfo = new FileInfo(file);
                // Console.WriteLine($"copy {file} to {SaveJob.Destination}\\{SaveID}\\{}");
                CopyPasteFile(file, Destination, infos);
                RealTimeState.WriteState(SaveJob.Id.ToString(), ResumeCounter, new FileInfo(file),
                    file.Replace(RootDir, SaveDir), infos.StateFileName, "", backupId.ToString());
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("No save in filesToSave");
            // Console.WriteLine(e);
        }
    }
}