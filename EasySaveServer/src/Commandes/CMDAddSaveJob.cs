using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using EasySaveServer;

namespace Client.Commandes;

public class CMDAddSaveJob : CMD
{
    
    private string _Name;
    private string _Source;
    private string _Destination;
    private string _Type;

    public CMDAddSaveJob(string name, string source, string destination, string type) : base("AddSaveJob")
    {
        _Name = name;
        _Source = source;
        _Destination = destination;
        _Type = type;
    }

    public override string toString()
    {
        JObject json = new JObject();
        json.Add("commande", base.name);
        json.Add("name", _Name);
        json.Add("source", _Source);
        json.Add("destination", _Destination);
        json.Add("type", _Type);
        string jsonString = JsonConvert.SerializeObject(json);
        return jsonString;
    }
    
    public override void run(MessageList messageList)
    {
        
        return;
    }
}