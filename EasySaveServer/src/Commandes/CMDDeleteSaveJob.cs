using EasySaveServer.Message;
using Job.Controller;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EasySaveServer.Commandes;

public class CMDDeleteSaveJob : CMD
{
    public CMDDeleteSaveJob(List<int> ids) : base("DeleteSaveJob")
    {
        Ids = ids;
    }

    public List<int> Ids { get; set; }

    public override string toString()
    {
        var json = new JObject();
        json.Add("commande", commande);
        json["ids"] = JToken.FromObject(Ids);
        var jsonString = JsonConvert.SerializeObject(json);
        return jsonString;
    }

    public override Task run(MessageList messageList)
    {
        try
        {
            Console.WriteLine("CMDDeleteSaveJob.run");
            //(int returnCode, string message) = SaveJobRepo.DeleteSaveJob(_Id);
            var separator = Ids.Count >= 1 ? ";" : "";
            DeleteSaveJob.Execute(Ids, separator);
            var config = ConfigSingleton.Instance();
            var msgConfigFile = new MSGConfigFile(config.ConfigFile);
            messageList.Messages.Add(msgConfigFile.toString());
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.StackTrace);
        }

        return Task.CompletedTask;
    }
}