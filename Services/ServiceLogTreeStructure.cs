using System;
using System.Text.Json;

namespace Services
{
    // Ajout d'une méthode pour stocker les infos dans des variables
    public class FileAttribute
    {
        // Création d'une classe pour stocker le nom du fichier
        public required string FileName { get; set; }
        // Création d'une classe pour stocker le type de fichier
        public required string FileType { get; set; }
    }
    
    internal class ServiceLogTreeStructure
    {
        public static void WriteLine(string fileName, string filetype)
        {
            //Créer l'objet File
            var file = new FileAttribute { FileName = fileName, FileType = filetype};
            
            if (file.FileType == "File")
            {
                // Sérialiser l'objet en JSON
                var json = JsonSerializer.Serialize(file);

                // Sauvegarder dans un fichier
                File.WriteAllText("test.json", json);

                Console.WriteLine("Fichier JSON créé !");
            }
        }
    }
}
