using UnityEngine;
using UnityEngine.UI;

public class TheoryInitialisation : MonoBehaviour
{
    // Theory
    public GameObject theory;

    public Text theoryTitle;
    public Text theoryText;

    public Button backT;
    public Button nextT;

    // Quiz
    public GameObject quiz;

    public Text quizTitle;
    public Text questionText;

    public Button answerA;
    public Button answerB;
    public Button answerC;
    public Button answerD;
    public Button cancelQ;
    public Button nextQ;

    // Result
    public GameObject result;

    public Text resultTitle;
    public Text resultText;

    public Button repeatButtonR;
    public Button finishButtonR;


    private SettingsManager settingsManager;
    private LanguageManager languageManager;
    private TheoryQuizManager theoryQuizManager;
    private int level;
    private int page = 1;

    private int countCorrect = 0;
    private int countFalse = 0;

    private ColorBlock defaultButtonColor;
    private ColorBlock correctButtonColor;
    private ColorBlock falseButtonColor;

    // Start is called before the first frame update
    void Start()
    {
        settingsManager = new SettingsManager();
        languageManager = new LanguageManager(settingsManager);
        theoryQuizManager = new TheoryQuizManager();

        // Load Settings
        level = settingsManager.GetIntValueByName("level");

        // Initialize
        InitializeTheory();
        InitializeQuiz();
        InitializeResult();
        InitializeColors();

        // Set correct text for the first board
        UpdateTheoryBoard();
    }

    private void InitializeTheory()
    {
        backT.gameObject.SetActive(false);
        backT.GetComponentInChildren<Text>().text = languageManager.GetValue("back");
        nextT.GetComponentInChildren<Text>().text = languageManager.GetValue("next");
        backT.onClick.AddListener(() => BackButtonTOnClick());
        nextT.onClick.AddListener(() => NextButtonTOnClick());
    }

    private void InitializeQuiz()
    {
        quiz.gameObject.SetActive(false);
        quizTitle.text = languageManager.GetValue("quiz");
        cancelQ.GetComponentInChildren<Text>().text = languageManager.GetValue("cancel");
        nextQ.GetComponentInChildren<Text>().text = languageManager.GetValue("next");
        cancelQ.onClick.AddListener(() => CancelButtonQOnClick());
        nextQ.onClick.AddListener(() => NextButtonQOnClick());
        answerA.onClick.AddListener(() => AnswerButtonOnClick("a", answerA));
        answerB.onClick.AddListener(() => AnswerButtonOnClick("b", answerB));
        answerC.onClick.AddListener(() => AnswerButtonOnClick("c", answerC));
        answerD.onClick.AddListener(() => AnswerButtonOnClick("d", answerD));
    }

    private void InitializeResult()
    {
        result.gameObject.SetActive(false);
        repeatButtonR.GetComponentInChildren<Text>().text = languageManager.GetValue("repeat");
        finishButtonR.GetComponentInChildren<Text>().text = languageManager.GetValue("finish");
        repeatButtonR.onClick.AddListener(() => RepeatButtonROnClick());
        finishButtonR.onClick.AddListener(() => FinishButtonROnClick());
        finishButtonR.gameObject.SetActive(false);
    }

    private void InitializeColors()
    {
        defaultButtonColor = answerA.colors;
        correctButtonColor = answerA.colors;
        correctButtonColor.normalColor = Color.green;
        correctButtonColor.disabledColor = Color.green;
        falseButtonColor = answerA.colors;
        falseButtonColor.normalColor = Color.red;
        falseButtonColor.disabledColor = Color.red;
    }

    // https://docs.unity3d.com/530/Documentation/ScriptReference/UI.Button-onClick.html
    private void BackButtonTOnClick()
    {
        // Go to previous page
        page--;
        Debug.Log($"Back button on THEORY clicked. Page is now {page}");

        // If we are on the first page, we disable the back button
        if (page == 1)
        {
            backT.gameObject.SetActive(false);
        }

        UpdateTheoryBoard();
    }

    private void NextButtonTOnClick()
    {
        // Go to next page
        page++;
        Debug.Log($"Next button on THEORY clicked. Page is now {page}");

        // If we are not on the first page, we enable the back button
        if (page > 1)
        {
            backT.gameObject.SetActive(true);
        }

        UpdateTheoryBoard();
    }

