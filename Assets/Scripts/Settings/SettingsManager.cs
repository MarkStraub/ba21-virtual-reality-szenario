using System;
using System.IO;
using UnityEngine;

public class SettingsManager
{
    private Settings settings;
    private bool autoSave;
    private int houseLevel = 4;
    private int workbenchLevel = 3;
    public SettingsManager(bool autoSave = true)
    {
        this.autoSave = autoSave;
        LoadSettings();
    }

    public void LoadSettings()
    {
        Debug.Log("BA-HELV: -----------------------------------------------------------------------------------------------------------------------------------------------");
        Debug.Log("BA-HELV: ");
        Debug.Log("BA-HELV: Load Settings");


        // https://stackoverflow.com/questions/51599050/unity-and-oculus-go-read-write-on-the-internal-storage
#if PLATFORM_ANDROID && !UNITY_EDITOR
        Debug.Log("BA-HELV: PLATFORM ANDROID");
        Debug.Log($"PATH{Application.persistentDataPath}/settings.json");
        Debug.Log("BA-HELV: ");
        if (File.Exists($"{Application.persistentDataPath}/settings.json"))
        {
            Debug.Log("BA-HELV: File exists");
            var jsonTextAsset = new TextAsset(System.IO.File.ReadAllText($"{Application.persistentDataPath}/settings.json"));
            settings = JsonUtility.FromJson<Settings>(jsonTextAsset.ToString());

            // Check if the settings have the same version. If not we take the settings from the Settins.cs which are newer
            var set = new Settings();
            if(settings.version != set.version)
            {
                settings = set;
            }
        }
        else
        {
            Debug.Log("BA-HELV: File does not exists");
            settings = new Settings();
        }

#elif UNITY_EDITOR
        Debug.Log("BA-HELV: UNITY EDITOR");
        if (File.Exists("Assets/Resources/settings.json"))
        {
            Debug.Log("BA-HELV: File exists");
            var jsonTextAsset = Resources.Load<TextAsset>("Settings");
            settings = JsonUtility.FromJson<Settings>(jsonTextAsset.ToString());

            // Check if the settings have the same version. If not we take the settings from the Settins.cs which are newer
            var set = new Settings();
            if (settings.version != set.version)
            {
                settings = set;
            }
        }
        else
        {
            Debug.Log("BA-HELV: File does not exists");
            settings = new Settings();
        }
#endif

        Debug.Log("BA-HELV: ");
        Debug.Log("BA-HELV: -----------------------------------------------------------------------------------------------------------------------------------------------");
    }

    public string GetStringValueByName(string name)
    {
        return settings.GetType().GetField(name).GetValue(settings).ToString();
    }

    public int GetIntValueByName(string name)
    {
        return Int32.Parse(settings.GetType().GetField(name).GetValue(settings).ToString());
    }

    public bool GetBooleanValueByName(string name)
    {
        return bool.Parse(settings.GetType().GetField(name).GetValue(settings).ToString());
    }

    public void SetValueByName(string name, string value)
    {
        settings.GetType().GetField(name).SetValue(settings, value);
        this.SaveSettings();
    }

    public void SetValueByName(string name, int value)
    {
        settings.GetType().GetField(name).SetValue(settings, value);
        this.SaveSettings();
    }

    public void SetValueByName(string name, bool value)
    {
        settings.GetType().GetField(name).SetValue(settings, value);
        this.SaveSettings();
    }

    public void SetLevelPassed(int level)
    {
        Debug.Log("Level: " + level);
        var sceneShortName = "th_";
        var sceneName = "theory";
        if (level >= houseLevel)
        {
            sceneShortName = $"ho_{((level + 1) % houseLevel)}";
            sceneName = "house";
        }
        else if (level >= workbenchLevel)
        {
            sceneShortName = $"wo_{((level + 1) % workbenchLevel)}";
            sceneName = "workbench";
        }
        else
        {
            sceneShortName = $"th_{level + 1}";
            sceneName = "theory";
        }
        SetValueByName(sceneShortName, true);
        SetValueByName("scene_short_name", sceneShortName);
        SetValueByName("scene_name", sceneName);
        SetValueByName("level", level + 1);
    }

    public string getNextLevelName()
    {
        int level = this.GetIntValueByName("level");
        if (level >= houseLevel)
        {
            return "house";
        }
        else if (level >= workbenchLevel)
        {
            return "workbench";
        }
        else
        {
            return "theory";
        }
    }

    public void resetToActualLevel(int level = 1)
    {
        for(int i = 1; i < level; i++)
        {
            this.SetLevelPassed(i);
        }
    }

    public void SaveSettings()
    {

        if (!autoSave)
        {
            return;
        }

        Debug.Log("BA-HELV: -----------------------------------------------------------------------------------------------------------------------------------------------");
        Debug.Log("BA-HELV: ");
        Debug.Log("BA-HELV: SAVE SETTINGS");
        Debug.Log("BA-HELV: ");

#if PLATFORM_ANDROID && !UNITY_EDITOR
        Debug.Log("BA-HELV: PLATFORM ANDROID");
        string path = $"{Application.persistentDataPath}/settings.json";
#elif UNITY_EDITOR
        Debug.Log("BA-HELV: UNITY EDITOR");
        string path = "Assets/Resources/settings.json";
#endif

        Debug.Log($"PATH: {path}");

        string jsonString = JsonUtility.ToJson(settings);

        try
        {
            System.IO.File.WriteAllText(path, jsonString);
        }
        catch (System.Exception e)
        {
            Debug.LogError("BA-HELV: ERROR EXCEPTION");
            Debug.LogException(e);
        }
        finally
        {
            Debug.Log("BA-HELV: ");
            Debug.Log("BA-HELV: -----------------------------------------------------------------------------------------------------------------------------------------------");
        }
    }
}
