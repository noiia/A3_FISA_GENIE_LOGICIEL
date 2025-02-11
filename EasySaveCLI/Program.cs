// See https://aka.ms/new-console-template for more information

using System.Diagnostics;

namespace EasySaveCLI
{
    internal static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Process proc = new Process();
            proc.StartInfo.FileName = @"../CLI/bin/Debug/net8.0/CLI.exe";
            proc.StartInfo.Arguments = "new";
            Console.WriteLine(proc.StartInfo.Arguments);
            proc.Start();
        }
    }
}