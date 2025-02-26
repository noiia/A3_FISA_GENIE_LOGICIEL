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
        JObject json = new JObject();
        json.Add("commande", base.Command);
        string jsonString = JsonConvert.SerializeObject(json);
        return jsonString;
    }
}