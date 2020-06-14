using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnswerButton : MonoBehaviour
{
    public Text answerText;
    private AnswerData answerData;
    private QuizGameController quizGameController;
    // Start is called before the first frame update
    void Start()
    {
        quizGameController = FindObjectOfType<QuizGameController>();
    }

    public void SetUp(AnswerData data)
    {
        answerData = data;
        answerText.text = answerData.answerText;
    }

    public void HandleClick()
    {
        quizGameController.AnswerButtonClicked(answerData.isCorrect);
    }
}
