using EasySaveServer.Message;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EasySaveServer.Commandes;

public class CMDGetConfig : CMD
{
    public CMDGetConfig() : base("GetConfig")
    {
    }

    public override string toString()
    {
        var json = new JObject();
        json.Add("commande", commande);
        var jsonString = JsonConvert.SerializeObject(json);
        return jsonString;
    }

    public override Task run(MessageList messageList)
    {
        try
        {
            Console.WriteLine("CMDGetConfig.run");
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