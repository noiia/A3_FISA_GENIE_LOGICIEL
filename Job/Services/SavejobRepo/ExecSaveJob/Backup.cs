using System.Diagnostics;
using Job.Config;
using Job.Controller;
using Logger;
using Newtonsoft.Json;

namespace Job.Services.ExecSaveJob;

public class Json
{
    public string BackupID { get; set; }

    // public string Name { get; set; }
    public string SaveJobID { get; set; }
    public string IsActive { get; set; }
    public string DateTime { get; set; }
    public string Source { get; set; }
    public string Destination { get; set; }
    public string RemainingFiles { get; set; }
    public string RemainingData { get; set; }
}

public class Infos
{
    public string ID { get; set; }
    public string SaveJobName { get; set; }
    public string SaveJobID { get; set; }
    public string StateFileName { get; set; }

    public string SaveDir { get; set; }

    public FileInfo FileInfo { get; set; }

    public Counters Counters { get; set; }

    public DateTime lastSave { get; set; } = DateTime.MinValue;
}

public abstract class Backup
{
    public TimeSpan cryptDuration;

    protected Backup(SaveJob saveJob)
    {
        if (Directory.Exists(saveJob.Source))
        {
            cryptDuration = new TimeSpan(0);
            SetBackupID();
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

    public static List<string> BackupFiles { get; set; }
    public static int MillisecondForWriteProgressInConfig { get; set; } = 10000;

    public string ID { get; set; }
    public string RootDir { get; set; }
    public string SavesDir { get; protected set; }

    public SaveJob SaveJob { get; set; }
    public string SaveDir { get; protected set; }

    protected Backup BackupInstance { get; set; }

    public void setSavesDir(string savesDir)
    {
        SavesDir = savesDir;
        setSaveDir();
        Refractor();
    }

    protected void Refractor()
    {
        if (RootDir[^1] != '\\') RootDir += '\\';
        if (SavesDir[^1] != '\\') SavesDir += '\\';
        if (SaveDir[^1] != '\\') SaveDir += '\\';
    }

    protected void setSaveDir()
    {
        SaveDir = $"{SavesDir}\\{getLastBackupNumber(SavesDir) + 1}";


        // DirectoryInfo directory = new DirectoryInfo(savesDir);
        // System.IO.Directory.CreateDirectory($"{savesDir}\\{this.getLastBackupNumber(savesDir) + 1}");
    }

    public static void GetInstance(SaveJob saveJob)
    {
        throw new NotImplementedException();
    }

    protected int getLastBackupNumber(string savesDir)
    {
        if (!Directory.Exists(savesDir)) return 0;
        var directory = new DirectoryInfo(savesDir);
        var lastBackupNumber = 0;

        foreach (var dir in directory.GetDirectories())
            if (int.TryParse(dir.Name, out var num))
                if (num > lastBackupNumber)
                    lastBackupNumber = num;

        return lastBackupNumber;
    }


    protected void CopyPasteFile(string RootFile, string ToFile, Infos infos, int Err = 0)
    {
        void HandleErrorLoop(int err, int Err1)
        {
            if (err == Err1)
            {
                Console.WriteLine($"Error {Err} happened twice in a row: exit the program");
                Environment.Exit(0);
            }
        }

        try
        {
            var extension = Path.GetExtension(RootFile);
            var configuration = ConfigSingleton.Instance();
            string[] extensionCrypt = configuration.GetCryptExtension();
            if (extensionCrypt.Contains(extension))
            {
                // Control timer
                var cryptKey = configuration.GetCryptKey();
                string[] args = ["Crypt", RootFile, ToFile, cryptKey];
                var cryptoSoftStartInfo = new ProcessStartInfo
                {
                    FileName = "CryptoSoft.exe",
                    Arguments = string.Join(' ', args),
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                };
                var processCryptoSoft = new Process { StartInfo = cryptoSoftStartInfo };
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                processCryptoSoft.Start();
                var output = processCryptoSoft.StandardOutput.ReadToEnd();
                var error = processCryptoSoft.StandardError.ReadToEnd();
                processCryptoSoft.WaitForExit();
                stopwatch.Stop();
                var ts = stopwatch.Elapsed;
                cryptDuration = cryptDuration.Add(ts);
            }
            else
            {
                // arbitrary transfer speed
                var BitsPerSec = 1;

                // File.Copy(RootFile, ToFile, true);
                if (infos.FileInfo.Length < configuration.GetLengthLimit())
                {
                    Console.WriteLine("not limited");
                    CopyFileWithProgress(RootFile, ToFile, infos);
                }
                else
                {
                    Console.WriteLine("limited");
                    // CopyFileWithProgress(RootFile, ToFile, infos, BitsPerSec );
                }

                CopyFileWithProgress(RootFile, ToFile, infos);
            }
        }
        catch (Exception E)
        {
            // Console.WriteLine(E.Message);
            // Console.WriteLine(E.GetType());

            switch (E.GetType().Name)
            {
                case nameof(UnauthorizedAccessException):
                    // Console.WriteLine($"Access denied: From {RootFile} To {ToFile}");
                    break;
                case nameof(DirectoryNotFoundException):
                    // Console.WriteLine($"Directory not found: From {RootFile} To {ToFile}");
                    HandleErrorLoop(Err, 2);
                    var to = Directory.GetParent(ToFile).FullName + "\\";
                    if (!Directory.Exists(to))
                    {
                        Directory.CreateDirectory(to);
                        // Directory.CreateDirectory(Path.GetFullPath(to));
                        CopyPasteFile(RootFile, ToFile, infos, 2);
                    }

                    break;
                case nameof(FileNotFoundException):
                    // Console.WriteLine($"File not found: From {RootFile} To {ToFile}");
                    break;
                case nameof(PathTooLongException):
                    // Console.WriteLine($"Path too long: From {RootFile} To {ToFile}");
                    break;
                case nameof(IOException):
                    // Console.WriteLine($"Already exists: From {RootFile} To {ToFile}");
                    // if (!AreFilesEqual(RootFile, ToFile))
                    // {
                    //     Console.WriteLine("Files are different");
                    //     File.Delete(ToFile);
                    //     CopyPasteFiles(RootFile, ToFile, 5);
                    // }
                    // else
                    // {
                    //     Console.WriteLine("Files are the same");
                    // }
                    break;
            }
        }
    }


    //from internet #TODO see if static is necessary and causes no problem to multi thread
    private static void CopyFileWithProgress(string sourceFilePath, string destinationFilePath, Infos infos)
    {
        const int bufferSize = 2 * 1048576; // 2 MB buffer size, you can adjust it as per your requirement
        // const int bufferSize = 1024;

        using (var sourceStream = new FileStream(sourceFilePath, FileMode.Open, FileAccess.Read))
        using (var destinationStream = new FileStream(destinationFilePath, FileMode.Create, FileAccess.Write))
        {
            var buffer = new byte[bufferSize];
            int bytesRead;
            long totalBytesCopied = 0;
            var fileSize = sourceStream.Length;

            var LastUpload = infos.lastSave;
            var configuration = ConfigSingleton.Instance();
            var sj = configuration.GetSaveJob(int.Parse(infos.SaveJobID));


            while ((bytesRead = sourceStream.Read(buffer, 0, bufferSize)) > 0)
            {
                destinationStream.Write(buffer, 0, bytesRead);
                destinationStream.Flush();
                totalBytesCopied += bytesRead;

                // Calculate progress
                // double progress = (double)totalBytesCopied / fileSize * 100;
                RealTimeState.WriteState(infos.SaveJobName, infos.Counters, infos.FileInfo, destinationFilePath,
                    infos.StateFileName, "", infos.ID, totalBytesCopied);

                if (DateTime.Now - LastUpload > TimeSpan.FromMicroseconds(MillisecondForWriteProgressInConfig))
                {
                    var counter = infos.Counters;

                    var progress = (int)((counter.TransferedData + totalBytesCopied) / counter.DataCount * 100);

                    Console.WriteLine($"TransferedData: {counter.TransferedData}%");
                    Console.WriteLine($"totalBytesCopied: {totalBytesCopied}%");
                    Console.WriteLine($"counter.DataCount: {counter.DataCount}%");

                    // Console.WriteLine($"Progress: {progress:F2}%");
                    UpdateConfigProgress(configuration, sj, progress);
                    infos.lastSave = DateTime.Now;
                }
            }

            var counter2 = infos.Counters;

            var progress2 = (int)((counter2.TransferedData + totalBytesCopied) / counter2.DataCount * 100);

            Console.WriteLine($"TransferedData: {counter2.TransferedData}%");
            Console.WriteLine($"totalBytesCopied: {totalBytesCopied}%");
            Console.WriteLine($"counter.DataCount: {counter2.DataCount}%");

            // Console.WriteLine($"Progress: {progress:F2}%");
            UpdateConfigProgress(configuration, sj, progress2);
            infos.lastSave = DateTime.Now;
        }
    }


    public static void CopyFileWithProgress(Configuration configuration, string sourceFilePath,
        string destinationFilePath, Infos infos, long offset)
    {
        const int bufferSize = 2 * 1048576;

        // const int bufferSize = 1024;

        using (var sourceStream = new FileStream(sourceFilePath, FileMode.Open, FileAccess.Read))
        using (var destinationStream = new FileStream(destinationFilePath, FileMode.Append, FileAccess.Write))
        {
            var buffer = new byte[bufferSize];
            int bytesRead;
            var totalBytesCopied = offset;
            var fileSize = sourceStream.Length;

            // source at offset
            sourceStream.Seek(offset, SeekOrigin.Begin);

            // destination at end of file
            destinationStream.Seek(0, SeekOrigin.End);

            var LastUpload = infos.lastSave;

            var sj = configuration.GetSaveJob(int.Parse(infos.SaveJobID));

            while ((bytesRead = sourceStream.Read(buffer, 0, bufferSize)) > 0)
            {
                destinationStream.Write(buffer, 0, bytesRead);
                destinationStream.Flush();
                totalBytesCopied += bytesRead;

                // Calculer la progression

                RealTimeState.WriteState(infos.SaveJobName, infos.Counters, infos.FileInfo, destinationFilePath,
                    infos.StateFileName, "", infos.ID, totalBytesCopied);

                if (DateTime.Now - LastUpload > TimeSpan.FromSeconds(MillisecondForWriteProgressInConfig))
                {
                    var counter = infos.Counters;

                    var progress = (int)(counter.TransferedData + totalBytesCopied / counter.DataCount * 100);
                    // Console.WriteLine($"Progress: {progress:F2}%");
                    // sj.Progress = (int)progress;
                    UpdateConfigProgress(configuration, sj, progress);
                    // configuration.LoadConfiguration();
                    LastUpload = DateTime.Now;
                }
            }
        }
    }


    public static void UpdateConfigProgress(Configuration configuration, SaveJob sj, int progress)
    {
        try
        {
            List<SaveJob> saveJobs = configuration.GetSaveJobs().ToList();

            // SaveJob sj = saveJobs.FirstOrDefault(i => i.Name == saveJobName);
            // if (sj != null)
            // {
            //     saveJobs.Remove(sj); 
            //     sj.Progress = progress;
            // }
            saveJobs.Remove(sj);
            sj.Progress = progress;
            saveJobs.Add(sj);

            configuration.SetSaveJobs(saveJobs.ToArray());
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }


    public virtual List<string> GetFiles(string rootDir, List<string> files)
    {
        var stateFileName = "statefile.log";

        // DirectoryInfo directoryInfo = new DirectoryInfo(rootDir);
        // Counters counters = new Counters(directoryInfo.GetFiles().Length, directoryInfo.GetFiles().Count(), true);
        //
        // RealTimeState.AddCounter(counters);
        foreach (var file in Directory.GetFiles(rootDir))
        {
            var fileInfo = new FileInfo(file);

            // RealTimeState.WriteState(this.SaveJob.Name, counters, fileInfo, SavesDir, stateFileName, "");
            files.Add(file);
        }

        foreach (var dir in Directory.GetDirectories(rootDir)) GetFiles(dir, files);

        // counters.IsActive = false;
        return files;
    }

    protected void TurnArchiveBitFalse(string filePath)
    {
        // Récupérer les attributs actuels du fichier
        var attributes = File.GetAttributes(filePath);

        // Désactiver le bit d'archive
        attributes &= ~FileAttributes.Archive;

        // Définir les nouveaux attributs du fichier
        File.SetAttributes(filePath, attributes);
    }

    public (bool, string) AnyBusinessAppRunning()
    {
        var configuration = ConfigSingleton.Instance();
        foreach (var process in configuration.GetBuisnessApp())
            if (Process.GetProcessesByName(process).Any())
                return (true, process);

        return (false, string.Empty);
    }

    protected void CopyDir(int id, LockTracker lockTracker, BigFileTracker bigFileTracker)
    {
        var _configuration = ConfigSingleton.Instance();
        var stateFileName = "statefile.log";
        List<string> files = new List<string>();
        files = GetFiles(RootDir, files);
        double DataCount = 0;

        foreach (var file in files) DataCount += new FileInfo(file).Length;
        var counters = new Counters(DataCount, files.Count, true);

        var infos = new Infos();
        // infos.SaveJobName = this.SaveJob.Name;
        infos.SaveJobID = SaveJob.Id.ToString();
        infos.Counters = counters;
        // Infos.FileInfo = new FileInfo(file);
        infos.SaveDir = SaveDir;
        infos.StateFileName = stateFileName;
        infos.ID = ID;

        var tooBigFile = new List<string>();
        var bigFiles = new Dictionary<int, List<string>>();
        foreach (var file in files)
        {
            lockTracker.AddOrUpdateLockStatus(Convert.ToInt32(id), "", 0);
            var (isProcessRunning, processName) = AnyBusinessAppRunning();
            if (!isProcessRunning)
            {
                if (file.Length < _configuration.GetLengthLimit())
                {
                    infos.FileInfo = new FileInfo(file);
                    RealTimeState.AddCounter(counters);
                    CopyPasteFile(file, file.Replace(RootDir, SaveDir), infos);
                    RealTimeState.WriteState(SaveJob.Name, counters, new FileInfo(file), file.Replace(RootDir, SaveDir),
                        stateFileName, "", ID);
                    TurnArchiveBitFalse(file);
                }
                else
                {
                    tooBigFile.Add(file);
                    bigFileTracker.AddOrUpdateBigFile(id, tooBigFile);
                }

                bigFiles = SaveJobRepo.SchedulingBigFileTransfert();
                if (bigFiles.TryGetValue(id, out var bigFilesToTransfer) && bigFilesToTransfer.Count > 0)
                {
                    foreach (var bigFile in bigFilesToTransfer)
                    {
                        infos.FileInfo = new FileInfo(bigFile);
                        RealTimeState.AddCounter(counters);
                        CopyPasteFile(bigFile, bigFile.Replace(RootDir, SaveDir), infos);
                        RealTimeState.WriteState(SaveJob.Id.ToString(), counters, new FileInfo(bigFile),
                            bigFile.Replace(RootDir, SaveDir), stateFileName, "", ID);
                        TurnArchiveBitFalse(bigFile);
                    }

                    SaveJobRepo.RemoveFileTransfered(id);
                }
            }
            else
            {
                while (isProcessRunning)
                {
                    lockTracker.AddOrUpdateLockStatus(Convert.ToInt32(id), processName, 4);
                    Thread.Sleep(500);
                    (isProcessRunning, processName) = AnyBusinessAppRunning();
                }
            }
        }

        // RealTimeState.WriteMessage($"Job {this.SaveJob.Name}, with ID {this.ID} has been saved");
    }

    public void Save(int id, LockTracker lockTracker, BigFileTracker bigFileTracker)
    {
        CopyDir(id, lockTracker, bigFileTracker);
        var config = ConfigSingleton.Instance();
        var elapsedTime = string.Format("{0:00}:{1:00}:{2:00}.{3:00}", cryptDuration.Hours, cryptDuration.Minutes,
            cryptDuration.Seconds, cryptDuration.Milliseconds / 10);
        LoggerUtility.WriteLog(config.GetLogType(), LoggerUtility.Warning, $"Crypt duration: {elapsedTime}");
    }


    public void SetBackupID()
    {
        var folderName = "EasySave";
        var fileName = "statefile.log";
        var filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), folderName,
            fileName);
        var LastID = 0;
        if (File.Exists(filePath))
        {
            string[] lines = File.ReadAllLines(filePath);
            foreach (var content in lines)
            {
                var output = content;
                var deserializedProduct = JsonConvert.DeserializeObject<Json>(output);
                if (deserializedProduct != null)
                    LastID = Math.Max(LastID, Convert.ToInt32(deserializedProduct.BackupID));
            }
        }

        ID = (LastID + 1).ToString();
    }
}