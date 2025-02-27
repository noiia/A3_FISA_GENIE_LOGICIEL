using EasySaveServer.Message;
using Job.Config;
using Job.Controller;
using Job.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EasySaveServer.Commandes;

public class CMDDeleteSaveJob : CMD
{
    private List<int> _Ids;
    
    public CMDDeleteSaveJob(List<int> ids) : base("DeleteSaveJob")
    {
        _Ids = ids;
    }

    public override string toString()
    {
        JObject json = new JObject();
        json.Add("commande", base.commande);
        json["ids"] = JToken.FromObject(Ids);
        string jsonString = JsonConvert.SerializeObject(json);
        return jsonString;
    }
    
    public override Task run(MessageList messageList)
    {
        try
        {
            Console.WriteLine("CMDDeleteSaveJob.run");
            //(int returnCode, string message) = SaveJobRepo.DeleteSaveJob(_Id);
            string separator = Ids.Count >= 1 ? ";" : "";
            DeleteSaveJob.Execute(Ids, separator);
            Configuration config = ConfigSingleton.Instance();
            MSGConfigFile msgConfigFile = new MSGConfigFile(config.ConfigFile);
            messageList.Messages.Add(msgConfigFile.toString());
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.StackTrace);
        }

        return Task.CompletedTask;
    }

    public List<int> Ids
    {
        get => _Ids;
        set => _Ids = value;
    }
}