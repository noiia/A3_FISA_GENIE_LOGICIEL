using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text.Json;

namespace ServiceLogTreeStructure
{
    // Ajout d'une méthode pour stocker les infos dans des variables
    public class DirAttribute
    {
        // Création d'une classe pour stocker le nom du fichier
        public string DirName { get; set; }
        public List<FileInfo> Files { get; set; }
        public List<DirAttribute> SubFolder { get; set; }
    }

    public class FileInfo
    {
        public string FileName { get; set; }
        public string FileHash { get; set; }
    }

    public class ServiceLogTreeStructure
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
                files.Add(new FileInfo
                {
                    FileName = Path.GetFileName(filePath),
                    FileHash = ComputeFileHash(filePath)
                });
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
}