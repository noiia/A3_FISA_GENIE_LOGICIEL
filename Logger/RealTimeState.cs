using System.Collections.Generic;
using System.IO;
using System;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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

            message = JsonConvert.SerializeObject(json);

            // Json deserializedProduct = JsonConvert.DeserializeObject<Json>(output);
            
            try
            {
                // 100%
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
            //tests
            
            //# TODO try pause and resume here before implement
            
            // var advancementByBackupId = GetFilesAdvancementByBackupId(Convert.ToInt32(id));
            // foreach (var advancement in advancementByBackupId)
            // {
            //     Console.WriteLine($"advancement : {advancement.Source} {advancement.Destination} {advancement.Advancement}");
            //     if (advancement.Advancement.EndsWith("100%"))
            //     {
            //         Console.WriteLine($"finished job {advancement.Destination}");
            //     }
            //     else
            //     {
            //         Console.WriteLine(advancement.Destination + " : " + advancement.Advancement);
            //     }
            // }
            // ContinueSaveJob(int.Parse(id));

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

            message = JsonConvert.SerializeObject(json);

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

        // public static object ReadState(string id)
        // {
        //     string output = null;
        //     Json deserializedProduct = JsonConvert.DeserializeObject<Json>(output);
        //     
        //     
        //     
        //     return null;
        // }
        
        
        public class BackupFile
        {
            public string Source { get; set; }
            public string Destination { get; set; }
            public string Advancement { get; set; }
        }
        
        public static List<BackupFile> GetFilesAdvancementByBackupId(int backupId)
        {
            string folderName = "EasySave";
            string fileName = "statefile.log";
            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), folderName, fileName);

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
                        .ToList();

                    var groupedEntries = filteredEntries
                        .GroupBy(entry => entry["Destination"].Value<string>())
                        .Select(group =>
                        {
                            // Select the last entry in the file for each destination
                            var lastEntry = group.Last();
                            return new BackupFile
                            {
                                Source = lastEntry["Source"].Value<string>(),
                                Destination = lastEntry["Destination"].Value<string>(),
                                Advancement = lastEntry["Advancement"].Value<string>().EndsWith("100%") ? "100%" : lastEntry["Advancement"].Value<string>()
                            };
                        })
                        .ToList();

                    return groupedEntries;
                }
                catch (Exception ex)
                {
                    // Handle exceptions (e.g., log the error)
                    Console.WriteLine($"Error reading or deserializing the file: {ex.Message}");
                    return null;
                }
            }
            return null;
        }
        
        
        
        public static void ContinueSaveJob(int backupId)
        {
            List<BackupFile> backupFiles = GetFilesAdvancementByBackupId(backupId);

            foreach (BackupFile backupFile in backupFiles)
            {
                string source = backupFile.Source;
                string destination = backupFile.Destination;
                string advancement = backupFile.Advancement;
                // FileInfo fileInfo = new FileInfo(source);
                // Counters counter = new Counters(1, 1, true);
                // counter.FileCount = 1;
                // counter.DataCount = fileInfo.Length;
                // WriteState("ContinueSaveJob", counter, fileInfo, destination, "statefile.log", "Continue Save Job", backupId.ToString(), double.Parse(advancement));
            }
        }
    }
}
