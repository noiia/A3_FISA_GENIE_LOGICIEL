using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AvaloniaApplicationClientDistant.Commandes;

public class CMDExecSaveJobs : CMD
{
    private readonly List<int> _Ids;

    public CMDExecSaveJobs(List<int> ids) : base("ExecSaveJobs")
    {
        _Ids = ids;
    }

    public override string toString()
    {
        var json = new JObject();
        json.Add("commande", Command);
        json["ids"] = JToken.FromObject(_Ids);
        var jsonString = JsonConvert.SerializeObject(json);
        return jsonString;
    }
}