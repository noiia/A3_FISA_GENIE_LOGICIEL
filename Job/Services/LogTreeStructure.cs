using System.Security.Cryptography;
using System.Text.Json;
using Job.Model;
using FileInfo = Job.Model.FileInfo;

namespace Job.Services;

public class LogTreeStructure
{
    public static void WriteFile(string sourceDir, string outputDir)
    {
        var directoryTree = GetDirectoryAttribute(sourceDir);
        string json = JsonSerializer.Serialize(directoryTree, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(Path.Combine(outputDir, ".structure.json"), json);
    }

    static DirAttribute GetDirectoryAttribute(string path)
    {
        var files = new List<FileInfo>();
        foreach (var filePath in Directory.GetFiles(path))
        {
            files.Add(new FileInfo(Path.GetFileName(filePath), ComputeFileHash(filePath)));
        }

        return new DirAttribute
        {
            DirName = Path.GetFileName(path),
            Files = files,
            SubFolder = new List<DirAttribute>(
                Array.ConvertAll(Directory.GetDirectories(path), GetDirectoryAttribute)
            )
        };
    }

    static string ComputeFileHash(string filePath)
    {
        using (var sha256 = SHA256.Create())
        {
            using (var stream = File.OpenRead(filePath))
            {
                var hash = sha256.ComputeHash(stream);
                return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
            }
        }
    }
}