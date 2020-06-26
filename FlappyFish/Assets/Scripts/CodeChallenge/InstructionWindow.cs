using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstructionWindow : MonoBehaviour
{
    public Transform button;

    void Start()
    {
        if(PlayerPrefs.GetInt("codeChallengeShowInstruciton") != 0)
        {
            transform.localPosition = new Vector3(0, 0, -100);
        }
    }

    public void Close()
    {
        Toggle toggle = button.GetComponent<Toggle>();
        if(toggle.isOn)
        {
            Debug.Log("Not going to show instruction window next time...");
            PlayerPrefs.SetInt("codeChallengeShowInstruciton", 0);
        }
        transform.localPosition = new Vector3(0, 2000, -100);
    }
}
