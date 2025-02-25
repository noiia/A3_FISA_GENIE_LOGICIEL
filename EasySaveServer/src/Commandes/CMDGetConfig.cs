using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Client.Commandes;

public class CMDGetConfig : CMD
{
    public CMDGetConfig() : base("GetConfig")
    {
        
    }
    
    public override string toString()
    {
        JObject json = new JObject();
        json.Add("commande", base.name);
        string jsonString = JsonConvert.SerializeObject(json);
        return jsonString;
    }
}