namespace Config.i18n;

public class fr
{
    public static List<string> FrTranslation()
    {
        List<string> frTranslate = new List<string>();
        // CommandeAddSaveJob
        frTranslate.Add("est appelé avec les arguments : ");
        frTranslate.Add("Le nouveau travail de sauvegarde a été sauvegardé");
        frTranslate.Add("Mauvais arguments, Add-Savejob <nom> <source> <destination> <type>");
        frTranslate.Add("\t Un travail de sauvegarde existe déjà sous ce nom");
        frTranslate.Add("Le répertoire source n'existe pas");
        frTranslate.Add("Le répertoire destination n'existe pas");
        frTranslate.Add("Type indisponible");
        frTranslate.Add(" les types possibles sont : full, diff");
        
        // CommandeDeleteSaveJob
        frTranslate.Add("Delete-SaveJob : est appelé avec les arguments :");
        frTranslate.Add("L'id doit être un nombre ");
        frTranslate.Add("\t Le travail a été supprimé");
        frTranslate.Add("\t Mauvais arguments fournis, la commande se formule comme suivant : Delete-SaveJob <id>");
        frTranslate.Add("\t Le travail demandé n'existe pas (");
        frTranslate.Add("Exec-SaveJob : est appelé avec les arguments de commande :");
        
        // CommandeExecSaveJob
        frTranslate.Add("\t Le travail de sauvegarde s'est déroulé sans encombre");
        frTranslate.Add("\t Mauvais arguments, la commande nécessite Exec-Savejob <id>|<nom> ou <id>;<id>;<id> ou <id>,<id> ");
        frTranslate.Add("\t Ce travail n'existe pas ");
        
        // CommandeListJobs
        frTranslate.Add("List-SaveJob : est appelé avec les arguments : ");
        frTranslate.Add("Il n'y a aucun travail de sauvegarde à imprimer");
        frTranslate.Add("\tNom :");
        frTranslate.Add("\t\tId : ");
        frTranslate.Add("\t\tDernière sauvegarde :");
        frTranslate.Add("\t\tSource :");
        frTranslate.Add("\t\tDestination :");
        frTranslate.Add("\t\tCréé : ");
        
        // CommandeSetLogPathcs
        frTranslate.Add("Set-LogPath : est appelé avec les arguments : ");
        frTranslate.Add("\tLogPath a été mis à jour : ");
        frTranslate.Add(" n'est pas un chemin valide");
        frTranslate.Add("\t Mauvais arguments, la commande nécessite Set-LogPath <logPath>");
        
        // CommandeSetLanguage
        frTranslate.Add("Langue modifiée au profit de la nouvelle langue : ");
        frTranslate.Add(" n'est pas un language supporté par le système");
        frTranslate.Add("Mauvais arguments fournis : ");

        
        return frTranslate;
    }
}