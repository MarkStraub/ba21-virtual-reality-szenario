using UnityEngine;

/// <summary>
/// Summary description for Language Manager
/// </summary>
public class LanguageManager
{
    private string languageCode;
    private readonly SettingsManager settingsManager;
    private Language language;
    private Language defaultLanguage;

    public LanguageManager() : this(new SettingsManager()) { }

    public LanguageManager(SettingsManager settingsManager)
    {
        this.settingsManager = settingsManager;
        this.languageCode = this.settingsManager.GetStringValueByName("language");
        this.LoadLanguage();
        this.LoadDefaultLanguage();
    }

    public void ReloadSettings()
    {
        //this.settingsManager.LoadSettings();
        this.languageCode = this.settingsManager.GetStringValueByName("language");
        this.LoadLanguage();
    }

    private void LoadLanguage()
    {
        var jsonTextAsset = Resources.Load<TextAsset>("Resources." + this.languageCode);
        this.language = JsonUtility.FromJson<Language>(jsonTextAsset.ToString());
    }

    private void LoadDefaultLanguage()
    {
        var jsonTextAsset = Resources.Load<TextAsset>("Resources.en_US");
        this.defaultLanguage = JsonUtility.FromJson<Language>(jsonTextAsset.ToString());
    }

    public string GetValue(string name)
    {

        try
        {
            var value = this.language.GetType().GetField(name).GetValue(this.language).ToString();
            return value;
        }
        catch (System.Exception)
        {
            // Ignore => Try to get the value from the default language
        }

        // Check if a value has been found. If not return the value of the default Language (en_US)
        try
        {
            var value = this.defaultLanguage.GetType().GetField(name).GetValue(this.defaultLanguage).ToString();
            return value;
        }
        catch (System.Exception)
        {
            // If the field could not be found we return null
            return null;
        }
    }
}
