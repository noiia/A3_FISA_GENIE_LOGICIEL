using EasySaveServer;
using EasySaveServer.Message;
using Job.Config;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Client.Commandes;

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
        json.Add("commande", base.commande);
        json.Add("logPath", _logPath);
        json.Add("cryptoKey", _cryptoKey);
        json.Add("language", _language);
        json.Add("logType", _logType);
        json["cryptExtension"] = JToken.FromObject(_cryptExtension);
        json["buisnessApp"] = JToken.FromObject(_buisnessApp);
        string jsonString = JsonConvert.SerializeObject(json);
        return jsonString;
    }  
    
    public override void run(MessageList messageList)
    {
        try
        {
            Console.WriteLine("CMDGetConfig.run");
            Configuration config = ConfigSingleton.Instance();
            config.SetLogPath(this._logPath);
            config.SetCryptKey(this._cryptoKey);
            config.SetLanguage(this._language);
            config.SetLogType(this._logType);
            config.SetCryptExtension(this._cryptExtension);
            config.SetBuisnessApp(this._buisnessApp);
            MSGConfigFile msgConfigFile = new MSGConfigFile(config.ConfigFile);
            messageList.Messages.Add(msgConfigFile.toString());
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.StackTrace);
        }

        return;
    }
}