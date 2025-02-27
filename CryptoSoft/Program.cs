namespace CryptoSoft;

internal class Program
{
    private static int Main(string[] args)
    {
        if (args.Length < 4)
        {
            Console.WriteLine("Usage: CryptoSoft.exe <Crypt | DeCrypt> <inputFile> <outputFile> <key>");
            return CryptoSoft.code_bad_args;
        }

        if (!(args[0].ToLower() == "crypt" || args[0].ToLower() == "decrypt"))
        {
            Console.WriteLine("Usage: CryptoSoft.exe <Crypt | DeCrypt> <inputFile> <outputFile> <key>");
            return CryptoSoft.code_bad_args;
        }

        var cryptoSoft = new CryptoSoft(args[1], args[2], args[3]);
        if (args[0].ToLower() == "crypt") return cryptoSoft.Crypt();

        return cryptoSoft.Decrypt();
    }
}