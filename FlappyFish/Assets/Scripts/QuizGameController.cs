using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class QuizGameController : MonoBehaviour
{
    public Text questionDisplayText;
    public Text scoreDisplayText;
    public SimpleObjectPool answerButtonObjectPool;
    public Transform answerButtonParent;
    public QuestionWindow questionWindow;

    private DataController dataController;
    private RoundData currentRoundData;
    private QuestionData[] questionPool;
    private StateController stateControllerScript;
    
    private int questionIndex;
    public int playerScore;
    private List<GameObject> answerButtonGameObjects = new List<GameObject>();
    private bool doOnce;
    
    // Start is called before the first frame update
    void Start()
    {
        dataController = FindObjectOfType<DataController>();
        currentRoundData = dataController.GetCurrentRoundData();
        questionPool = currentRoundData.questions;

        stateControllerScript = GameObject.Find("StateController").GetComponent<StateController>();

        playerScore = 0;
        scoreDisplayText.text = playerScore.ToString();
        questionIndex = 0;

        ShowQuestion();

        doOnce = false;
    }

    private void ShowQuestion()
    {
        RemoveAnswerButton();
        QuestionData questionData = questionPool [questionIndex];
        questionDisplayText.text = questionData.questionText;

        for (int i = 0; i < questionData.answers.Length; i++)
        {
            GameObject answerButtonGameObject = answerButtonObjectPool.GetObject();
            
            AnswerButton answerButton = answerButtonGameObject.GetComponent<AnswerButton>();
            answerButton.SetUp(questionData.answers[i]);

            answerButtonGameObject.transform.SetParent(answerButtonParent, false); // worldPositionStays argument set to false -> This will retain local orientation and scale
            answerButtonGameObjects.Add(answerButtonGameObject);
        }
    }

    private void RemoveAnswerButton()
    {
        while (answerButtonGameObjects.Count > 0)
        {
            answerButtonObjectPool.ReturnObject(answerButtonGameObjects[0]);
            answerButtonGameObjects.RemoveAt(0);
        }
    }

    public void AnswerButtonClicked(bool isCorrect)
    {
        if (!doOnce)
        {
            doOnce = true;
            if (isCorrect)
            {
                playerScore += currentRoundData.pointsAddedForCorrectAnswer;
                scoreDisplayText.text = playerScore.ToString();
            }

            if (questionPool.Length > questionIndex + 1)
            {
                questionIndex++;
                ShowQuestion();
                stateControllerScript.currentState = StateController.State.Playing;
                questionWindow.Hide();
            }
            else
            {
                //End of quetionnaire : For now let's just keep playing !!!
                stateControllerScript.currentState = StateController.State.Playing;
                questionWindow.Hide();
            }
        }
    }

    void Update()
    {
        if (stateControllerScript.currentState == StateController.State.WaitingAnswer)
        {
            doOnce = false;
        }
    }
}
