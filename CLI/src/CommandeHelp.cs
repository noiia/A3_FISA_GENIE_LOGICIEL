﻿using System;

namespace CLI
{
    public class CommandeHelp : Commande
    {
        public CommandeHelp() : base("help", new string[] { "h", "?", "Help" }) { }

        public override void Action(string[] args)
        {
            Console.WriteLine("Help : ");
            Console.WriteLine("\tlHelp");
            Console.WriteLine("\tList-Jobs");
            Console.WriteLine("\tAdd-SaveJob <name> <source> <destination>");
            Console.WriteLine("\tDelete-SaveJob <id>|<name>");
            Console.WriteLine("\tExec-SaveJob <id>|<name>");
            Console.WriteLine("\tSet-LogPath <path>");
        }

    }
}