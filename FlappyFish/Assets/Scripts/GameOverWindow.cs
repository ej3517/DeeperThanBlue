﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;
using UnityEditor.U2D;

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