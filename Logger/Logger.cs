using System;
using System.IO;
using System.Text.Json;
using System.Xml.Serialization;

namespace Logger;
    public class LogStructure
    {
        public DateTime CreateDate { get; set; }
        public string LogType { get; set; }
        public string Message { get; set; }
    }

    public static class LoggerUtility
    {
        private static string _logPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData); 
        public static string LogPath
        {
            get => _logPath;
            set => _logPath = value; 
        }
        
        
        public const string Error = "ERROR";
        public const string Warning = "WARNING";
        public const string Info = "INFO";
        public const string Debug = "DEBUG";
        
        public const string JSON = "json";
        public const string XML = "xml";

        /// <summary>
        ///     Write your log in a file using JSON data format.
        /// </summary>
        /// <param name="logFileType"></param>
        /// <param name="type">Add the type of log. (ex: INFO, WARNING, ERROR, DEBUG)</param>
        /// <param name="msg">Add you custom message to the log.</param>
        public static void WriteLog(string logFileType, string type, string msg)
        {
            const string folderName = "EasySave";
            var message = new LogStructure { CreateDate = DateTime.Now, LogType = type, Message = msg };
            var fileName = "log_" + DateTime.Now.Date.ToString("dd_MM_yyyy") + "."+logFileType;
                
            var filePath = Path.Combine(LogPath,folderName, fileName);

            Directory.CreateDirectory(LogPath);


            if (!File.Exists(filePath))
                using (File.Create(filePath))
                {
                    Console.WriteLine("Log file created at : " + filePath);
                }

            try
            {
                switch (logFileType)
                {
                    case "json":
                    {
                        using (var writer = File.AppendText(filePath))
                        {
                            writer.WriteLine(JsonSerializer.Serialize(message));
                            break;
                        }
                    }
                    case "xml":
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(LogStructure));
                        using (StreamWriter writer = new StreamWriter(filePath, true))
                        {
                            serializer.Serialize(writer, message);
                        }
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
