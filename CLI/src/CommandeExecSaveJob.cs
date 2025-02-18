using System;
using System.Diagnostics;
using System.IO;

using Config.i18n;
using Config;
using Job.Config;
// using Job.Config.i18n;
using Logger;


namespace CLI
{
    public class CommandeExecSaveJob : Commande
    {
        
        //TODO : ajouté le temps du fichier delta du cp 
        public CommandeExecSaveJob() : base("Exec-SaveJob", new string[] { "execsj", "e-sj" }) { }

        public override void Action(string[] args)
        {
            Job.Controller.ExecuteSaveJob.Execute(args);
        }

    }
}