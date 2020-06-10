using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class QuestionWindow : MonoBehaviour
{
    // Countdown Timer
    public Text timeRemainingDisplayText;
    
    private float timeRemaining;
    private bool canCount;
    private bool doOnce;

    private void Awake()
    {
       Hide();
       canCount = true;
       doOnce = false;
    }
    
    private void UpdateTimeRemainingDisplay()
    {
        timeRemainingDisplayText.text = timeRemaining.ToString("f");
    }

    private void ResetQuestionTimer()
    {
        timeRemaining = MyGlobals.DURATION_EASY_QUESTION;
    }

    public void Hide()
    {
        gameObject.transform.localScale = new Vector3(0, 0, 0);
    }
    public void Show()
    {
        ResetQuestionTimer();
        canCount = true;
        doOnce = false;
        gameObject.transform.localScale = new Vector3(1, 1, 1);
    }
    
    void Update()
    {
        if (timeRemaining >= 0.0f && canCount)
        {
            timeRemaining -= Time.deltaTime;
            UpdateTimeRemainingDisplay();
        }

        if (timeRemaining < 0.0f && !doOnce)
        {
            canCount = false;
            doOnce = true;
            Hide();
        }
    }
}
