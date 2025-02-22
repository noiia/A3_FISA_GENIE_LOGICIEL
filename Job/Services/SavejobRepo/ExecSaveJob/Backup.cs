using System;
using System.Diagnostics;
using System.IO;
using Job.Config;
using Logger;
using Newtonsoft.Json;

namespace ExecSaveJob;

public class Json
{
    public string BackupID { get; set; }
    public string Name { get; set; }
    public string IsActive { get; set; }
    public string DateTime { get; set; }
    public string Source { get; set; }
    public string Destination { get; set; }
    public string RemainingFiles { get; set; }
    public string RemainingData { get; set; }
}

public class Infos
{
    public Infos() { }
    public string ID { get; set; }
    public string SaveJobName { get; set; }
    public string StateFileName { get; set; }

    public string SavesDir { get; set; }

    public FileInfo FileInfo { get; set; }

    public Counters Counters { get; set; }
}

public abstract class Backup
{
    public static List<string> BackupFiles { get; set; }
    
    public string ID { get; set; }
    public string RootDir { get; set; }
    public string SavesDir { get; protected set; }
    
    public SaveJob SaveJob { get; set; }

    public void setSavesDir(string savesDir)
    {
        this.SavesDir = savesDir;
        this.setSaveDir();
        this.Refractor();
    }
    public string SaveDir { get; protected set; }
    
    protected void Refractor()
    {
        if (RootDir[^1] != '\\') RootDir += '\\';
        if (SavesDir[^1] != '\\') SavesDir += '\\';
        if (SaveDir[^1] != '\\') SaveDir += '\\';
    }
    
    protected void setSaveDir()
    {
        this.SaveDir = $"{this.SavesDir}\\{this.getLastBackupNumber(this.SavesDir) + 1}";
        
        
        // DirectoryInfo directory = new DirectoryInfo(savesDir);
        // System.IO.Directory.CreateDirectory($"{savesDir}\\{this.getLastBackupNumber(savesDir) + 1}");
    }
    
    protected Backup BackupInstance { get; set; }

    public static void GetInstance(SaveJob saveJob)
    {
        throw new NotImplementedException();
    }

