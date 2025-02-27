using EasySaveServer;
using EasySaveServer.Message;
using Job.Config;
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
        json.Add("commande", base.commande);
        json.Add("id", _Id);
        string jsonString = JsonConvert.SerializeObject(json);
        return jsonString;
    }
    
    public override void run(MessageList messageList)
    {
        try
        {
            Console.WriteLine("CMDDeleteSaveJob.run");
            //Delete saveJob
            Configuration config = ConfigSingleton.Instance();
            MSGConfigFile msgConfigFile = new MSGConfigFile(config.ConfigFile);
            messageList.Messages.Add(msgConfigFile.toString());
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.StackTrace);
        }

        return;
    }

    public int Id
    {
        get => _Id;
        set => _Id = value;
    }
}