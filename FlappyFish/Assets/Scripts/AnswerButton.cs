using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnswerButton : MonoBehaviour
{
    public Text answerText;
    public Button button;
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
        ColorBlock colors = button.colors;
        if (answerData.isTrue)
        {
            colors.pressedColor = Color.green;
        }
        else
        {
            colors.pressedColor = Color.red;
        }
        button.colors = colors;
    }

    public void HandleClick()
    {
        quizGameController.AnswerButtonClicked(answerData.isTrue);
    }
}
