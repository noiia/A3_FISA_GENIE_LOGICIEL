using System.Text.Json;
using Config;
using Job.Model;

namespace Job.Config
{
    public class Configuration
    {
        private string _configPath;
        private ConfigFile _configFile;
        private readonly Mutex _mutex = new Mutex();


        public Configuration(string configPath)
        {
            
            this._configPath = configPath;
            this.LoadConfiguration();
        }
        
        

        public ConfigFile ConfigFile
        {
            get => _configFile;
            set => _configFile = value ?? throw new ArgumentNullException(nameof(value));
        }

        public void LoadConfiguration()
        {
            _mutex.WaitOne();
            try
            {
                if (!File.Exists(this._configPath))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(this._configPath));
                    string defaultLogPath =
                        (Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\EasySave\\")
                        .Replace("\\", "/");
                    ConfigFile tempConfigFile = new ConfigFile([], defaultLogPath, "CryptoKey", "en", "json", [],[],[]);
                    string json = JsonSerializer.Serialize(tempConfigFile);
                    File.WriteAllText(this._configPath, json);
                }

                string fileContent = File.ReadAllText(this._configPath);
                this._configFile = JsonSerializer.Deserialize<ConfigFile>(fileContent);
            }
            finally
            {
                _mutex.ReleaseMutex();
            }
        }

        public void SaveConfiguration()
        {
            _mutex.WaitOne();
            try
            {
                string json = JsonSerializer.Serialize(this._configFile);
                File.WriteAllText(this._configPath, json);
            }
            finally
            {
                _mutex.ReleaseMutex();
            }
        }

        public SaveJob GetSaveJob(int id)
        {
            if (_configFile == null || _configFile.SaveJobs == null)
            {
                return null;
            }
            return _configFile.SaveJobs.FirstOrDefault(job => job.Id == id);
        }

        public SaveJob GetSaveJob(string name)
        {
            if (_configFile == null || _configFile.SaveJobs == null)
            {
                return null;
            }
            return _configFile.SaveJobs.FirstOrDefault(job => job.Name == name);
        }

        public SaveJob[] GetSaveJobs()
        {
            return _configFile.SaveJobs;
        }

        public void SetSaveJobs(SaveJob[] saveJobs)
        {
            Logger.LoggerUtility.WriteLog(GetLogType(), Logger.LoggerUtility.Info, "SetSaveJobs");
            // Console.WriteLine("SetSaveJobs");
            // foreach (var VARIABLE in saveJobs)
            // {
            //     Console.WriteLine($"Saving job {VARIABLE.Name}");
            // }
            _configFile.SaveJobs = saveJobs;
            SaveConfiguration();
        }

        public void AddSaveJob(SaveJob saveJob)
        {
            if (this.GetSaveJob(saveJob.Id) == null && this.GetSaveJob(saveJob.Name) == null){
                _configFile.SaveJobs = _configFile.SaveJobs.Append(saveJob).ToArray();
                this.SaveConfiguration();
            }
            else
            {
                if (this.GetSaveJob(saveJob.Id) != null)
                {
                    throw new Exception("A SaveJob already exists with the same ID");
                }
                else
                {
                    throw new Exception("A SaveJob already exists with the same Name");
                }
            }
        }

        public void AddSaveJob(int id, string name, string source, string destination, DateTime lastSave, DateTime created, string status, string type)
        {
            SaveJob newSaveJob = new SaveJob(id, name, source, destination, lastSave, created, status, type);
            this.AddSaveJob(newSaveJob);
        }

        public void DeleteSaveJob(SaveJob dsaveJob)
        {
            this.DeleteSaveJob(dsaveJob.Id);
        }

        public void DeleteSaveJob(int id)
        {
            if (this.GetSaveJob(id) != null)
            {
                _configFile.SaveJobs = _configFile.SaveJobs.Where(job => job.Id != id).ToArray();
                this.SaveConfiguration();
            }
            else
            {
                throw new Exception("There is no SaveJob with this ID");
            }
        }

        public void DeleteSaveJob(string name)
        {
            if (this.GetSaveJob(name) != null)
            {
                _configFile.SaveJobs = _configFile.SaveJobs.Where(job => job.Name != name).ToArray();
                this.SaveConfiguration();
            }
            else
            {
                throw new Exception("There is no SaveJob with this name");
            }
        }

