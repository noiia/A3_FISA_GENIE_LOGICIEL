using System.Text.Json;
using Config;
using Job.Model;

namespace Job.Config
{
    public class Configuration
    {
        private string _configPath;
        private ConfigFile _configFile;

        public Configuration(string configPath)
        {
            this._configPath = configPath;
            this.LoadConfiguration();
        }

        public void LoadConfiguration()
        {
            if (!File.Exists(this._configPath))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(this._configPath));
                string defaultLogPath =
                    (Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\EasySave\\Logs\\")
                    .Replace("\\", "/");
                ConfigFile tempConfigFile = new ConfigFile([], defaultLogPath, "CryptoKey", "en", "json", [],[]);
                string json = JsonSerializer.Serialize(tempConfigFile);
                File.WriteAllText(this._configPath, json);
            }

            string fileContent = File.ReadAllText(this._configPath);
            this._configFile = JsonSerializer.Deserialize<ConfigFile>(fileContent);
        }

        public void SaveConfiguration()
        {
            string json = JsonSerializer.Serialize(this._configFile);
            File.WriteAllText(this._configPath, json);
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

        public void AddSaveJob(int id, string name, string source, string destination, DateTime lastSave, DateTime created, string type)
        {
            SaveJob newSaveJob = new SaveJob(id, name, source, destination, lastSave, created, type);
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
    }
}
