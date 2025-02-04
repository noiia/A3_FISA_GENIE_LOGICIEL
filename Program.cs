using Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EasySave
{
    internal static class Program
    {
        /// <summary>
        /// Application entry point.
        /// </summary>
        [STAThread]
        static void Main()
        {
            string CONFIGPATH = ( Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\EsaySave\\" + "config.json");
            Configuration configuration = new Configuration(CONFIGPATH);
            configuration.LoadConfiguration();
            configuration.AddSaveJob(3, "test", "test", "test", DateTime.Now, DateTime.Now);
            //LoggerUtility.WriteLog(LoggerUtility.INFO, "EasySave Application Start.");
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());
        }
    }
}