    protected Backup(SaveJob saveJob)
    {
        if (Directory.Exists(saveJob.Source))
        {
            this.SetBackupID();
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
    
    protected int getLastBackupNumber(string savesDir)
    {
        if (!Directory.Exists(savesDir))
        {
            return 0;
        }
        DirectoryInfo directory = new DirectoryInfo(savesDir);
        int lastBackupNumber = 0;

        foreach (DirectoryInfo dir in directory.GetDirectories())
        {
            if (int.TryParse(dir.Name, out int num))
            {
                if (num > lastBackupNumber)
                {
                    lastBackupNumber = num;
                }
            }
        }
        return lastBackupNumber;
    }


    
    protected void CopyPasteFile(string RootFile, string ToFile, Infos infos, int Err = 0 )
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
            string extension = Path.GetExtension(RootFile);
            Configuration configuration = new Configuration( Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\EasySave\\" + "config.json");
            string[] extensionCrypt = configuration.GetCryptExtension();
            if (extensionCrypt.Contains(extension))
            {
                string cryptKey = configuration.GetCryptKey();
                string[] args = ["Crypt", RootFile, ToFile, cryptKey];
                ProcessStartInfo cryptoSoftStartInfo = new ProcessStartInfo
                {
                    FileName = "CryptoSoft.exe",
                    Arguments = string.Join(' ', args),
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                };
                Process processCryptoSoft = new Process { StartInfo = cryptoSoftStartInfo };
                processCryptoSoft.Start();
                string output = processCryptoSoft.StandardOutput.ReadToEnd();
                string error = processCryptoSoft.StandardError.ReadToEnd();
                processCryptoSoft.WaitForExit();
            }
            else
            {
                // File.Copy(RootFile, ToFile, true);
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
                    string to = (Directory.GetParent(ToFile).FullName+"\\");
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
                default:
                    break;
            }
        }
    }    
    
    
    //from internet #TODO see if static is necessary and causes no problem to multi thread
    static void CopyFileWithProgress(string sourceFilePath, string destinationFilePath, Infos infos)
    {
        const int bufferSize = 2 * 1048576; // 2 MB buffer size, you can adjust it as per your requirement
        // const int bufferSize = 1024;
        
        using (var sourceStream = new FileStream(sourceFilePath, FileMode.Open, FileAccess.Read))
        using (var destinationStream = new FileStream(destinationFilePath, FileMode.Create, FileAccess.Write))
        {
            byte[] buffer = new byte[bufferSize];
            int bytesRead;
            long totalBytesCopied = 0;
            long fileSize = sourceStream.Length;
 
            while ((bytesRead = sourceStream.Read(buffer, 0, bufferSize)) > 0)
            {
                destinationStream.Write(buffer, 0, bytesRead);
                totalBytesCopied += bytesRead;
 
                // Calculate progress
                double progress = (double)totalBytesCopied / fileSize * 100;
                Console.WriteLine($"Progress: {progress:F2}%");
                RealTimeState.WriteState(infos.SaveJobName, infos.Counters, infos.FileInfo, infos.SavesDir, infos.StateFileName, "", infos.ID, totalBytesCopied);                
            }
        }
    }
    
    
    protected virtual List<string> GetFiles(string rootDir, List<string> files)
    {
        string stateFileName = "statefile.log";
        
        // DirectoryInfo directoryInfo = new DirectoryInfo(rootDir);
        // Counters counters = new Counters(directoryInfo.GetFiles().Length, directoryInfo.GetFiles().Count(), true);
        //
        // RealTimeState.AddCounter(counters);
        foreach (string file in Directory.GetFiles(rootDir))
        {
            FileInfo fileInfo = new FileInfo(file);
            
            // RealTimeState.WriteState(this.SaveJob.Name, counters, fileInfo, SavesDir, stateFileName, "");
            files.Add(file);
        }

        foreach (string dir in Directory.GetDirectories(rootDir))
        {
            GetFiles(dir, files);
        }

        // counters.IsActive = false;
        return files;
    }
    
    protected void turnArchiveBitFalse(string filePath)
    {
        // Récupérer les attributs actuels du fichier
        FileAttributes attributes = File.GetAttributes(filePath);

        // Désactiver le bit d'archive
        attributes &= ~FileAttributes.Archive;

        // Définir les nouveaux attributs du fichier
        File.SetAttributes(filePath, attributes);
    }

    
    
    protected void CopyDir()
    {
        string stateFileName = "statefile.log";
        List<string> files = new List<string>();
        files = GetFiles(RootDir, files);
        double DataCount = 0;
        
        foreach (string file in files)
        {
            DataCount += new FileInfo(file).Length;
        }
        Counters counters = new Counters(DataCount, files.Count, true);
        
        Infos infos = new Infos();
        infos.SaveJobName = this.SaveJob.Name;
        infos.Counters = counters;
        // Infos.FileInfo = new FileInfo(file);
        infos.SavesDir = SavesDir;
        infos.StateFileName = stateFileName;
        infos.ID = this.ID;
        
        
        foreach (string file in files)
        {
            infos.FileInfo = new FileInfo(file);
            RealTimeState.AddCounter(counters);
            //#TODO avancement
            // while (!Backup.Finished)
            // {
            // advancement = CopyPasteFile(file, file.Replace(RootDir, SaveDir)); => wait 10s or finish => return progress or end 
            // if advancement = end => Finished = true
            // RealTimeState.WriteState(this.SaveJob.Name, counters, new FileInfo(file), SavesDir, stateFileName, "", this.ID, advancement); 
            // }
            
            CopyPasteFile(file, file.Replace(RootDir, SaveDir), infos);
            RealTimeState.WriteState(this.SaveJob.Name, counters, new FileInfo(file), SavesDir, stateFileName, "", this.ID);
            turnArchiveBitFalse(file);
        } 
    }

    public void Save()
    {
        CopyDir();
    }


    public void SetBackupID()
    {
        string folderName = "EasySave";
        string fileName = "statefile.log";
        string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), folderName, fileName);
        int LastID = 0;
        if (File.Exists(filePath))
        {
            string[] lines = File.ReadAllLines(filePath);
            foreach (string content in lines)
            {
                string output = content;
                Json? deserializedProduct = JsonConvert.DeserializeObject<Json>(output);
                if (deserializedProduct != null)
                {
                    LastID = Math.Max(LastID, Convert.ToInt32(deserializedProduct.BackupID));
                }
            }
        }
        this.ID = (LastID + 1).ToString();
    }
}