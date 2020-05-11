using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class QuestionWindow : MonoBehaviour
{
    private static Text questionText;
    private static QuestionWindow instance;

   private void Awake()
   {
       Hide();
       questionText = transform.Find("QuestionText").GetComponent<Text>();
    }

    public QuestionWindow()
    {
        instance = this;
    }


    public static QuestionWindow getInstance()
    {
        return instance;
    }

    public void displayQuestion()
    {
        questionText.text = "Test";         // TODO: change this to the actual question
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

}
