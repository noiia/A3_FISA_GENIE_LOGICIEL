using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// using Job.Config.i18n;
using Job.Config;
using Job.Config.i18n;

namespace CLI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            CLI cli = new CLI();
            cli.Run(args);
            // Console.WriteLine(Translation.Translator.GetString("NoSjToPrint"));
        }
    }
}
