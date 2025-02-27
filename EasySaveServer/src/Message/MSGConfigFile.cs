using Config;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EasySaveServer.Message;

public class MSGConfigFile : MSG
{
    private readonly ConfigFile _configFile;

    public MSGConfigFile(ConfigFile configFile) : base("configFile")
    {
        _configFile = configFile;
    }

    public override string toString()
    {
        var json = new JObject();
        json.Add("message", name);
        json["configFile"] = JToken.FromObject(_configFile);
        var jsonString = JsonConvert.SerializeObject(json);
        return jsonString;
    }
}