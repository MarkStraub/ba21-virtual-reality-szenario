using UnityEngine;

public class TheoryQuizManager
{
    private readonly TheoryQuiz theoryQuiz;

    public TheoryQuizManager()
    {
        theoryQuiz =  new TheoryQuiz();
    }

    public string GetStringValue(string name)
    {
        return theoryQuiz.GetType().GetField(name).GetValue(theoryQuiz).ToString();
    }

    public bool GetBooleanValue(string name)
    {
        return bool.Parse(theoryQuiz.GetType().GetField(name).GetValue(theoryQuiz).ToString());
    }
}
