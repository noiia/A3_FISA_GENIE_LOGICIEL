using Job.Config;

namespace Config;

public class ConfigFile
{
    private string[]? _buisnessApp;
    private string[]? _cryptExtension;
    private string _cryptoKey;
    private string[]? _fileExtension;
    private string _language; //fr || en
    private string _logPath;
    private string _logType; //json || xml
    private SaveJob[] _saveJobs;

    public ConfigFile(SaveJob[] saveJobs, string logPath, string cryptoKey, string language, string logType,
        string[] cryptExtension, string[] buisnessApp, string[] fileExtension, int progress = 0)
    {
        _saveJobs = saveJobs;
        _logPath = logPath;
        _cryptoKey = cryptoKey;
        _language = language;
        _logType = logType;
        _cryptExtension = cryptExtension;
        LengthLimit = 100;
        if (_cryptExtension == null) _cryptExtension = new string[0];
        _buisnessApp = buisnessApp;
        if (_buisnessApp == null) _buisnessApp = new string[0];
        _fileExtension = fileExtension;
        if (_fileExtension == null) _fileExtension = new string[0];
        Progress = progress;
    }

    public SaveJob[] SaveJobs
    {
        get => _saveJobs != null ? _saveJobs : Array.Empty<SaveJob>();
        set => _saveJobs = value;
    }

    public string LogPath
    {
        get => _logPath;
        set => _logPath = value ?? throw new ArgumentNullException(nameof(value));
    }

    public string Language
    {
        get => _language;
        set => _language = value ?? throw new ArgumentNullException(nameof(value));
    }

    public string LogType
    {
        get => _logType;
        set => _logType = value ?? throw new ArgumentNullException(nameof(value));
    }

    public string[] CryptExtension
    {
        get => _cryptExtension;
        set => _cryptExtension = value ?? throw new ArgumentNullException(nameof(value));
    }

    public string[] BuisnessApp
    {
        get => _buisnessApp;
        set => _buisnessApp = value ?? throw new ArgumentNullException(nameof(value));
    }

    public string CryptoKey
    {
        get => _cryptoKey;
        set => _cryptoKey = value ?? throw new ArgumentNullException(nameof(value));
    }

    public int LengthLimit { get; set; }

    public string[] FileExtension
    {
        get => _fileExtension;
        set => _fileExtension = value ?? throw new ArgumentNullException(nameof(value));
    }

    public int Progress { get; set; }
}