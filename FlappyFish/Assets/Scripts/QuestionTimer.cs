using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestionTimer : MonoBehaviour
{
    private Text timer;
    private float currentTime;
    public float timerSpeed = 1f;

    void Start()
    {
        timer = GetComponent<Text>();
    }

    void Update()
    {
        currentTime += Time.deltaTime;
        timer.text = (currentTime%60).ToString("0.0");
    }
}
