using EasySaveServer;
using EasySaveServer.Message;
using Job.Config;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Client.Commandes;

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
        json.Add("commande", base.commande);
        json["ids"] = JToken.FromObject(_Ids);
        string jsonString = JsonConvert.SerializeObject(json);
        return jsonString;
    }   
    public override void run(MessageList messageList)
    {
        try
        {
            Console.WriteLine("CMDExecSaveJobs.run");
            //ExecSaveJob
            
            
            // Send data
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.StackTrace);
        }

        return;
    }

    public List<int> Ids
    {
        get => _Ids;
        set => _Ids = value ?? throw new ArgumentNullException(nameof(value));
    }
}