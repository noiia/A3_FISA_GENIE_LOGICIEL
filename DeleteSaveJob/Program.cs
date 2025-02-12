using Config;

namespace DeleteSaveJob
{
    public class Program
    {
        // DeleteSaveJob <id_savejob> <args>
        public static void Main(string[] args)
        {
            Configuration config = new Configuration(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\EasySave\\" + "config.json");
            // config.GetSaveJob(args[0]);
            //#TODO faire en sorte que ça ne fonctionne pas qu'avec les id mais aussi les noms
             return ServiceDeleteSaveJob.Run(args, config);
        }
    }
}