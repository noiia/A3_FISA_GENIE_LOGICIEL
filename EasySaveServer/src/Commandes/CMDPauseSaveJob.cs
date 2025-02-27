﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EasySaveServer.Commandes;

public class CMDPauseSaveJob:CMD
{
    private int _Id;
    
    public CMDPauseSaveJob(int id) : base("PauseSaveJob")
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

    public int Id
    {
        get => _Id;
        set => _Id = value;
    }
}