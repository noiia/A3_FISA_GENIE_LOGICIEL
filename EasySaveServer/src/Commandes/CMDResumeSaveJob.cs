using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EasySaveServer.Commandes;

public class CMDResumeSaveJob : CMD
{
    public CMDResumeSaveJob(int id) : base("ResumeSaveJob")
    {
        Id = id;
    }

    public int Id { get; set; }

    public override string toString()
    {
        var json = new JObject();
        json.Add("commande", commande);
        json.Add("id", Id);
        var jsonString = JsonConvert.SerializeObject(json);
        return jsonString;
    }

    public override Task run(MessageList messageList)
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

        return Task.CompletedTask;
    }
}