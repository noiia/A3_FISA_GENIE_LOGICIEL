using Job.Controller;
// using Job.Config.i18n;

namespace CLI;

public class CommandDeleteSaveJob : Commande
{
    public CommandDeleteSaveJob() : base("delete-SaveJob",
        new[]
        {
            "delete-save-job", "delete-save-jobs", "delete-jobs", "delete-savejobs", "delete-job", "delete-savejob"
        })
    {
    }

    public override Task Action(string[] args)
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

        var (returnCode, message) = DeleteSaveJob.Execute(ids, separator);

        Console.WriteLine(message);

        return Task.CompletedTask;
    }
}