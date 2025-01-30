using System.Text.Json;

namespace ConsoleApp1;

public class Configuration
{
    
    private string configPath;
    private ConfigFile configFile;
    
    public Configuration(string configPath)
    {
        this.configPath = configPath;
        Console.WriteLine($"Path: {configPath}");
        return;
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
        this.configFile = JsonSerializer.Deserialize<ConfigFile>(fileContent);
        return;
    }

    public void saveConfiguration()
    {
        string json = JsonSerializer.Serialize(this.configFile);
        File.WriteAllText(this.configPath, json);
        return;
    }

    public SaveJob getSaveJob(int id)
    {
        SaveJob[] saveJobs = configFile.SaveJobs;
        foreach (SaveJob saveJob in saveJobs)
        {
            if (saveJob.Id == id)
            {
                return saveJob;
            }
        }
        return null;
    }
    
    public SaveJob getSaveJob(string name)
    {
        SaveJob[] saveJobs = configFile.SaveJobs;
        foreach (SaveJob saveJob in saveJobs)
        {
            if (saveJob.Name == name)
            {
                return saveJob;
            }
        }
        return null;
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
        configFile.SaveJobs = configFile.SaveJobs.Append(newSaveJob).ToArray();
        saveConfiguration();
    }

    public void deleteSaveJob(SaveJob dsaveJob)
    {
        configFile.SaveJobs = configFile.SaveJobs.Where(job => job.Id != dsaveJob.Id).ToArray();
        saveConfiguration();
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