using System;
using UnityEngine;
using UnityEngine.UI;


public class GameOverWindow : MonoBehaviour
{
    private Text scoreText;
    private Text highScoreText;

    private void Awake()
    {
        scoreText = transform.Find("scoreText").GetComponent<Text>();
        highScoreText = transform.Find("highScoreText").GetComponent<Text>();
        Hide();
    }

    private void Start()
    {
        Bird.GetInstance().OnDied += Bird_OnDied;
    }
    

    private void Bird_OnDied(object sender, EventArgs e)
    {
        scoreText.text = Level.GetInstance().GetPipesPassedCount().ToString();
        if (Score.TrySetNewHighScore(Level.GetInstance().GetPipesPassedCount()))
        {
            // New highscore
            highScoreText.text = "NEW HIGHSCORE";
        }
        else
        {
            highScoreText.text = "HIGHSCORE " + Score.GetHighScore().ToString();
        }

        Show();

        // PERSONAL HIGHSCORES
        int newscore = Level.GetInstance().GetPipesPassedCount();
        HighScoreTable.Add_highscore(newscore);

        // STATISTICS - TIMES WON/LOST
        // ** get saved values
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
        // ** save updated values
        PlayerPrefs.SetString("timesWon", tmpWon.ToString());
        PlayerPrefs.Save();
        PlayerPrefs.SetString("timesLost", tmpLost.ToString());
        PlayerPrefs.Save();

    }

    private void Hide()
    {
        gameObject.transform.localScale = new Vector3(0,0,0);
    }
    private void Show()
    {
        gameObject.transform.localScale = new Vector3(1,1,1);
    }

}
