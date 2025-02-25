using System.ComponentModel;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Job.Config.i18n;

public class Translation : INotifyPropertyChanged
{
    private static Translation _instance;
    public static Translation Instance => _instance ??= new Translation();

    public static ResourceManager Translator { get; private set; } =
        new("Job.Config.i18n.Resources.Resources", typeof(Translation).Assembly);

    public static event PropertyChangedEventHandler StaticPropertyChanged;
    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    
    public static void SelectLanguage(string language)
    {
        Thread.CurrentThread.CurrentUICulture = new CultureInfo(language);
        Translator = new ResourceManager("Job.Config.i18n.Resources.Resources", typeof(Translation).Assembly);

        // Met à jour toutes les propriétés
        Instance.OnPropertyChanged(nameof(SettingsTitle));
        Instance.OnPropertyChanged(nameof(SelectLanguageTitle));
        Instance.OnPropertyChanged(nameof(SelectLanguagePlaceholder));
        Instance.OnPropertyChanged(nameof(SelectLogTypeTitle));
        Instance.OnPropertyChanged(nameof(SelectLogTypePlaceholder));
        Instance.OnPropertyChanged(nameof(SetLogPathTitle));
        Instance.OnPropertyChanged(nameof(SetLogPathWatermark));
        Instance.OnPropertyChanged(nameof(BrowseButton));
        Instance.OnPropertyChanged(nameof(ResetButton));
        Instance.OnPropertyChanged(nameof(FileTypesToEncryptTitle));
        Instance.OnPropertyChanged(nameof(AddFileTypeWatermark));
        Instance.OnPropertyChanged(nameof(AddButton));
        Instance.OnPropertyChanged(nameof(RemoveSelectedButton));
        Instance.OnPropertyChanged(nameof(BusinessAppBlockingTitle));
        Instance.OnPropertyChanged(nameof(AddBusinessAppWatermark));
        Instance.OnPropertyChanged(nameof(SetEncryptionKeyTitle));
        Instance.OnPropertyChanged(nameof(EnterEncryptionKeyWatermark));
        Instance.OnPropertyChanged(nameof(UpdateKeyButton));
        Instance.OnPropertyChanged(nameof(FilePriority));
        Instance.OnPropertyChanged(nameof(AddFileExtension));
        Instance.OnPropertyChanged(nameof(NameTranslate));
        Instance.OnPropertyChanged(nameof(Browse));
        Instance.OnPropertyChanged(nameof(SrcPath));
        Instance.OnPropertyChanged(nameof(DestPath));
        Instance.OnPropertyChanged(nameof(SelectSaveType));
        Instance.OnPropertyChanged(nameof(Confirm));
        Instance.OnPropertyChanged(nameof(Execute));
        Instance.OnPropertyChanged(nameof(Delete));
        Instance.OnPropertyChanged(nameof(Select));
        Instance.OnPropertyChanged(nameof(LastExecution));
        Instance.OnPropertyChanged(nameof(Creation));
        Instance.OnPropertyChanged(nameof(Status));
        Instance.OnPropertyChanged(nameof(Type));
        Instance.OnPropertyChanged(nameof(Edit));
    }

    // Settings
    public string SettingsTitle => Translator.GetString("SettingsTitle") ?? "[SettingsTitle]";
    public string SelectLanguageTitle => Translator.GetString("SelectLanguageTitle") ?? "[SelectLanguageTitle]";
    public string SelectLanguagePlaceholder => Translator.GetString("SelectLanguagePlaceholder") ?? "[SelectLanguagePlaceholder]";
    public string SelectLogTypeTitle => Translator.GetString("SelectLogTypeTitle") ?? "[SelectLogTypeTitle]";
    public string SelectLogTypePlaceholder => Translator.GetString("SelectLogTypePlaceholder") ?? "[SelectLogTypePlaceholder]";
    public string SetLogPathTitle => Translator.GetString("SetLogPathTitle") ?? "[SetLogPathTitle]";
    public string SetLogPathWatermark => Translator.GetString("SetLogPathWatermark") ?? "[SetLogPathWatermark]";
    public string BrowseButton => Translator.GetString("BrowseButton") ?? "[BrowseButton]";
    public string ResetButton => Translator.GetString("ResetButton") ?? "[ResetButton]";
    public string FileTypesToEncryptTitle => Translator.GetString("FileTypesToEncryptTitle") ?? "[FileTypesToEncryptTitle]";
    public string AddFileTypeWatermark => Translator.GetString("AddFileTypeWatermark") ?? "[AddFileTypeWatermark]";
    public string AddButton => Translator.GetString("AddButton") ?? "[AddButton]";
    public string RemoveSelectedButton => Translator.GetString("RemoveSelectedButton") ?? "[RemoveSelectedButton]";
    public string BusinessAppBlockingTitle => Translator.GetString("BusinessAppBlockingTitle") ?? "[BusinessAppBlockingTitle]";
    public string AddBusinessAppWatermark => Translator.GetString("AddBusinessAppWatermark") ?? "[AddBusinessAppWatermark]";
    public string SetEncryptionKeyTitle => Translator.GetString("SetEncryptionKeyTitle") ?? "[SetEncryptionKeyTitle]";
    public string EnterEncryptionKeyWatermark => Translator.GetString("EnterEncryptionKeyWatermark") ?? "[EnterEncryptionKeyWatermark]";
    public string UpdateKeyButton => Translator.GetString("UpdateKeyButton") ?? "[UpdateKeyButton]";
    public string FilePriority => Translator.GetString("FilePriority") ?? "[FilePriority]";
    public string AddFileExtension => Translator.GetString("AddFileExtension") ?? "[AddFileExtension]";

    // addSaveJob
    public string NameTranslate => Translator.GetString("Name") ?? "[Name]";
    public string Browse => Translator.GetString("BrowseButton") ?? "[Browse]";
    public string SrcPath => Translator.GetString("SrcPath") ?? "[SrcPath]";
    public string DestPath => Translator.GetString("DestPath") ?? "[DestPath]";
    public string SelectSaveType => Translator.GetString("SelectSaveType") ?? "[SelectSaveType]";
    public string Confirm => Translator.GetString("Confirm") ?? "[Confirm]";
    
    // home
    public string Execute => Translator.GetString("Execute") ?? "[Execute]";
    public string Delete => Translator.GetString("Delete") ?? "[Delete]";
    public string Select => Translator.GetString("Select") ?? "[Select]";
    public string LastExecution => Translator.GetString("LastExecution") ?? "[LastExecution]";
    public string Creation => Translator.GetString("Creation") ?? "[Creation]";
    public string Status => Translator.GetString("Status") ?? "[Status]";
    public string Type => Translator.GetString("Type") ?? "[Type]";
    public string Edit => Translator.GetString("Edit") ?? "[Edit]";
    public string Progress => Translator.GetString("Progress") ?? "[Progress]";

}