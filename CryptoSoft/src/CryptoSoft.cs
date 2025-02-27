using System.Security.Cryptography;
using System.Text;

namespace CryptoSoft;

public class CryptoSoft
{
    public static int code_ok = 1;
    public static int code_bad_args = 2;
    public static int code_err_crypt = 3;
    public static int code_err_decrypt = 4;
    public static int code_err_input_notfound = 5;
    public static int code_err_output_notfound = 6;
    private readonly string inputFile;

    private readonly string key;
    private readonly string keyHash;
    private readonly string outputFile;

    public CryptoSoft(string inputFile, string outputFile, string key)
    {
        this.key = key;
        keyHash = GetSHA256Hash(this.key);
        this.inputFile = inputFile;
        if (!File.Exists(this.inputFile)) throw new FileNotFoundException("The file could not be found.");
        this.outputFile = outputFile;
        if (!DirectoryExists(this.outputFile))
            throw new FileNotFoundException("The path to the output file could not be found.");
    }

    public int Crypt()
    {
        // Lire le contenu du fichier
        var fileBytes = File.ReadAllBytes(inputFile);

        // Convertir la clé en tableau de bytes
        var keyBytes = Encoding.UTF8.GetBytes(keyHash);

        // Chiffrer les données
        for (var i = 0; i < fileBytes.Length; i++) fileBytes[i] ^= keyBytes[i % keyBytes.Length];

        // Écrire les données chiffrées dans le fichier
        File.WriteAllBytes(outputFile, fileBytes);
        return code_ok;
    }

    public int Decrypt()
    {
        // Lire le contenu du fichier
        var fileBytes = File.ReadAllBytes(inputFile);

        // Convertir la clé en tableau de bytes
        var keyBytes = Encoding.UTF8.GetBytes(keyHash);

        // Déchiffrer les données
        for (var i = 0; i < fileBytes.Length; i++) fileBytes[i] ^= keyBytes[i % keyBytes.Length];

        // Écrire les données déchiffrées dans le fichier
        File.WriteAllBytes(outputFile, fileBytes);
        return code_ok;
    }

    private string GetSHA256Hash(string input)
    {
        // Convertir la chaîne en tableau de bytes
        var bytes = Encoding.UTF8.GetBytes(input);

        // Créer un objet SHA256
        using (var sha256 = SHA256.Create())
        {
            // Calculer le hash
            var hashBytes = sha256.ComputeHash(bytes);

            // Convertir le tableau de bytes en une chaîne hexadécimale
            var builder = new StringBuilder();
            foreach (var b in hashBytes) builder.Append(b.ToString("x2"));
            return builder.ToString();
        }
    }

    private static bool DirectoryExists(string filePath)
    {
        // Extraire le chemin du répertoire à partir du chemin du fichier
        var directoryPath = Path.GetDirectoryName(filePath);

        // Vérifier si le répertoire existe
        return Directory.Exists(directoryPath);
    }
}