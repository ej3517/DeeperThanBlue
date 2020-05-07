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

    private void Awake()
    {
        scoreText = transform.Find("scoreText").GetComponent<Text>();
        transform.Find("retryBtn").GetComponent<Button_UI>().ClickFunc = () =>
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
            };
        Hide();
    }

    private void Start()
    {
        Bird.GetInstance().OnDied += Bird_OnDied;
    }
    

        private void Bird_OnDied(object sender, EventArgs e)
    {
        scoreText.text = Level.GetInstance().GetPipesPassedCount().ToString();
        Show();
    }

    private void Hide()
    {
        gameObject.SetActive(true);
    }
    private void Show()
    {
        gameObject.SetActive(true);
    }

}
