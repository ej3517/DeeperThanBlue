using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModuleController : MonoBehaviour
{
    public SimpleObjectPool moduleButtonObjectPool;
    public Transform moduleButtonParent;
    
    private DataController dataController;
    private RoundData[] allModules;
    private LeaderboardStructure[] allLeaderboards; 
    // private RoundData currentRoundData;
    
    private List<GameObject> moduleButtonGameObjects = new List<GameObject>();


    void Start()
    {
        dataController = FindObjectOfType<DataController>();
        allModules = dataController.GetCurrentAllRounds();
        allLeaderboards = dataController.GetCurrentAllLeaderboards(); 

        Debug.Log("the number of modules : " + allModules.Length.ToString());
        Debug.Log("the first module is : " + allModules[0].module);
        ShowModules();
    }
    
    private void ShowModules()
    {
        RemoveModuleButton();
        for (int i = 0; i < allModules.Length; i++)
        {
            GameObject moduleButtonGameObject = moduleButtonObjectPool.GetObject();
            ModuleButton moduleButton = moduleButtonGameObject.GetComponent<ModuleButton>();
            moduleButton.SetUp(allModules[i]);
            moduleButton.SetUpLeaderboard(allLeaderboards[i]); 
            moduleButtonGameObject.transform.SetParent(moduleButtonParent, false);
            moduleButtonGameObjects.Add(moduleButtonGameObject);
        }
    }

    
    private void RemoveModuleButton()
    {
        while (moduleButtonGameObjects.Count > 0)
        {
            moduleButtonObjectPool.ReturnObject(moduleButtonGameObjects[0]);
            moduleButtonGameObjects.RemoveAt(0);
        }
    }

    public void ModuleButtonClicked(RoundData roundDataWanted)
    {
        dataController.NewQuestionSetWanted(roundDataWanted);
        SoundManager.PlaySound(SoundManager.Sound.ButtonClick);
        Loader.Load(Loader.Scene.GameScene);
    }

    public void LeaderboardButtonClicked(LeaderboardStructure leaderboardDataWanted)
    {
        dataController.NewLeaderboardWanted(leaderboardDataWanted); 
        SoundManager.PlaySound(SoundManager.Sound.ButtonClick);
    }
}
