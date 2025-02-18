﻿using System;
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

        public override void Action(string[] args)
        {
            Job.Controller.DeleteSaveJob.Execute(args);
        }
    }
