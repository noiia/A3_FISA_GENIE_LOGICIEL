using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Logger;

public class Counters
{
    private static int _nextId = 1;

    public Counters(double dataCount, int fileCount, bool isActive)
    {
        Id = _nextId++;
        DataCount = dataCount;
        FileCount = fileCount;
        IsActive = isActive;
    }

    public Counters(double dataCount, int fileCount, bool isActive, int iD)
    {
        Id = iD;
        DataCount = dataCount;
        FileCount = fileCount;
        IsActive = isActive;
    }

    public int Id { get; }
    public int FileCount { get; set; }
    public int TransferedFileCount { get; set; }
    public double DataCount { get; set; }
    public double TransferedData { get; set; }
    public bool IsActive { get; set; }
}

public class Json
{
    public string BackupID { get; set; }
    public string SaveJobID { get; set; }
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
    private static readonly string filePath =
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EasySave", "statefile.log");

    private static readonly List<Counters> counters = new();

    public static void AddCounter(Counters counter)
    {
        counters.Add(counter);
    }

    public static Counters GetCounterById(int i)
    {
        foreach (var counter in counters)
            if (counter.Id == i)
                return counter;

        return null;
    }

    private static void CreateFile(string filePath)
    {
        using (File.Create(filePath))
        {
            //merge error ?
            LoggerUtility.WriteLog(LoggerUtility.JSON, LoggerUtility.Info,
                $"Real time state file created at : {filePath}");
        }
    }


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
            LoggerUtility.WriteLog(LoggerUtility.JSON, LoggerUtility.Error, exception.Message);
            switch (exception.GetType().Name)
            {
                case nameof(UnauthorizedAccessException):
                    Console.WriteLine($"Access denied: {filePath}");
                    break;
                case nameof(FileNotFoundException):
                    Console.WriteLine(exception.Message);
                    if (!File.Exists(filePath)) CreateFile(filePath);
                    break;
            }
        }
    }

    public static void WriteState(string saveJobId, Counters counter, FileInfo fileInfo, string destinationPath,
        string fileName, string msg, string id)
    {
        const string folderName = "EasySave";

        var header = false;

        counter.TransferedFileCount++;
        counter.TransferedData += fileInfo.Length;

        double remainingFile = counter.FileCount - counter.TransferedFileCount;
        var remainingData = counter.DataCount - counter.TransferedData;

        string message;

        var json = new Json();

        json.BackupID = id;
        // json.Name = saveJobName;
        json.SaveJobID = saveJobId;
        json.IsActive = counter.IsActive ? "Active" : "Not Active";
        json.DateTime = DateTime.Now.ToString("yyyy'-'MM'-'dd' 'HH':'mm':'ss");
        json.Source = fileInfo.FullName;
        json.Destination = destinationPath;
        json.RemainingFiles = remainingFile.ToString();
        json.RemainingData = remainingData.ToString();
        json.Advancement = $"{fileInfo.Length} 100%";

        message = JsonConvert.SerializeObject(json);

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
            //merge error ?
            LoggerUtility.WriteLog(LoggerUtility.JSON, LoggerUtility.Error, exception.Message);
            switch (exception.GetType().Name)
            {
                case nameof(UnauthorizedAccessException):
                    Console.WriteLine($"Access denied: {filePath}");
                    break;
                case nameof(FileNotFoundException):
                    Console.WriteLine(exception.Message);
                    if (!File.Exists(filePath)) CreateFile(filePath);
                    break;
            }
        }
        //tests

        //# TODO try pause and resume here before implement
    }


    public static void WriteState(string saveJobID, Counters counter, FileInfo fileInfo, string destinationPath,
        string fileName, string msg, string id, double advancement)
    {
        const string folderName = "EasySave";

        var header = false;

        double remainingFile = counter.FileCount - counter.TransferedFileCount;
        var remainingData = counter.DataCount - counter.TransferedData - advancement;

        var message =
            $"{DateTime.Now.ToString("yyyy'-'MM'-'dd' 'HH':'mm':'ss")} : Source : {fileInfo.FullName}, Destination:{destinationPath} remaining files :{remainingFile}, remaining data : {remainingData}";

        var json = new Json();

        json.BackupID = id;
        json.SaveJobID = saveJobID;
        json.IsActive = counter.IsActive ? "Active" : "Not Active";
        json.DateTime = DateTime.Now.ToString("yyyy'-'MM'-'dd' 'HH':'mm':'ss");
        json.Source = fileInfo.FullName;
        json.Destination = destinationPath;
        json.RemainingFiles = remainingFile.ToString();
        json.RemainingData = remainingData.ToString();
        json.Advancement = advancement.ToString();

        message = JsonConvert.SerializeObject(json);

        try
        {
            using (var writer = File.AppendText(filePath))
            {
                writer.WriteLine(message);
            }
        }
        catch (Exception exception)
        {
            //merge error
            LoggerUtility.WriteLog(LoggerUtility.JSON, LoggerUtility.Error, exception.Message);
            switch (exception.GetType().Name)
            {
                case nameof(UnauthorizedAccessException):
                    Console.WriteLine($"Access denied: {filePath}");
                    break;
                case nameof(FileNotFoundException):
                    Console.WriteLine(exception.Message);
                    if (!File.Exists(filePath)) CreateFile(filePath);
                    break;
            }
        }
    }

    public static List<BackupFile> GetFilesAdvancementByBackupId(int backupId)
    {
        var folderName = "EasySave";
        var fileName = "statefile.log";
        var filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), folderName,
            fileName);

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
                            Advancement = lastEntry["Advancement"].Value<string>().EndsWith("100%")
                                ? "100%"
                                : lastEntry["Advancement"].Value<string>()
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

        return null;
    }

    public class BackupFile
    {
        public string Source { get; set; }
        public string Destination { get; set; }
        public string Advancement { get; set; }
    }
}