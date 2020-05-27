using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class QuestionWindow : MonoBehaviour
{
    private Text questionText;
    private List<string> questionList;
    private static QuestionWindow instance;
    private int questionNumber;
    
    // Countdown Timer
    private Text timerText;
    public float timer;
    private bool canCount;
    private bool doOnce;

    private void Awake()
    {
       Hide();
       // Countdown Timer
       timerText = transform.Find("QuestionTimer").GetComponent<Text>();
       canCount = true;
       doOnce = false;
       //Import questions
       questionText = transform.Find("QuestionText").GetComponent<Text>();
       questionList = new List<string>();
       try
       {
            using (StreamReader sr = new StreamReader("Questions.txt"))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    questionList.Add(line);
                }
            }
       }
       catch (IOException e)
       {
            Debug.LogWarning("Could not read Questions.txt: " + e.Message);
       }
    }
    
    void Update()
    {
        if (timer >= 0.0f && canCount)
        {
            timer -= Time.deltaTime;
            timerText.text = timer.ToString("F");
        }

        if (timer < 0.0f && !doOnce)
        {
            canCount = false;
            doOnce = true;
            timerText.text = "0.00";
            timer = 0.0f;
            Hide();
        }
    }

    public QuestionWindow()
    {
        instance = this;
    }

    public static QuestionWindow GetInstance()
    {
        return instance;
    }

    public void DisplayQuestion()
    {
        // timer = MyGlobals.DURATION_EASY_QUESTION;
        canCount = true;
        doOnce = false;
        questionNumber = UnityEngine.Random.Range(0, questionList.Count);
        questionText.text = questionList[questionNumber];
        Show();
    }

    public void Hide()
    {
        gameObject.transform.localScale = new Vector3(0, 0, 0);
    }
    private void Show()
    {
        gameObject.transform.localScale = new Vector3(1, 1, 1);
    }

    public void YesButton()
    {
    }

    public void NoButton()
    {
    }

}
