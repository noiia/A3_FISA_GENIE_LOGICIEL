using EasySaveServer.Message;
using Job.Controller;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EasySaveServer.Commandes;

public class CMDAddSaveJob : CMD
{
    private string _Destination;

    private string _Name;
    private string _Source;
    private string _Type;

    public CMDAddSaveJob(string name, string source, string destination, string type) : base("AddSaveJob")
    {
        _Name = name;
        _Source = source;
        _Destination = destination;
        _Type = type;
    }

    public string Name
    {
        get => _Name;
        set => _Name = value ?? throw new ArgumentNullException(nameof(value));
    }

    public string Source
    {
        get => _Source;
        set => _Source = value ?? throw new ArgumentNullException(nameof(value));
    }

    public string Destination
    {
        get => _Destination;
        set => _Destination = value ?? throw new ArgumentNullException(nameof(value));
    }

    public string Type
    {
        get => _Type;
        set => _Type = value ?? throw new ArgumentNullException(nameof(value));
    }

    public override string toString()
    {
        var json = new JObject();
        json.Add("commande", commande);
        json.Add("name", _Name);
        json.Add("source", _Source);
        json.Add("destination", _Destination);
        json.Add("type", _Type);
        var jsonString = JsonConvert.SerializeObject(json);
        return jsonString;
    }

    public override Task run(MessageList messageList)
    {
        try
        {
            Console.WriteLine("CMDAddSaveJob.run");
            //(int returnCode, string message) = AddSaveJob.Execute(_Name, _Source, _Destination, _Type);
            AddSaveJob.Execute(_Name, _Source, _Destination, _Type);
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