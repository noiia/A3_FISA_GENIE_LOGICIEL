using System;
using System.IO;
using System.Text.Json;

using Config;

namespace Logger;
    public class LogStructure
    {
        public DateTime CreateDate { get; set; }
        public string LogType { get; set; }
        public string Message { get; set; }
    }

    public static class LoggerUtility
    {
        public const string Error = "ERROR";
        public const string Warning = "WARNING";
        public const string Info = "INFO";
        public const string Debug = "DEBUG";

        /// <summary>
        ///     Write your log in a file using JSON data format.
        /// </summary>
        /// <param name="type">Add the type of log. (ex: INFO, WARNING, ERROR, DEBUG)</param>
        /// <param name="msg">Add you custom message to the log.</param>
        public static void WriteLog(string type, string msg)
        {
            const string folderName = "EasySave";
            var message = new LogStructure { CreateDate = DateTime.Now, LogType = type, Message = msg };
            var fileName = "log_" + DateTime.Now.Date.ToString("dd-MM-yyyy") + ".log";

            Configuration config =
                new Configuration(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                                  "\\EasySave\\" + "config.json");
            
            if (config.GetLogPath() == null)
            {
                config.SetLogPath(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), folderName));
            }    
                
            var filePath = Path.Combine(config.GetLogPath(), fileName);

            Directory.CreateDirectory(config.GetLogPath());


            if (!File.Exists(filePath))
                using (File.Create(filePath))
                {
                    Console.WriteLine("Log file created at : " + filePath);
                }

            using (var writer = File.AppendText(filePath))
            {
                writer.WriteLine(JsonSerializer.Serialize(message));
            }
        }
    }
