using Client.Commandes;
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
        Console.WriteLine(cmd.Name);
        switch (cmd.Name)
        {
            case "AddSaveJob":
                CMDAddSaveJob cmdAddSaveJob = JsonConvert.DeserializeObject<CMDAddSaveJob>(asciiMessage);
                break;
            case "DeleteSaveJob":
                CMDDeleteSaveJob cmdDeleteSaveJob = JsonConvert.DeserializeObject<CMDDeleteSaveJob>(asciiMessage);
                break;
            case "ExecSaveJobs":
                CMDExecSaveJobs cmdExecSaveJobs = JsonConvert.DeserializeObject<CMDExecSaveJobs>(asciiMessage);
                break;
            case "GetConfig":
                CMDGetConfig cmdGetConfig = JsonConvert.DeserializeObject<CMDGetConfig>(asciiMessage);
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