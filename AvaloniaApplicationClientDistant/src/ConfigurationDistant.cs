using System;
using System.Linq;
using System.Text.Json;
using System.Threading;
using AvaloniaApplicationClientDistant.Commandes;
using Config;
using Job.Config;
using Logger;

namespace AvaloniaApplicationClientDistant;

public class ConfigurationDistant
{
    private static ConfigurationDistant _instance;
    private ConfigFile _configFile;

    // Constructeur privé pour empêcher l'instanciation directe
    private ConfigurationDistant()
    {
        var client = Client.GetInstance();
        client.SendMessage(new CMDGetConfig());
    }

    public ConfigFile ConfigFile
    {
        get => _configFile;
        set => _configFile = value ?? throw new ArgumentNullException(nameof(value));
    }

    public void WaitLoad()
    {
        while (_configFile == null) Thread.Sleep(500);
    }

    // Propriété statique pour accéder à l'instance unique
    public static ConfigurationDistant GetInstance()
    {
        if (_instance == null) _instance = new ConfigurationDistant();
        return _instance;
    }

    public void SaveConfiguration()
    {
        var json = JsonSerializer.Serialize(_configFile);
        var client = Client.GetInstance();
        client.SendMessage(new CMDSetConfigFile(_configFile.SaveJobs, _configFile.LogPath, _configFile.CryptoKey,
            _configFile.Language, _configFile.LogType, _configFile.CryptExtension, _configFile.BuisnessApp));
    }

    public SaveJob GetSaveJob(int id)
    {
        if (_configFile == null || _configFile.SaveJobs == null) return null;

        return _configFile.SaveJobs.FirstOrDefault(job => job.Id == id);
    }

    public SaveJob GetSaveJob(string name)
    {
        if (_configFile == null || _configFile.SaveJobs == null) return null;

        return _configFile.SaveJobs.FirstOrDefault(job => job.Name == name);
    }

    public SaveJob[] GetSaveJobs()
    {
        return _configFile.SaveJobs;
    }

    public void SetSaveJobs(SaveJob[] saveJobs)
    {
        LoggerUtility.WriteLog(GetLogType(), LoggerUtility.Info, "SetSaveJobs");
        Console.WriteLine("SetSaveJobs");
        foreach (var VARIABLE in saveJobs) Console.WriteLine($"Saving job {VARIABLE.Name}");

        _configFile.SaveJobs = saveJobs;
        SaveConfiguration();
    }

    public void AddSaveJob(SaveJob saveJob)
    {
        if (GetSaveJob(saveJob.Id) == null && GetSaveJob(saveJob.Name) == null)
        {
            _configFile.SaveJobs = _configFile.SaveJobs.Append(saveJob).ToArray();
            SaveConfiguration();
        }
        else
        {
            if (GetSaveJob(saveJob.Id) != null) throw new Exception("A SaveJob already exists with the same ID");

            throw new Exception("A SaveJob already exists with the same Name");
        }
    }

    public void AddSaveJob(int id, string name, string source, string destination, DateTime lastSave, DateTime created,
        string status, string type)
    {
        var newSaveJob = new SaveJob(id, name, source, destination, lastSave, created, status, type);
        AddSaveJob(newSaveJob);
    }

    public void DeleteSaveJob(SaveJob dsaveJob)
    {
        DeleteSaveJob(dsaveJob.Id);
    }

    public void DeleteSaveJob(int id)
    {
        if (GetSaveJob(id) != null)
        {
            _configFile.SaveJobs = _configFile.SaveJobs.Where(job => job.Id != id).ToArray();
            SaveConfiguration();
        }
        else
        {
            throw new Exception("There is no SaveJob with this ID");
        }
    }

    public void DeleteSaveJob(string name)
    {
        if (GetSaveJob(name) != null)
        {
            _configFile.SaveJobs = _configFile.SaveJobs.Where(job => job.Name != name).ToArray();
            SaveConfiguration();
        }
        else
        {
            throw new Exception("There is no SaveJob with this name");
        }
    }

    public int FindFirstFreeId()
    {
        for (var i = 0; i < 2147483647; i++)
            if (GetSaveJob(i) == null)
                return i;

        return -1;
    }

    public string? GetLogPath()
    {
        return _configFile.LogPath;
    }

    public void SetLogPath(string logPath)
    {
        _configFile.LogPath = logPath;
        SaveConfiguration();
    }

    public void SetLanguage(string language)
    {
        _configFile.Language = language;
        SaveConfiguration();
    }

    public string GetLanguage()
    {
        return _configFile.Language;
    }

    public void SetLogType(string logType)
    {
        _configFile.LogType = logType;
        SaveConfiguration();
    }

    public string GetLogType()
    {
        return _configFile.LogType;
    }

    public void SetCryptExtension(string[] cryptExtension)
    {
        _configFile.CryptExtension = cryptExtension;
        SaveConfiguration();
    }

    public string[] GetCryptExtension()
    {
        return _configFile.CryptExtension;
    }

    public void AddCryptExtension(string cryptExtension)
    {
        if (!_configFile.CryptExtension.Contains(cryptExtension))
        {
            _configFile.CryptExtension = _configFile.CryptExtension.Append(cryptExtension).ToArray();
            SaveConfiguration();
        }
    }

    public void RemoveCryptExtension(string cryptExtension)
    {
        if (_configFile.CryptExtension.Contains(cryptExtension))
        {
            _configFile.CryptExtension =
                _configFile.CryptExtension.Where(ext => ext != cryptExtension).ToArray();
            SaveConfiguration();
        }
    }

    public void SetBuisnessApp(string[] buisnessApp)
    {
        _configFile.BuisnessApp = buisnessApp;
        SaveConfiguration();
    }

    public string[] GetBuisnessApp()
    {
        return _configFile.BuisnessApp;
    }

    public void AddBuisnessApp(string buisnessApp)
    {
        if (!_configFile.BuisnessApp.Contains(buisnessApp))
        {
            _configFile.BuisnessApp = _configFile.BuisnessApp.Append(buisnessApp).ToArray();
            SaveConfiguration();
        }
    }

    public void RemoveBuisnessApp(string buisnessApp)
    {
        if (_configFile.BuisnessApp.Contains(buisnessApp))
        {
            _configFile.BuisnessApp = _configFile.BuisnessApp.Where(app => app != buisnessApp).ToArray();
            SaveConfiguration();
        }
    }


    public void SetCryptKey(string cryptKey)
    {
        _configFile.CryptoKey = cryptKey;
        SaveConfiguration();
    }

    public string GetCryptKey()
    {
        return _configFile.CryptoKey;
    }


    public void SetFileExtension(string[] fileExtension)
    {
        _configFile.FileExtension = fileExtension;
        SaveConfiguration();
    }

    public string[] GetFileExtension()
    {
        return _configFile.FileExtension;
    }

    public void AddFileExtension(string fileExtension)
    {
        if (!_configFile.FileExtension.Contains(fileExtension))
        {
            _configFile.FileExtension = _configFile.FileExtension.Append(fileExtension).ToArray();
            SaveConfiguration();
        }
    }

    public void RemoveFileExtension(string fileExtension)
    {
        if (_configFile.FileExtension.Contains(fileExtension))
        {
            _configFile.FileExtension =
                _configFile.FileExtension.Where(app => app != fileExtension).ToArray();
            SaveConfiguration();
        }
    }

    public void SetMaxFileSize(int maxFileSize)
    {
        _configFile.LengthLimit = maxFileSize;
        SaveConfiguration();
    }

    public int GetMaxFileSize()
    {
        return _configFile.LengthLimit;
    }
}