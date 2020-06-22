using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinningWindow : MonoBehaviour
{
    private Text scoreText;
    private Text highScoreText;
    private StateController stateControllerScript;
    private DataController dataController;
    private RoundData currentRoundData;
    
    private void Start()
    {
        dataController = FindObjectOfType<DataController>();
        currentRoundData = dataController.GetCurrentRoundData();
        scoreText = transform.Find("scoreText").GetComponent<Text>();
        stateControllerScript = GameObject.Find("StateController").GetComponent<StateController>();
        Hide();
    }

    private void Update()
    {
        switch (stateControllerScript.currentState)
        {
            case StateController.State.Won:
                scoreText.text = "You have reached 100 in " + currentRoundData.module;
                Show();
                break;
        }
    }
    
    public void Hide()
    {
        gameObject.transform.localScale = new Vector3(0,0,0);
    }
    public void Show()
    {
        SoundManager.PlaySound(SoundManager.Sound.Win);
        gameObject.transform.localScale = new Vector3(1,1,1);
    }
}
