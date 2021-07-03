using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneMenuHandler : MonoBehaviour
{
    private SettingsManager settingsManager;
    private LanguageManager languageManager;
    public MenuButtonWatcher watcher;
    public GameObject SceneMenu;
    public GameObject Question;
    public GameObject Theory;
    public GameObject QuestionContinue;
    public GameObject QuestionBack;
    public GameObject Result;
    public GameObject ResultQuit;

    public GameObject RightHandController;
    public GameObject RightHandMenuController;
    public GameObject LeftHandController;
    public GameObject LeftHandMenuController;
    public GameObject player;
    public WireVerification wVerification;
    public ComponentVerification cVerification;
    public string sceneShortName;
    public bool menuHasVariablePosition = false;

    public bool initial = true;
    private bool isActive;
    private bool isPressed = false;

    private string title;
    private string questionText;
    private string theoryText;
    private string theoryTitle;

    public int level = 4;

    void Start()
    {
        if(LeftHandController != null ) LeftHandController.SetActive(false);
        if(RightHandController != null) RightHandController.SetActive(false);
        watcher.menuButtonPress.AddListener(onMenuButtonEvent);
        isActive = true;
        
        this.settingsManager = new SettingsManager();
        this.languageManager = new LanguageManager(settingsManager);
        // sceneShortName = settingsManager.GetStringValueByName("scene_short_name");
        var levelTemp = settingsManager.GetIntValueByName("level");
        if(levelTemp < level)
        {
            settingsManager.resetToActualLevel(level);
        } else if(levelTemp > level)
        {
            level = levelTemp;
        }
        title = sceneShortName + "_title";
        questionText = sceneShortName + "_question_text";
        theoryText = sceneShortName + "_theory_text";
        theoryTitle = sceneShortName + "_theory_title";
        if (this.initial)
        {
            Debug.Log("Activate Initial Question");
            setAllTextes();
            ActivateQuestion();
        } else
        {
            Debug.Log("Deactivate Initial Question");
            toggleSceneMenu();
        }
    }

    private void setAllTextes()
    {
        SceneMenu.GetComponentInChildren<Text>().text = languageManager.GetValue(this.title);
        var sceneMenuButtons = SceneMenu.GetComponentsInChildren<Button>();
        sceneMenuButtons[0].GetComponentInChildren<Text>().text = languageManager.GetValue("cntnue");
        if(sceneMenuButtons.Length > 2)
        {
            sceneMenuButtons[1].GetComponentInChildren<Text>().text = languageManager.GetValue("theory");
            sceneMenuButtons[2].GetComponentInChildren<Text>().text = languageManager.GetValue("question");
            sceneMenuButtons[3].GetComponentInChildren<Text>().text = languageManager.GetValue("check");
        }
        sceneMenuButtons[sceneMenuButtons.Length - 1].GetComponentInChildren<Text>().text = languageManager.GetValue("quit");

        if(Question != null)
        {
            var questionsText = Question.GetComponentsInChildren<Text>();
            questionsText[0].text = languageManager.GetValue("task");
            questionsText[1].text = languageManager.GetValue(this.questionText);
            var questionButtons = Question.GetComponentsInChildren<Button>();
            questionButtons[0].GetComponentInChildren<Text>().text = languageManager.GetValue("cntnue");
            questionButtons[1].GetComponentInChildren<Text>().text = languageManager.GetValue("back");
            questionButtons[2].GetComponentInChildren<Text>().text = languageManager.GetValue("quit");
        }
        
        if(Theory != null)
        {
            var theoryText = Theory.GetComponentsInChildren<Text>();
            theoryText[0].text = languageManager.GetValue(this.theoryTitle);
            theoryText[1].text = languageManager.GetValue(this.theoryText);
            var theoryButtons = Theory.GetComponentsInChildren<Button>();
            theoryButtons[0].GetComponentInChildren<Text>().text = languageManager.GetValue("back");
            theoryButtons[1].GetComponentInChildren<Text>().text = languageManager.GetValue("quit");
        }
    }

    public void onMenuButtonEvent(bool pressed)
    {
        if((isPressed != pressed) && pressed)
        {
            toggleSceneMenu();
            isPressed = pressed;
        } else if((isPressed != pressed) && !pressed)
        {
            isPressed = pressed;
        }
    }

    public void ActivateSceneMenu(){
        SceneMenu.SetActive(true);
        Question.SetActive(false);
        Theory.SetActive(false);
        Result.SetActive(false);
        isActive = true;
    }

    public void DeactivateSceneMenu()
    {
        SceneMenu.SetActive(false);
        Question.SetActive(false);
        Theory.SetActive(false);
        Result.SetActive(false);
        toggleControllerMode();
        isActive = false;
    }

    public void HomeButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("home");
    }

    public void ActivateQuestion()
    {
        Result.SetActive(false);
        SceneMenu.SetActive(false);
        Theory.SetActive(false);
        Question.SetActive(true);
        QuestionContinue.SetActive(this.initial);
        QuestionBack.SetActive(!this.initial);
        if(this.initial)
        {
            this.initial = false;
        }
    }

    public void ActivateTheory()
    {
        SceneMenu.SetActive(false);
        Theory.SetActive(true);
        Question.SetActive(false);
        Result.SetActive(false);
    }


    public void QuitButton()
    {
        // Quit Game
        Application.Quit();
    }

    public void checkResultButton()
    {
        Debug.Log("Check result");
        var success = false;
        if(wVerification != null)
        {
            Debug.Log("Doing wire verification");
            success = wVerification.verify();
        } else if(cVerification != null)
        {
            Debug.Log("Doing component verification");
            success = cVerification.verify();
        }
        setResult(success);
        showResult();
    }

    private void setResult(bool success)
    {
        var resultTexts = Result.GetComponentsInChildren<Text>();
        var resultButtons = Result.GetComponentsInChildren<Button>();
        var resultTitle = "failure_title";
        var resultText = "failure_text";
        resultButtons[0].onClick.AddListener(() => toggleSceneMenu());
        resultButtons[1].onClick.AddListener(() => retry());
        resultButtons[1].GetComponentInChildren<Text>().text = languageManager.GetValue("retry");
        if (success)
        {
            resultTitle = "success_title";
            resultText = "success_text";
            //int level = settingsManager.GetIntValueByName("level");
            settingsManager.SetLevelPassed(level);
            resultButtons[0].onClick.AddListener(() => HomeButton());
            ResultQuit.SetActive(false);
        }
        resultTexts[0].text = languageManager.GetValue(resultTitle);
        resultTexts[1].text = languageManager.GetValue(resultText);
    }

    public void nextLevel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(settingsManager.getNextLevelName());
    }

    public void retry()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(settingsManager.GetStringValueByName("scene_name"));
    }

    private void showResult()
    {
        SceneMenu.SetActive(false);
        Theory.SetActive(false);
        Question.SetActive(false);
        Result.SetActive(true);
    }

    public void toggleSceneMenu()
    {
        if(menuHasVariablePosition)
        {
            setNewMenuPosition();
        }
        SceneMenu.SetActive(!isActive);
        toggleControllerMode();
        if (isActive)
        {
            if (Question != null) Question.SetActive(false);
            if (Theory != null) Theory.SetActive(false);
            if (Result != null) Result.SetActive(false);
        }
        isActive = !isActive;
    }

    private void toggleControllerMode()
    {
        if (LeftHandController != null) LeftHandController.SetActive(isActive);
        if (LeftHandMenuController != null) LeftHandMenuController.SetActive(!isActive);
        if (RightHandController != null) RightHandController.SetActive(isActive);
        if (RightHandMenuController != null) RightHandMenuController.SetActive(!isActive);
    }

    public void setNewMenuPosition()
    {
        /*var xrRig = player.GetComponentInChildren<GameObject>();
        Vector3 playerPosition = xrRig.transform.position;*/
        //Vector3 menuPosition = gameObject.transform.position;
        // GameObject parent = SceneMenu.GetComponentInParent<GameObject>();
        //gameObject.transform.position.Set(menuPosition.x, menuPosition.y, 100);
    }
}