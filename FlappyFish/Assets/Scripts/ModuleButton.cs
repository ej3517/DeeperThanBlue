using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModuleButton : MonoBehaviour
{
    public Text moduleText;
    private RoundData roundData;
    private ModuleController moduleController;
    // Start is called before the first frame update
    
    void Start()
    {
        moduleController = FindObjectOfType<ModuleController>();
    }

    public void SetUp(RoundData data)
    {
        roundData = data;
        moduleText.text = roundData.module;
    }

    public void HandleClick()
    {
        moduleController.ModuleButtonClicked(roundData);
    }

    public void HandleLeaderboardClick()
    {
        Debug.Log("Leaderoboard Click called");
    }
}
