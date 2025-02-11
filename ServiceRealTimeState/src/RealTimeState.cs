using Logger;
using System.Reflection.Metadata.Ecma335;

namespace ServiceRealTimeState;

public class Counters {
    private static int _nextId = 1;
    public int Id { get; private set; }
    public int FileCount { get; set; }
    public int TransferedFileCount { get; set; }
    public double DataCount { get; set; }
    public double TransferedData { get; set; }
    public bool IsActive { get; set; }

    public Counters(int dataCount, int fileCount, bool isActive) {
        Id = _nextId++;
        DataCount = dataCount;
        FileCount = fileCount;
        IsActive = isActive;
    }
}

public class RealTimeState

{
    private static List<Counters> counters = new List<Counters>();
    public static void AddCounter(Counters counter)
    {
        counters.Add(counter);
    }

    public static Counters GetCounterById(int i)
    {
        foreach (var counter in counters)
        {
            if (counter.Id == i )
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
    public static void WriteState(string SaveJobName, Counters counter, string[] fileInfo, string destinationPathDir, string fileName, string msg)
    {
        const string folderName = "EasySave";
        
        bool header = false;
        
        var filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), folderName, fileName);
        var destinationPath = Path.Combine(destinationPathDir, fileInfo[0]);
        counter.TransferedFileCount++;
        counter.TransferedData += double.Parse(fileInfo[2]);
        
        double remainingFile = counter.FileCount - counter.TransferedFileCount;
        double remainingData = counter.DataCount - counter.TransferedData;
        string message = $"{DateTime.Now.Date.ToString()} : Source :{fileInfo[1]}, Destination:{destinationPath} remaining files :{remainingFile}, remaining data : {remainingData}";

        if (!header)
        {
            using (var writer = File.AppendText(filePath))
            {
                string isActive = counter.IsActive ? "Active" : "Not Active";
                string headerMessage = $"{SaveJobName} : {isActive}";
                writer.WriteLine(headerMessage);
            }
        }
        
        try
        {
            using (var writer = File.AppendText(filePath))
            {
                writer.WriteLine(message);
            }
        }
        catch(Exception exception)
        {
            LoggerUtility.WriteLog(LoggerUtility.Error,exception.Message);
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
}