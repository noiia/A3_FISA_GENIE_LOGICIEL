using Job.Config;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AvaloniaApplicationClientDistant.Commandes;

public class CMDSetConfigFile : CMD
{
    private readonly string[]? _buisnessApp;
    private readonly string[]? _cryptExtension;
    private readonly string _cryptoKey;
    private readonly string _language; //fr || en
    private readonly string _logPath;
    private readonly string _logType; //json || xml
    private readonly SaveJob[] _saveJob;

    public CMDSetConfigFile(SaveJob[] saveJob, string logPath, string cryptoKey, string language, string logType,
        string[]? cryptExtension, string[]? buisnessApp) : base("SetConfigFile")
    {
        _saveJob = saveJob;
        _logPath = logPath;
        _cryptoKey = cryptoKey;
        _language = language;
        _logType = logType;
        _cryptExtension = cryptExtension;
        _buisnessApp = buisnessApp;
    }

    public override string toString()
    {
        var json = new JObject();
        json.Add("commande", Command);
        json["saveJob"] = JToken.FromObject(_saveJob);
        json.Add("logPath", _logPath);
        json.Add("cryptoKey", _cryptoKey);
        json.Add("language", _language);
        json.Add("logType", _logType);
        json["cryptExtension"] = JToken.FromObject(_cryptExtension);
        json["buisnessApp"] = JToken.FromObject(_buisnessApp);
        var jsonString = JsonConvert.SerializeObject(json);
        return jsonString;
    }
}