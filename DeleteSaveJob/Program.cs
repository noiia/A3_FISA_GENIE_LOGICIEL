﻿using Config;

namespace DeleteSaveJob
{
    public class Program
    {
        // DeleteSaveJob <id_savejob> <args>
        public static void Main(string[] args)
        {
            Configuration config = new Configuration(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\EasySave\\" + "config.json");
            config.GetSaveJob(args[0]);
            ServiceDeleteSaveJob.Run(args, config);
        }
    }
}