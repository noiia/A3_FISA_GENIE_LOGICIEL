﻿using Client.Commandes;
using Newtonsoft.Json;

namespace EasySaveServer;

public class CommandeFromObject
{
    private string asciiMessage;
    private string commande;
    private MessageList messageList;

    public CommandeFromObject(string asciiMessage, MessageList messageList)
    {
        this.asciiMessage = asciiMessage;
        this.messageList = messageList;
    }

    public void run()
    {
        CMD cmd = JsonConvert.DeserializeObject<CMD>(asciiMessage);
        Console.WriteLine(cmd.Commande);
        switch (cmd.Commande)
        {
            case "AddSaveJob":
                CMDAddSaveJob cmdAddSaveJob = JsonConvert.DeserializeObject<CMDAddSaveJob>(asciiMessage);
                cmdAddSaveJob.run(messageList);
                break;
            case "DeleteSaveJob":
                CMDDeleteSaveJob cmdDeleteSaveJob = JsonConvert.DeserializeObject<CMDDeleteSaveJob>(asciiMessage);
                cmdDeleteSaveJob.run(messageList);
                break;
            case "ExecSaveJobs":
                CMDExecSaveJobs cmdExecSaveJobs = JsonConvert.DeserializeObject<CMDExecSaveJobs>(asciiMessage);
                cmdExecSaveJobs.run(messageList);
                break;
            case "GetConfig":
                CMDGetConfig cmdGetConfig = JsonConvert.DeserializeObject<CMDGetConfig>(asciiMessage);
                cmdGetConfig.run(messageList);
                break;
            case "SetConfigFile":
                CMDSetConfigFile cmdSetConfigFile = JsonConvert.DeserializeObject<CMDSetConfigFile>(asciiMessage);
                break;
            case "PauseSaveJob":
                CMDPauseSaveJob cmdPauseSaveJob = JsonConvert.DeserializeObject<CMDPauseSaveJob>(asciiMessage);
                break;
            case "ResumeSaveJob":
                CMDResumeSaveJob cmdResumeSaveJob = JsonConvert.DeserializeObject<CMDResumeSaveJob>(asciiMessage);
                break;
            default:
                Console.WriteLine($"Bad commande : {cmd.Commande}");
                break;
        }
        
        // if (message.commande)
        // {
        //     commande = message.commande;
        //     Console.WriteLine($"Commande received : {commande}");
        // }
        // else
        // {
        //     throw new Exception("No commande recived");
        // }
    }
}