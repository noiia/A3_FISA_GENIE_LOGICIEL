using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Client.Commandes;

public class CMDDeleteSaveJob : CMD
{
    private int _Id;
    
    public CMDDeleteSaveJob(int id) : base("DeleteSaveJob")
    {
        _Id = id;
    }

    public override string toString()
    {
        JObject json = new JObject();
        json.Add("commande", base.name);
        json.Add("id", _Id);
        string jsonString = JsonConvert.SerializeObject(json);
        return jsonString;
    }
}