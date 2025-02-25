using EasySaveServer;
using EasySaveServer.Message;
using Job.Config;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Client.Commandes;

public class CMDGetConfig : CMD
{
    public CMDGetConfig() : base("GetConfig")
    {
        
    }
    
    public override string toString()
    {
        JObject json = new JObject();
        json.Add("commande", base.commande);
        string jsonString = JsonConvert.SerializeObject(json);
        return jsonString;
    }
    
    public override void run(MessageList messageList)
    {
        try
        {
            Console.WriteLine("CMDGetConfig.run");
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
}