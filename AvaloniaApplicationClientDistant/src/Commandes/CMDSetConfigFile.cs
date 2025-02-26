using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AvaloniaApplicationClientDistant.Commandes;

public class CMDSetConfigFile:CMD
{
    private string _logPath;
    private string _cryptoKey;
    private string _language; //fr || en
    private string _logType; //json || xml
    private string[]? _cryptExtension;
    private string[]? _buisnessApp;

    public CMDSetConfigFile(string logPath, string cryptoKey, string language, string logType, string[]? cryptExtension, string[]? buisnessApp) : base("SetConfigFile")
    {
        _logPath = logPath;
        _cryptoKey = cryptoKey;
        _language = language;
        _logType = logType;
        _cryptExtension = cryptExtension;
        _buisnessApp = buisnessApp;
    }

    public override string toString()
    {
        JObject json = new JObject();
        json.Add("commande", base.Command);
        json.Add("logPath", _logPath);
        json.Add("cryptoKey", _cryptoKey);
        json.Add("language", _language);
        json.Add("logType", _logType);
        json["cryptExtension"] = JToken.FromObject(_cryptExtension);
        json["buisnessApp"] = JToken.FromObject(_buisnessApp);
        string jsonString = JsonConvert.SerializeObject(json);
        return jsonString;
    }   
}