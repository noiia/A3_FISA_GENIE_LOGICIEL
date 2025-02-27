using System;
using Config;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AvaloniaApplicationClientDistant.Message;

public class MSGConfigFile: MSG
{
    private ConfigFile _configFile;

    public MSGConfigFile(ConfigFile configFile) : base("configFile")
    {
        _configFile = configFile;
    }
    
    public override string toString()
    {
        JObject json = new JObject();
        json.Add("message", message);
        json["configFile"] = JToken.FromObject(_configFile);
        string jsonString = JsonConvert.SerializeObject(json);
        return jsonString;
    }


    public ConfigFile ConfigFile
    {
        get => _configFile;
        set => _configFile = value ?? throw new ArgumentNullException(nameof(value));
    }
}