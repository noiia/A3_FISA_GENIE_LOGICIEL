﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySaveServer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Client.Client client = new Client.Client();
            client.Run(args);
        }
    }
}