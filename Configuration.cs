using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace EasySave
{
    public class Configuration
    {
        private string configPath;
        private ConfigFile configFile;

        public Configuration(string configPath)
        {
            this.configPath = configPath;
            Console.WriteLine($"Path: {configPath}");
        }

        public void loadConfiguration()
        {
            if (File.Exists(this.configPath))
            {
                Console.WriteLine("Le fichier existe");
            }
            else
            {
                Console.WriteLine("Création du fichier ");
                Directory.CreateDirectory(Path.GetDirectoryName(this.configPath));
                File.WriteAllText(this.configPath, "{\"SaveJobs\":[]}\n");
            }

            string fileContent = File.ReadAllText(this.configPath);
            this.configFile = JsonConvert.DeserializeObject<ConfigFile>(fileContent);
        }

        public void saveConfiguration()
        {
            string json = JsonConvert.SerializeObject(this.configFile, Formatting.Indented);
            File.WriteAllText(this.configPath, json);
        }

        public SaveJob getSaveJob(int id)
        {
            return configFile.SaveJobs.FirstOrDefault(job => job.Id == id);
        }

        public SaveJob getSaveJob(string name)
        {
            return configFile.SaveJobs.FirstOrDefault(job => job.Name == name);
        }

        public SaveJob[] getSaveJobs()
        {
            return configFile.SaveJobs;
        }

        public void addSaveJob(SaveJob saveJob)
        {
            configFile.SaveJobs = configFile.SaveJobs.Append(saveJob).ToArray();
            saveConfiguration();
        }

        public void addSaveJob(int id, string name, string source, string destination, DateTime lastSave, DateTime created)
        {
            SaveJob newSaveJob = new SaveJob(id, name, source, destination, lastSave, created);
            addSaveJob(newSaveJob);
        }

        public void deleteSaveJob(SaveJob dsaveJob)
        {
            deleteSaveJob(dsaveJob.Id);
        }

        public void deleteSaveJob(int id)
        {
            configFile.SaveJobs = configFile.SaveJobs.Where(job => job.Id != id).ToArray();
            saveConfiguration();
        }

        public void deleteSaveJob(string name)
        {
            configFile.SaveJobs = configFile.SaveJobs.Where(job => job.Name != name).ToArray();
            saveConfiguration();
        }
    }
}
