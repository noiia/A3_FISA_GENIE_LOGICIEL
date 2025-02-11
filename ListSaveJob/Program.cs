using Config;
using ListSaveJobs;

namespace ListSaveJobs
{
    public class Program
    {
        // ListSaveJob
        public static void Main(string[] args)
        {
            Configuration config = new Configuration(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\EasySave\\" + "config.json");
            config.GetSaveJob(args[0]);
            ServiceListSaveJob.Run(args, config);
        }
    }
}