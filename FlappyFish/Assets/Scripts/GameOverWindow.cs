using System;
using UnityEngine;
using UnityEngine.UI;

public class GameOverWindow : MonoBehaviour
{
    
    private Text scoreText;
    private Text highScoreText;
    private StateController stateControllerScript;
    private QuizGameController quizGameControllerScript;
    private bool wasAlive;
    
    private void Start()
    {
        scoreText = transform.Find("scoreText").GetComponent<Text>();
        highScoreText = transform.Find("highScoreText").GetComponent<Text>();
        stateControllerScript = GameObject.Find("StateController").GetComponent<StateController>();
        quizGameControllerScript = GameObject.Find("QuizGameController").GetComponent<QuizGameController>();
        wasAlive = true;
        Hide();
    }

    private void Update()
    {
        switch (stateControllerScript.currentState)
        {
            case StateController.State.Dead:
                int newscore = quizGameControllerScript.playerScore;
                scoreText.text = newscore.ToString()+"/100";
                /*if (Score.TrySetNewHighScore(Level.GetInstance().GetPipesPassedCount()))
                {
                    // New highscore
                    highScoreText.text = "NEW HIGHSCORE";
                }
                else
                {
                    highScoreText.text = "HIGHSCORE " + Score.GetHighScore().ToString();
                }*/
                if(wasAlive){

                    // PERSONAL HIGHSCORES
                    myHighScoreTable.Add_highscore(newscore);

                    // STATISTICS - TIMES WON/LOST
                    // get saved values
                    string strWon = PlayerPrefs.GetString("timesWon");
                    string strLost = PlayerPrefs.GetString("timesLost");
                    int tmpWon = Int32.Parse(strWon);
                    int tmpLost = Int32.Parse(strLost);

                    if (newscore >= 50){
                        tmpWon++;
                    }
                    else {
                        tmpLost++;
                    }
                    // save updated values
                    PlayerPrefs.SetString("timesWon", tmpWon.ToString());
                    PlayerPrefs.Save();
                    PlayerPrefs.SetString("timesLost", tmpLost.ToString());
                    PlayerPrefs.Save();

                    wasAlive = false;
                }
            
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
        SoundManager.PlaySound(SoundManager.Sound.Lose);
        gameObject.transform.localScale = new Vector3(1,1,1);
    }

}
