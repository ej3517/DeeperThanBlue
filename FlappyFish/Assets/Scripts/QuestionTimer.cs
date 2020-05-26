using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestionTimer : MonoBehaviour
{
    private Text timer;

    void Start()
    {
        timer = GetComponent<Text>();
    }

    void Update()
    {
        timer.text = "test";
    }
}
