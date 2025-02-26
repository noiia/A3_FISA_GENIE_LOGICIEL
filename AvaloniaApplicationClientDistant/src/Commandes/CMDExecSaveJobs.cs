using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AvaloniaApplicationClientDistant.Commandes;

public class CMDExecSaveJobs: CMD
{
    private List<int> _Ids;
    
    public CMDExecSaveJobs(List<int> ids) : base("ExecSaveJobs")
    {
        _Ids = ids;
    }

    public override string toString()
    {
        JObject json = new JObject();
        json.Add("commande", base.Command);
        json["ids"] = JToken.FromObject(_Ids);
        string jsonString = JsonConvert.SerializeObject(json);
        return jsonString;
    }   
}