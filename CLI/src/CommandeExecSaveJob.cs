using Job.Controller;
// using Job.Config.i18n;


namespace CLI;

public class CommandeExecSaveJob : Commande
{
    //TODO : ajouté le temps du fichier delta du cp 
    public CommandeExecSaveJob() : base("Exec-SaveJob", new[] { "execsj", "e-sj" })
    {
    }

    public override async Task Action(string[] args)
    {
        var content = args[0];

        string separator;
        if (content.Contains(";"))
            separator = ";";
        else if (content.Contains(","))
            separator = ",";
        else
            separator = "";

        string[] contentSplited = content.Split(separator);
        var ids = contentSplited.Select(int.Parse).ToList();

        var executionTracker = new ExecutionTracker();
        var lockTracker = new LockTracker();

        var (returnCode, message) = await ExecuteSaveJob.Execute(ids, separator, executionTracker, lockTracker);

        Console.WriteLine(message);
    }
}