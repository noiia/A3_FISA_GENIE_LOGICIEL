using EasySaveServer.Message;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EasySaveServer.Commandes;

public class CMDSetConfigFile : CMD
{
    private readonly string[]? _buisnessApp;
    private readonly string[]? _cryptExtension;
    private readonly string _cryptoKey;
    private readonly string _language; //fr || en
    private readonly string _logPath;
    private readonly string _logType; //json || xml

    public CMDSetConfigFile(string logPath, string cryptoKey, string language, string logType, string[]? cryptExtension,
        string[]? buisnessApp) : base("SetConfigFile")
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
        var json = new JObject();
        json.Add("commande", commande);
        json.Add("logPath", _logPath);
        json.Add("cryptoKey", _cryptoKey);
        json.Add("language", _language);
        json.Add("logType", _logType);
        json["cryptExtension"] = JToken.FromObject(_cryptExtension);
        json["buisnessApp"] = JToken.FromObject(_buisnessApp);
        var jsonString = JsonConvert.SerializeObject(json);
        return jsonString;
    }

    public override Task run(MessageList messageList)
    {
        try
        {
            Console.WriteLine("CMDGetConfig.run");
            var config = ConfigSingleton.Instance();
            config.SetLogPath(_logPath);
            config.SetCryptKey(_cryptoKey);
            config.SetLanguage(_language);
            config.SetLogType(_logType);
            config.SetCryptExtension(_cryptExtension);
            config.SetBuisnessApp(_buisnessApp);
            var msgConfigFile = new MSGConfigFile(config.ConfigFile);
            messageList.Messages.Add(msgConfigFile.toString());
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.StackTrace);
        }

        return Task.CompletedTask;
    }
}