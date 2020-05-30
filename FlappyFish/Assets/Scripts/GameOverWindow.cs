using System;
using UnityEngine;
using UnityEngine.UI;


public class GameOverWindow : MonoBehaviour
{
    
    private Text scoreText;
    private Text highScoreText;
    private StateController stateControllerScript;
    private QuizGameController quizGameControllerScript;
    
    private void Start()
    {
        scoreText = transform.Find("scoreText").GetComponent<Text>();
        highScoreText = transform.Find("highScoreText").GetComponent<Text>();
        stateControllerScript = GameObject.Find("StateController").GetComponent<StateController>();
        quizGameControllerScript = GameObject.Find("QuizGameController").GetComponent<QuizGameController>();
        Hide();
    }

    private void Update()
    {
        switch (stateControllerScript.currentState)
        {
            case StateController.State.Dead:
                scoreText.text = quizGameControllerScript.playerScore.ToString();
                /*if (Score.TrySetNewHighScore(Level.GetInstance().GetPipesPassedCount()))
                {
                    // New highscore
                    highScoreText.text = "NEW HIGHSCORE";
                }
                else
                {
                    highScoreText.text = "HIGHSCORE " + Score.GetHighScore().ToString();
                }*/
                
                Show();
                break;
        }
    }
    
    public void Hide()
    {
        gameObject.transform.localScale = new Vector3(0,0,0);
    }
    public void Show()
    {
        gameObject.transform.localScale = new Vector3(1,1,1);
    }

}
