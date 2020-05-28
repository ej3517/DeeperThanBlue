using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreWindow : MonoBehaviour
{
    private Text scoreText;
    private Text highScoreText;

    private void Awake()
    {
        scoreText = transform.Find("ScoreText").GetComponent<Text>();
        highScoreText = transform.Find("HighScoreText").GetComponent<Text>();
    }

    private void Start()
    {
        highScoreText.text = "HIGHSCORE " + Score.GetHighScore().ToString();
    }

    private void Update()
    {
        try
        {
            scoreText.text = Level.GetInstance().GetPipesPassedCount().ToString();
        }
        catch (Exception)         // Just to hide the errors when they're not needed
        {; }
    }
}
