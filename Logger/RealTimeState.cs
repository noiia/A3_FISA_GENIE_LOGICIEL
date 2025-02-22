using System.Collections.Generic;
using System.IO;
using System;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Logger
{
    public class Counters
    {
        private static int _nextId = 1;
        public int Id { get; private set; }
        public int FileCount { get; set; }
        public int TransferedFileCount { get; set; }
        public double DataCount { get; set; }
        public double TransferedData { get; set; }
        public bool IsActive { get; set; }

        public Counters(double dataCount, int fileCount, bool isActive)
        {
            Id = _nextId++;
            DataCount = dataCount;
            FileCount = fileCount;
            IsActive = isActive;
        }
    }


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
        public string Advancement { get; set; }
    }
    
    
    public class RealTimeState
    {
        private static string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EasySave", "statefile.log");
        private static List<Counters> counters = new List<Counters>();
        public static void AddCounter(Counters counter)
        {
            counters.Add(counter);
        }

        public static Counters GetCounterById(int i)
        {
            foreach (var counter in counters)
            {
                if (counter.Id == i)
                {
                    return counter;
                }
            }
            return null;
        }
        private static void CreateFile(string filePath)
        {
            using (File.Create(filePath))
            {
                LoggerUtility.WriteLog(LoggerUtility.Info, $"Real time state file created at : {filePath}");
            }
        }

        // fileInfo[0] = fileInfo.Name
        // fileInfo[1] = fileInfo.FullName
        // fileInfo[2] = fileInfo.Lenght


        public static void WriteMessage(string message)
        {
            try
            {
                using (var writer = File.AppendText(filePath))
                {
                    writer.WriteLine(message);
                }
            }
            catch (Exception exception)
            {
                LoggerUtility.WriteLog(LoggerUtility.Error, exception.Message);
                switch (exception.GetType().Name)
                {
                    case nameof(UnauthorizedAccessException):
                        Console.WriteLine($"Access denied: {filePath}");
                        break;
                    case nameof(FileNotFoundException):
                        Console.WriteLine(exception.Message);
                        if (!File.Exists(filePath))
                        {
                            CreateFile(filePath);
                        }
                        break;
                }
            }
        }
        
        public static void WriteState(string saveJobName, Counters counter, FileInfo fileInfo, string destinationPathDir, string fileName, string msg, string id)
        {
            const string folderName = "EasySave";

            bool header = false;

            var filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), folderName, fileName);
            var destinationPath = Path.Combine(destinationPathDir, fileInfo.Name);
            counter.TransferedFileCount++;
            counter.TransferedData += fileInfo.Length;

            double remainingFile = counter.FileCount - counter.TransferedFileCount;
            double remainingData = counter.DataCount - counter.TransferedData;


            // if (!header)
            // {
            //     using (var writer = File.AppendText(filePath))
            //     {
            //         string isActive = counter.IsActive ? "Active" : "Not Active";
            //         string headerMessage = $"{SaveJobName} : {isActive}";
            //         writer.WriteLine(headerMessage);
            //     }
            // }
            
            // string message = $"{DateTime.Now.ToString("yyyy'-'MM'-'dd' 'HH':'mm':'ss")} : Source : {fileInfo.FullName}, Destination:{destinationPath} remaining files :{remainingFile}, remaining data : {remainingData}";

            string message;
            
            Json json = new Json();

            json.BackupID = id;
            json.Name = saveJobName;
            json.IsActive = counter.IsActive ? "Active" : "Not Active";
            json.DateTime = DateTime.Now.ToString("yyyy'-'MM'-'dd' 'HH':'mm':'ss");
            json.Source = fileInfo.FullName;
            json.Destination = destinationPath;
            json.RemainingFiles = remainingFile.ToString();
            json.RemainingData = remainingData.ToString();
            json.Advancement = $"{fileInfo.Length} 100%";

            message = JsonSerializer.Serialize(json);

            // Json deserializedProduct = JsonConvert.DeserializeObject<Json>(output);
            
            try
            {
                using (var writer = File.AppendText(filePath))
                {
                    writer.WriteLine(message);
                }
            }
            catch (Exception exception)
            {
                LoggerUtility.WriteLog(LoggerUtility.Error, exception.Message);
                switch (exception.GetType().Name)
                {
                    case nameof(UnauthorizedAccessException):
                        Console.WriteLine($"Access denied: {filePath}");
                        break;
                    case nameof(FileNotFoundException):
                        Console.WriteLine(exception.Message);
                        if (!File.Exists(filePath))
                        {
                            CreateFile(filePath);
                        }
                        break;
                }
            }
        }
        
        
        
        public static void WriteState(string saveJobName, Counters counter, FileInfo fileInfo, string destinationPathDir, string fileName, string msg, string id, double advancement)
        {
            const string folderName = "EasySave";

            bool header = false;

            var filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), folderName, fileName);
            var destinationPath = Path.Combine(destinationPathDir, fileInfo.Name);
            // counter.TransferedFileCount++;
            // counter.TransferedData += fileInfo.Length;

            double remainingFile = counter.FileCount - counter.TransferedFileCount;
            double remainingData = counter.DataCount - counter.TransferedData - advancement;


            // if (!header)
            // {
            //     using (var writer = File.AppendText(filePath))
            //     {
            //         string isActive = counter.IsActive ? "Active" : "Not Active";
            //         string headerMessage = $"{SaveJobName} : {isActive}";
            //         writer.WriteLine(headerMessage);
            //     }
            // }
            
            string message = $"{DateTime.Now.ToString("yyyy'-'MM'-'dd' 'HH':'mm':'ss")} : Source : {fileInfo.FullName}, Destination:{destinationPath} remaining files :{remainingFile}, remaining data : {remainingData}";

            Json json = new Json();

            json.BackupID = id;
            json.Name = saveJobName;
            json.IsActive = counter.IsActive ? "Active" : "Not Active";
            json.DateTime = DateTime.Now.ToString("yyyy'-'MM'-'dd' 'HH':'mm':'ss");
            json.Source = fileInfo.FullName;
            json.Destination = destinationPath;
            json.RemainingFiles = remainingFile.ToString();
            json.RemainingData = remainingData.ToString();
            json.Advancement = advancement.ToString();

            message = JsonSerializer.Serialize(json);

            // Json deserializedProduct = JsonConvert.DeserializeObject<Json>(output);
            
            try
            {
                using (var writer = File.AppendText(filePath))
                {
                    writer.WriteLine(message);
                }
            }
            catch (Exception exception)
            {
                LoggerUtility.WriteLog(LoggerUtility.Error, exception.Message);
                switch (exception.GetType().Name)
                {
                    case nameof(UnauthorizedAccessException):
                        Console.WriteLine($"Access denied: {filePath}");
                        break;
                    case nameof(FileNotFoundException):
                        Console.WriteLine(exception.Message);
                        if (!File.Exists(filePath))
                        {
                            CreateFile(filePath);
                        }
                        break;
                }
            }
        }

        public static object ReadState(string id)
        {
            string output = null;
            Json deserializedProduct = JsonSerializer.Deserialize<Json>(output);
            
            
            
            return null;
        }
        
        
        public class BackupFile
        {
            [JsonPropertyName("BackupID")]
            public int BackupID { get; set; }

            [JsonPropertyName("Source")]
            public string Source { get; set; }

            [JsonPropertyName("Destination")]
            public string Destination { get; set; }

            [JsonPropertyName("Advancement")]
            public string Advancement { get; set; }
        }
        
        public List<BackupFile> GetFilesByBackupId(int backupId)
        {
            string folderName = "EasySave";
            string fileName = "statefile.log";
            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), folderName, fileName);
            if (File.Exists(filePath))
            {
                string jsonData = File.ReadAllText(filePath);
                var backupEntries = JsonSerializer.Deserialize<List<BackupFile>>(jsonData, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                
                if (backupEntries == null) return new List<BackupFile>();
                
                var filteredEntries = backupEntries
                    .Where(entry => entry.BackupID == backupId)
                    .ToList();

                var backupFiles = filteredEntries.Select(entry => new BackupFile
                {
                    Source = entry.Source,
                    Destination = entry.Destination,
                    Advancement = entry.Advancement
                }).ToList();

                return backupFiles;
            }
            return null;
        }

    }
}
