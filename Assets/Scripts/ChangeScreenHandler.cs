using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ChangeScreenHandler : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject Settings;
    public GameObject SettingsLanguage;
    public GameObject LearningUnit;
    public GameObject LearningScenario;
    public string sceneName;

    private SettingsManager settingsManager;
    private LanguageManager languageManager;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Started Change Screen");

        this.settingsManager = new SettingsManager();
        this.languageManager = new LanguageManager(settingsManager);

        InitializeText();

        sceneName = this.settingsManager.GetStringValueByName("scene_name");

        MainMenuButton();
    }

    private void InitializeText()
    {
        // Main Menu
        MainMenu.GetComponentInChildren<Text>().text = languageManager.GetValue("menu");
        var mainMenuButtons = MainMenu.GetComponentsInChildren<Button>();
        mainMenuButtons[0].GetComponentInChildren<Text>().text = languageManager.GetValue("cntnue");
        mainMenuButtons[1].GetComponentInChildren<Text>().text = languageManager.GetValue("unit");
        mainMenuButtons[2].GetComponentInChildren<Text>().text = languageManager.GetValue("settings");
        mainMenuButtons[3].GetComponentInChildren<Text>().text = languageManager.GetValue("quit");

        // Settings
        Settings.GetComponentInChildren<Text>().text = languageManager.GetValue("settings");
        var settingsButtons = Settings.GetComponentsInChildren<Button>();
        settingsButtons[0].GetComponentInChildren<Text>().text = languageManager.GetValue("chnge_language");
        settingsButtons[1].GetComponentInChildren<Text>().text = languageManager.GetValue("home");
        settingsButtons[2].GetComponentInChildren<Text>().text = languageManager.GetValue("back");

        // Settings Language
        SettingsLanguage.GetComponentInChildren<Text>().text = languageManager.GetValue("language");
        var settingsLanguageButtons = SettingsLanguage.GetComponentsInChildren<Button>();
        settingsLanguageButtons[0].GetComponentInChildren<Text>().text = languageManager.GetValue("en");
        settingsLanguageButtons[1].GetComponentInChildren<Text>().text = languageManager.GetValue("de");
        settingsLanguageButtons[2].GetComponentInChildren<Text>().text = languageManager.GetValue("sw");
        settingsLanguageButtons[3].GetComponentInChildren<Text>().text = languageManager.GetValue("home");
        settingsLanguageButtons[4].GetComponentInChildren<Text>().text = languageManager.GetValue("back");

        // Learning Unit
        LearningUnit.GetComponentInChildren<Text>().text = languageManager.GetValue("unit"); ;
        var learningUnitButtons = LearningUnit.GetComponentsInChildren<Button>();
        learningUnitButtons[0].GetComponentInChildren<Text>().text = languageManager.GetValue("theory");
        learningUnitButtons[1].GetComponentInChildren<Text>().text = languageManager.GetValue("workbench");
        learningUnitButtons[2].GetComponentInChildren<Text>().text = languageManager.GetValue("house");
        learningUnitButtons[3].GetComponentInChildren<Text>().text = languageManager.GetValue("home");
        learningUnitButtons[4].GetComponentInChildren<Text>().text = languageManager.GetValue("back");

        // Learning Scenario
        var learningScenarioButtons = LearningScenario.GetComponentsInChildren<Button>();
        learningScenarioButtons[3].GetComponentInChildren<Text>().text = languageManager.GetValue("home");
        learningScenarioButtons[4].GetComponentInChildren<Text>().text = languageManager.GetValue("back");
    }

    public void PlayNowButton()
    {

        UnityEngine.SceneManagement.SceneManager.LoadScene(this.settingsManager.GetStringValueByName("scene_name"));
    }

    public void MainMenuButton()
    {
        Debug.Log("Main Menu Button started");
        // Show Main Menu
        MainMenu.SetActive(true);

        Settings.SetActive(false);

        SettingsLanguage.SetActive(false);

        LearningUnit.SetActive(false);

        LearningScenario.SetActive(false);

        Debug.Log(sceneName);
        Debug.Log("MainMenuButton completed");
    }

    public void SettingsButton()
    {
        // Show Credits Menu
        MainMenu.SetActive(false);

        Settings.SetActive(true);

        SettingsLanguage.SetActive(false);

        LearningUnit.SetActive(false);

        LearningScenario.SetActive(false);
    }

    public void SettingsLanguageMenuButton()
    {
        MainMenu.SetActive(false);

        Settings.SetActive(false);

        SettingsLanguage.SetActive(true);

        LearningUnit.SetActive(false);

        LearningScenario.SetActive(false);
    }

    public void LearningUnitButton()
    {
        MainMenu.SetActive(false);

        Settings.SetActive(false);

        SettingsLanguage.SetActive(false);

        LearningUnit.SetActive(true);

        LearningScenario.SetActive(false);
    }

    public void LearningScenarioButton(string unit)
    {
        setLearningUnit(unit);
        ArrayList buttonList = new ArrayList();
        switch (unit)
        {
            case "theory":
                buttonList.Add("th_1");
                buttonList.Add("th_2");
                buttonList.Add("th_3");
                break;
            case "workbench":
                buttonList.Add("wo_1");
                buttonList.Add("wo_2");
                buttonList.Add("wo_3");
                break;
            case "house":
                buttonList.Add("ho_1");
                buttonList.Add("ho_2");
                buttonList.Add("ho_3");
                break;
        }
        FillLearningScenarioMenu(buttonList);

        MainMenu.SetActive(false);
        Settings.SetActive(false);
        SettingsLanguage.SetActive(false);
        LearningUnit.SetActive(false);
        LearningScenario.SetActive(true);
    }

    private void FillLearningScenarioMenu(ArrayList buttonTitles)
    {
        LearningScenario.GetComponentInChildren<Text>().text = languageManager.GetValue(this.sceneName);
        var learningScenarioButtons = LearningScenario.GetComponentsInChildren<Button>();
        for (var i = 0; i < buttonTitles.Count; i++)
        {
            learningScenarioButtons[i].GetComponentInChildren<Text>().text = languageManager.GetValue((string)buttonTitles[i]);
            learningScenarioButtons[i].interactable = settingsManager.GetBooleanValueByName((string)buttonTitles[i]);
        }
    }

    public void SetLanguage(string language)
    {
        settingsManager.SetValueByName("language", language);
        languageManager.ReloadSettings();
        InitializeText();
    }

    public void setLearningUnit(string unit)
    {
        sceneName = unit;

        // Save scene name in the settings and set level to 1
        // settingsManager.SetValueByName("scene_name", sceneName);
        // settingsManager.SetValueByName("level", 1);
    }

    public void setAndPlayLearningUnit(string unit)
    {
        sceneName = unit;

        // Save scene name in the settings
        settingsManager.SetValueByName("scene_name", sceneName);

        PlayLearningUnit();
    }

    public void PlayLearningUnit(int level = 1)
    {
        switch (sceneName)
        {
            case "workbench":
                level = level + 3;
                break;
            case "house":
                level = level + 4;
                break;
            default:
                break;
        }
        settingsManager.SetValueByName("scene_name", sceneName);
        settingsManager.SetValueByName("level", level);
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

    public void QuitButton()
    {
        // Quit Game
        Application.Quit();
    }

}