using System;
using System.Collections.Generic;
using System.Diagnostics;

using Config.i18n;
using Config;
using Job.Config;
// using Job.Config.i18n;
using Logger;

namespace CLI;


    public class CommandDeleteSaveJob : Commande
    {
        public CommandDeleteSaveJob() : base("delete-SaveJob", new string[] { "delete-save-job", "delete-save-jobs", "delete-jobs", "delete-savejobs", "delete-job", "delete-savejob" }) { }

        public override Task Action(string[] args)
        {
            string content = args[0];
        
            string separator;   
            if (content.Contains(";")) {
                separator = ";";
            } else if (content.Contains(",")) {
                separator = ",";
            } else {
                separator = "";
            }

            string[] contentSplited = content.Split(separator);
            List<int> ids = contentSplited.Select(int.Parse).ToList();
        
            (int returnCode, string message) = Job.Controller.DeleteSaveJob.Execute(ids, separator);
            
            Console.WriteLine(message);
            
            return Task.CompletedTask;
        }
    }
