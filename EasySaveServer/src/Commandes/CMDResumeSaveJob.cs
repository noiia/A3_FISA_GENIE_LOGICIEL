using EasySaveServer;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Client.Commandes;

public class CMDResumeSaveJob:CMD
{
    private int _Id;
    
    public CMDResumeSaveJob(int id) : base("ResumeSaveJob")
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
            //Pause SaveJob
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