    private void UpdateTheoryBoard()
    {
        var theoryTxt = languageManager.GetValue($"th_{level}_text_{page}");
        if (theoryTxt != null && theoryTxt != "")
        {
            theoryTitle.text = languageManager.GetValue($"th_{level}_title");
            theoryText.text = theoryTxt;
        }
        else
        {
            // There is no next page. => Go to quiz
            theory.gameObject.SetActive(false);
            quiz.gameObject.SetActive(true);
            backT.gameObject.SetActive(false);

            // Start a new quiz
            page = 1;
            countCorrect = 0;
            countFalse = 0;

            UpdateQuizBoard();
        }
    }

    private void CancelButtonQOnClick()
    {
        Debug.Log($"Cancel button on QUIZ clicked. Go back to theory");

        theory.gameObject.SetActive(true);
        quiz.gameObject.SetActive(false);
        page = 1;

        UpdateTheoryBoard();
    }

    private void NextButtonQOnClick()
    {
        // Go to next page
        page++;
        Debug.Log($"Next button on QUIZ clicked. Page is now {page}");

        UpdateQuizBoard();
    }

    private void UpdateQuizBoard()
    {
        var questionTxt = languageManager.GetValue($"qz_{level}_question_{page}");
        if (questionTxt != null && questionTxt != "")
        {
            questionText.text = questionTxt;
            UpdateAnswerButton(answerA, 'a');
            UpdateAnswerButton(answerB, 'b');
            UpdateAnswerButton(answerC, 'c');
            UpdateAnswerButton(answerD, 'd');

            // Always disable the next button that the user has to choose an answer
            nextQ.gameObject.SetActive(false);

            // Change all answer buttons to default button color
            answerA.colors = defaultButtonColor;
            answerB.colors = defaultButtonColor;
            answerC.colors = defaultButtonColor;
            answerD.colors = defaultButtonColor;

            // Enable all buttons
            answerA.interactable = true;
            answerB.interactable = true;
            answerC.interactable = true;
            answerD.interactable = true;
        }
        else
        {
            // There is no next page. => Go to quiz
            quiz.SetActive(false);
            result.SetActive(true);

            UpdateResultBoard();
        }
    }

    private void UpdateAnswerButton(Button button, char letter)
    {
        // Check if the answer button is active
        if (theoryQuizManager.GetBooleanValue($"qz_{level}_{letter}_{page}_active"))
        {
            button.gameObject.SetActive(true);
            button.GetComponentInChildren<Text>().text = languageManager.GetValue($"qz_{level}_answer_{letter}_{page}");
        }
        else
        {
            button.gameObject.SetActive(false);
        }
    }

    private void AnswerButtonOnClick(string answer, Button answerButton)
    {
        // Disable all buttons
        answerA.interactable = false;
        answerB.interactable = false;
        answerC.interactable = false;
        answerD.interactable = false;


        string solution = theoryQuizManager.GetStringValue($"qz_{level}_solution_{page}");
        Debug.Log($"Answer: {answer} | Solution: {solution}");

        if (answer == solution)
        {
            countCorrect++;
            answerButton.colors = correctButtonColor;
        }
        else
        {
            countFalse++;
            answerButton.colors = falseButtonColor;
        }

        // Enable next button
        nextQ.gameObject.SetActive(true);

    }

    private void UpdateResultBoard()
    {
        resultTitle.text = languageManager.GetValue("qz_result");

        var totalQuestions = countCorrect + countFalse;
        // https://docs.microsoft.com/en-us/dotnet/api/system.double.tostring?view=net-5.0
        var percentage = (double)countCorrect / totalQuestions;

        // Show correct button if 100% show finish button, repeat otherwise
        if (percentage == 1)
        {
            finishButtonR.gameObject.SetActive(true);

            // Enable the next level
            settingsManager.SetLevelPassed(level);
        }


        var txt = languageManager.GetValue("qz_total");
        txt += $": {totalQuestions}\n\n\n";
        txt += $"{languageManager.GetValue("qz_correct")}: {countCorrect}\n";
        txt += $"{languageManager.GetValue("qz_false")}: {countFalse}\n\n\n";
        txt += $"{languageManager.GetValue("qz_percentage")}: {percentage.ToString("p", System.Globalization.CultureInfo.InvariantCulture)}";

        resultText.text = txt;
    }

    private void RepeatButtonROnClick()
    {
        Debug.Log($"Repeat button on RESULT clicked. Go back to theory");

        theory.SetActive(true);
        result.SetActive(false);
        page = 1;

        UpdateTheoryBoard();
    }

    private void FinishButtonROnClick()
    {
        // Return to main menu
        UnityEngine.SceneManagement.SceneManager.LoadScene("home");
    }
}
