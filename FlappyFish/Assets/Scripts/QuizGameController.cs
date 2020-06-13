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
    public WinningWindow winningWindow;
    public Level levelScript;

    private DataController dataController;
    private RoundData currentRoundData;
    private QuestionsList[] questionPoolEasy;
    private QuestionsList[] questionPoolHard;
    private StateController stateControllerScript;
    
    private int questionIndexEasy;
    private int questionIndexHard;
    public int playerScore;
    public bool currentlyHard;
    private List<GameObject> answerButtonGameObjects = new List<GameObject>();
    private bool doOnce;
    
    // Start is called before the first frame update
    void Start()
    {
        dataController = FindObjectOfType<DataController>();
        currentRoundData = dataController.GetCurrentRoundData();
        questionPoolEasy = currentRoundData.GetEasyQuestion().questions;
        questionPoolHard = currentRoundData.GetHardQuestion().questions;
        questionIndexEasy = 0;
        questionIndexHard = 0;

        stateControllerScript = GameObject.Find("StateController").GetComponent<StateController>();

        playerScore = 0;
        scoreDisplayText.text = playerScore.ToString();

        currentlyHard = true;
        ShowQuestion();

        doOnce = false;
    }

    private void ShowQuestion()
    {
        RemoveAnswerButton();
        QuestionsList questionData;
        if (currentlyHard) {
            questionData = questionPoolHard[questionIndexHard]; // select the hard questions 
        }
        else {
            questionData = questionPoolEasy[questionIndexEasy]; // select the easy questions
        }
        questionDisplayText.text = questionData.question;

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
            if (currentlyHard)
            {
                if (isCorrect)
                {
                    playerScore += MyGlobals.POINTS_HARD_QUESTION;
                    if (questionPoolHard.Length > questionIndexHard + 1)
                    {
                        questionIndexHard++;
                        ShowQuestion();
                        stateControllerScript.currentState = StateController.State.Playing;
                        questionWindow.Hide();
                    }
                    else
                    {
                        stateControllerScript.currentState = StateController.State.Playing;
                        questionWindow.Hide();
                        questionIndexHard = 0; // start to first question
                    }
                }
                else
                {
                    stateControllerScript.currentState = StateController.State.Dead;
                }
            }
            else
            {
                if (isCorrect)
                {
                    playerScore += MyGlobals.POINTS_EASY_QUESTION;
                    levelScript.birdSpeed += MyGlobals.SPEED_RING_BOOST;
                }
                if (questionPoolEasy.Length > questionIndexEasy + 1)
                {
                    questionIndexEasy++;
                    ShowQuestion();
                    stateControllerScript.currentState = StateController.State.Playing;
                    questionWindow.Hide();
                }
                else
                {
                    stateControllerScript.currentState = StateController.State.Playing;
                    questionWindow.Hide();
                    questionIndexEasy = 0; //start to last question
                }
            }
        }
    }

    public void GetHardQuestion()
    {
        currentlyHard = true;
        ShowQuestion();
    }
    
    public void GetEasyQuestion()
    {
        currentlyHard = false;
        ShowQuestion();
    }
    
    void Update()
    {
        if (playerScore >= MyGlobals.WINNING_THRESHOLD)
        {
            winningWindow.Show();
            stateControllerScript.currentState = StateController.State.Won;
        }
        scoreDisplayText.text = playerScore.ToString();
        if (stateControllerScript.currentState == StateController.State.WaitingAnswer)
        {
            doOnce = false;
        }
    }
}
