using Config;

namespace ServiceLogTreeStructure
{
    public class Program
    {
        // DeleteSaveJob <id_savejob> <args>
        public static void Main(string[] args)
        {
            ServiceLogTreeStructure.WriteFile(args[0], args[1]);
        }
    }
}