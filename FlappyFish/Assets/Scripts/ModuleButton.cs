using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModuleButton : MonoBehaviour
{
    public Text moduleText;
    private RoundData roundData;
    private LeaderboardStructure leaderboardStructure; 
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

    public void SetUpLeaderboard(LeaderboardStructure data)
    {
        leaderboardStructure = data; 
        moduleText.text = leaderboardStructure.module; 
    }

    public void HandleClick()
    {
        moduleController.ModuleButtonClicked(roundData);
    }

    public void HandleLeaderboardClick()
    {
        moduleController.LeaderboardButtonClicked(leaderboardStructure); 
        Debug.Log("Leaderoboard Click called");
    }
}
