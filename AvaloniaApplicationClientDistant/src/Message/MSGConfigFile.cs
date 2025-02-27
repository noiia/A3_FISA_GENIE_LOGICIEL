using System;
using Config;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AvaloniaApplicationClientDistant.Message;

public class MSGConfigFile : MSG
{
    private ConfigFile _configFile;

    public MSGConfigFile(ConfigFile configFile) : base("configFile")
    {
        _configFile = configFile;
    }


    public ConfigFile ConfigFile
    {
        get => _configFile;
        set => _configFile = value ?? throw new ArgumentNullException(nameof(value));
    }

    public override string toString()
    {
        var json = new JObject();
        json.Add("message", message);
        json["configFile"] = JToken.FromObject(_configFile);
        var jsonString = JsonConvert.SerializeObject(json);
        return jsonString;
    }
}