        public void SetLengthLimit(int lengthLimit)
        {
            _configFile.LengthLimit = lengthLimit;
        }

        public int GetLengthLimit()
        {
            return _configFile.LengthLimit;
        }

        //abuse encore un peu plus la prochaine fois
        public int FindFirstFreeId()
        {
            for (int i = 0; i < 2147483647; i++)
            {
                if (this.GetSaveJob(i) == null)
                {
                    return i;
                }
            }
            return -1;
        }

        public string? GetLogPath(){
            return this._configFile.LogPath;
        }

        public void SetLogPath(string logPath){
            this._configFile.LogPath = logPath;
            this.SaveConfiguration();
            return;
        }

        public void SetLanguage(string language)
        {
            this._configFile.Language = language;
            this.SaveConfiguration();
            return;
        }

        public string GetLanguage()
        {
            return this._configFile.Language;
        }

        public void SetLogType(string logType)
        {
            this._configFile.LogType = logType;
            this.SaveConfiguration();
        }

        public string GetLogType()
        {
            return this._configFile.LogType;
        }

        public void SetCryptExtension(string[] cryptExtension)
        {
            this._configFile.CryptExtension = cryptExtension;
            this.SaveConfiguration();
        }

        public string[] GetCryptExtension()
        {
            return this._configFile.CryptExtension;
        }

        public void AddCryptExtension(string cryptExtension)
        {
            Console.WriteLine(cryptExtension);
            Console.WriteLine(this._configFile.CryptExtension.ToString());
            if (!this._configFile.CryptExtension.Contains(cryptExtension))
            {
                this._configFile.CryptExtension = this._configFile.CryptExtension.Append(cryptExtension).ToArray();
                this.SaveConfiguration();
            }
        }

        public void RemoveCryptExtension(string cryptExtension)
        {
            if (this._configFile.CryptExtension.Contains(cryptExtension))
            {
                this._configFile.CryptExtension = this._configFile.CryptExtension.Where(ext => ext != cryptExtension).ToArray();
                this.SaveConfiguration();
            }
        }

        public void SetBuisnessApp(string[] buisnessApp)
        {
            this._configFile.BuisnessApp = buisnessApp;
            this.SaveConfiguration();
        }

        public string[] GetBuisnessApp()
        {
            return this._configFile.BuisnessApp;
        }

        public void AddBuisnessApp(string buisnessApp)
        {
            if (!this._configFile.BuisnessApp.Contains(buisnessApp))
            {
                this._configFile.BuisnessApp = this._configFile.BuisnessApp.Append(buisnessApp).ToArray();
                this.SaveConfiguration();
            }
        }

        public void RemoveBuisnessApp(string buisnessApp)
        {
            if (this._configFile.BuisnessApp.Contains(buisnessApp))
            {
                this._configFile.BuisnessApp = this._configFile.BuisnessApp.Where(app => app != buisnessApp).ToArray();
                this.SaveConfiguration();
            }
        }
        
        
        public void SetCryptKey(string cryptKey)
        {
            this._configFile.CryptoKey = cryptKey;
            this.SaveConfiguration();
        }

        public string GetCryptKey()
        {
            return this._configFile.CryptoKey;
        }
        
        
        public void SetFileExtension(string[] fileExtension)
        {
            this._configFile.FileExtension = fileExtension;
            this.SaveConfiguration();
        }

        public string[] GetFileExtension()
        {
            return this._configFile.FileExtension;
        }
        
        public void AddFileExtension(string fileExtension)
        {
            if (!this._configFile.FileExtension.Contains(fileExtension))
            {
                this._configFile.FileExtension = this._configFile.FileExtension.Append(fileExtension).ToArray();
                this.SaveConfiguration();
            }
        }

        public void RemoveFileExtension(string fileExtension)
        {
            if (this._configFile.FileExtension.Contains(fileExtension))
            {
                this._configFile.FileExtension = this._configFile.FileExtension.Where(app => app != fileExtension).ToArray();
                this.SaveConfiguration();
            }
        }
        
        
        public void SetMaxFileSize(int maxFileSize)
        {
            this._configFile.LengthLimit = maxFileSize;
            this.SaveConfiguration();
        }

        public int GetMaxFileSize()
        {
            return this._configFile.LengthLimit;
        }
    }
}
