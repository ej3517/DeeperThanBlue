using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnswerButton : MonoBehaviour
{
    public Text answerText;
    private AnswersList answerData;
    private QuizGameController quizGameController;
    // Start is called before the first frame update
    void Start()
    {
        quizGameController = FindObjectOfType<QuizGameController>();
    }

    public void SetUp(AnswersList data)
    {
        answerData = data;
        answerText.text = answerData.answer;
    }

    public void HandleClick()
    {
        quizGameController.AnswerButtonClicked(answerData.isTrue);
        // quizGameController.AnswerButtonClicked (answerData.isCorrect);
    }
}
