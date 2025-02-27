using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AvaloniaApplicationClientDistant.Commandes;

public class CMDGetConfig : CMD
{
    public CMDGetConfig() : base("GetConfig")
    {
    }

    public override string toString()
    {
        var json = new JObject();
        json.Add("commande", Command);
        var jsonString = JsonConvert.SerializeObject(json);
        return jsonString;
    }
}