using System;
using System.Text.Json;

namespace Services
{
    // Ajout d'une méthode pour stocker les infos dans des variables
    public class DirAttribute
    {
        // Création d'une classe pour stocker le nom du fichier
        public string DirName { get; set; }
        public List<string> Files { get; set; }
        public List<DirAttribute> SubFolder { get; set; }
    }
    
    public class ServiceLogTreeStructure
    {
        public static void WriteFile(string sourceDir, string outputDir)
        {
            var directoryTree = GetDirectoryAttribute(sourceDir);

            string json = JsonSerializer.Serialize(directoryTree, new JsonSerializerOptions { WriteIndented = true });

            File.WriteAllText((outputDir + "/.structure.json"), json);
        }
        
        
        static DirAttribute GetDirectoryAttribute(string path)
        {
            return new DirAttribute
            {
                DirName = Path.GetFileName(path),
                Files = new List<string>(Directory.GetFiles(path)),
                SubFolder = new List<DirAttribute>(
                    Array.ConvertAll(Directory.GetDirectories(path), GetDirectoryAttribute)
                )
            };
        }
    }
}
