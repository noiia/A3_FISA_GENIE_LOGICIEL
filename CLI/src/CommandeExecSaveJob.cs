using System;
using System.Diagnostics;
using System.IO;

using Config.i18n;
using Config;
using Job.Config;
using Job.Controller;
// using Job.Config.i18n;
using Logger;


namespace CLI
{
    public class CommandeExecSaveJob : Commande
    {
        
        //TODO : ajouté le temps du fichier delta du cp 
        public CommandeExecSaveJob() : base("Exec-SaveJob", new string[] { "execsj", "e-sj" }) { }

        public override async Task Action(string[] args)
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
            List<int>  ids = contentSplited.Select(int.Parse).ToList();
            
            ExecutionTracker executionTracker = new ExecutionTracker();
        
            (int returnCode, string message) = await Job.Controller.ExecuteSaveJob.Execute(ids, separator, executionTracker);
            
            Console.WriteLine(message);

        }

    }
}