using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class QuestionWindow : MonoBehaviour
{
    private static Text questionText;
    private static QuestionWindow instance;

    private static List<string> questionList;

   private void Awake()
   {
       Hide();
       questionText = transform.Find("QuestionText").GetComponent<Text>();

        questionList = new List<string>();
        //Import questions
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
        int questionNumber = UnityEngine.Random.Range(0, questionList.Count);
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

}
