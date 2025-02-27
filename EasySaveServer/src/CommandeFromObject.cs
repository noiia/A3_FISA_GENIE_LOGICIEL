using Newtonsoft.Json;
using CMD = EasySaveServer.Commandes.CMD;
using CMDAddSaveJob = EasySaveServer.Commandes.CMDAddSaveJob;
using CMDDeleteSaveJob = EasySaveServer.Commandes.CMDDeleteSaveJob;
using CMDExecSaveJobs = EasySaveServer.Commandes.CMDExecSaveJobs;
using CMDGetConfig = EasySaveServer.Commandes.CMDGetConfig;
using CMDPauseSaveJob = EasySaveServer.Commandes.CMDPauseSaveJob;
using CMDResumeSaveJob = EasySaveServer.Commandes.CMDResumeSaveJob;
using CMDSetConfigFile = EasySaveServer.Commandes.CMDSetConfigFile;

namespace EasySaveServer;

public class CommandeFromObject
{
    private readonly string asciiMessage;
    private readonly MessageList messageList;
    private string commande;

    public CommandeFromObject(string asciiMessage, MessageList messageList)
    {
        this.asciiMessage = asciiMessage;
        this.messageList = messageList;
    }

    public Task run()
    {
        var cmd = JsonConvert.DeserializeObject<CMD>(asciiMessage);
        Console.WriteLine(cmd.Commande);
        switch (cmd.Commande)
        {
            case "AddSaveJob":
                var cmdAddSaveJob = JsonConvert.DeserializeObject<CMDAddSaveJob>(asciiMessage);
                cmdAddSaveJob.run(messageList);
                break;
            case "DeleteSaveJob":
                var cmdDeleteSaveJob = JsonConvert.DeserializeObject<CMDDeleteSaveJob>(asciiMessage);
                cmdDeleteSaveJob.run(messageList);
                break;
            case "ExecSaveJobs":
                var cmdExecSaveJobs = JsonConvert.DeserializeObject<CMDExecSaveJobs>(asciiMessage);
                cmdExecSaveJobs.run(messageList);
                break;
            case "GetConfig":
                var cmdGetConfig = JsonConvert.DeserializeObject<CMDGetConfig>(asciiMessage);
                cmdGetConfig.run(messageList);
                break;
            case "SetConfigFile":
                var cmdSetConfigFile = JsonConvert.DeserializeObject<CMDSetConfigFile>(asciiMessage);
                break;
            case "PauseSaveJob":
                var cmdPauseSaveJob = JsonConvert.DeserializeObject<CMDPauseSaveJob>(asciiMessage);
                break;
            case "ResumeSaveJob":
                var cmdResumeSaveJob = JsonConvert.DeserializeObject<CMDResumeSaveJob>(asciiMessage);
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
        return Task.CompletedTask;
    }
}