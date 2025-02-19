using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace CryptoSoft
{
    internal class Program
    {
        static int Main(string[] args)
        {
            if (args.Length < 4)
            {
                Console.WriteLine("Usage: CryptoSoft.exe <Crypt | DeCrypt> <inputFile> <outputFile> <key>");
                return CryptoSoft.code_bad_args;
            }

            if (!((args[0].ToLower() == "crypt")||(args[0].ToLower() == "decrypt")))
            {
                Console.WriteLine("Usage: CryptoSoft.exe <Crypt | DeCrypt> <inputFile> <outputFile> <key>");
                return CryptoSoft.code_bad_args;
            }
            CryptoSoft cryptoSoft = new CryptoSoft(args[1], args[2], args[3]);
            if (args[0].ToLower() == "crypt")
            {
                return cryptoSoft.Crypt();
            }
            else
            {
                return cryptoSoft.Decrypt();
            }
        }
    }
}