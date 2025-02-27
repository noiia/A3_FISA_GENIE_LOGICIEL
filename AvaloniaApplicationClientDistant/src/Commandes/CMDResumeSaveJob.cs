using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AvaloniaApplicationClientDistant.Commandes;

public class CMDResumeSaveJob : CMD
{
    private readonly int _Id;

    public CMDResumeSaveJob(int id) : base("ResumeSaveJob")
    {
        _Id = id;
    }

    public override string toString()
    {
        var json = new JObject();
        json.Add("commande", Command);
        json.Add("id", _Id);
        var jsonString = JsonConvert.SerializeObject(json);
        return jsonString;
    }
}