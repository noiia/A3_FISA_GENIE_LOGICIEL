namespace CLI.i18n;

public class en
{
    public static List<string> EnTranslation()
    {
        List<string> enTranslate = new List<string>();
        // CommandeAddSaveJob
        enTranslate.Add("is call with args : ");
        enTranslate.Add("Save Job has been created");
        enTranslate.Add("Bad arguments, require Add-Savejob <name> <source> <destination> <type>");
        enTranslate.Add("\t A save Job already exist with this name");
        enTranslate.Add("\t Source directory does not exist");
        enTranslate.Add("\t Destination directory does not exist");
        enTranslate.Add("\t Invalid type");
        enTranslate.Add("real type are full | diff");
        
        // CommandeDeleteSaveJob
        enTranslate.Add("Delete-SaveJob : is call with args :");
        enTranslate.Add("The id should be a number ");
        enTranslate.Add("\t Save Job has been deleted");
        enTranslate.Add("\t Bad arguments, require Delete-SaveJob <id>");
        enTranslate.Add("\t Save Job does not exist (");
        enTranslate.Add("Exec-SaveJob : is call with args :");
        
        // CommandeExecSaveJob
        enTranslate.Add("\t Save Job done successfully");
        enTranslate.Add("\t Bad arguments, require Exec-Savejob <id>|<name> or <id>;<id>;<id> or <id>,<id> ");
        enTranslate.Add("\t This job does not exist ");
        
        // CommandeListJobs
        enTranslate.Add("List-SaveJob : is call with args : ");
        enTranslate.Add("There is no SaveJob to print");
        enTranslate.Add("\tName :");
        enTranslate.Add("\t\tId : ");
        enTranslate.Add("\t\tLast save :");
        enTranslate.Add("\t\tSource :");
        enTranslate.Add("\t\tDestination :");
        enTranslate.Add("\t\tCreated : ");
        
        // CommandeSetLogPathcs
        enTranslate.Add("Set-LogPath : is call with args : ");
        enTranslate.Add("\tLogPath has been updated ");
        enTranslate.Add("is not a valid path");
        enTranslate.Add("\t Bad arguments, require Set-LogPath <logPath>");

        // CommandeSetLanguage
        enTranslate.Add("Language changed to : ");
        enTranslate.Add(" this language is not supported, require en or fr");
        enTranslate.Add("Bad arguments, require en or fr : ");
        
        return enTranslate;
    }
}