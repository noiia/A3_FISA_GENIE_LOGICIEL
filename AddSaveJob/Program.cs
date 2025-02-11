using Config;

namespace AddSaveJob
{
    public class Program
    {
        // AddSaveJob <name> <source> <destination> <type>
        public static int Main(string[] args)
        {
            Configuration config = new Configuration(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\EasySave\\" + "config.json");
            config.LoadConfiguration();
            return ServiceAddSaveJob.Run(args, config);
        }
    }
}