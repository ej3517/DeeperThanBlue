using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using System.Net.Http;  
using System.Net;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text; 
using System.IO;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System.Linq;
using System; 

public class highScoreTable : MonoBehaviour
{
    private Transform entryContainer;
    private Transform entryTemplate; 
    private DataController dataController; 
    private List<HighscoreEntry> highscoreEntryList; 
    private List<Transform> highscoreEntryTransformList;  

    private List<Leaderboard> leaderboardContainer; 
    private void Awake()
    {
        dataController = FindObjectOfType<DataController>();

        entryContainer = transform.Find("highScoreContainer");
        entryTemplate = entryContainer.Find("highScoreEntryTemplate");

        entryTemplate.gameObject.SetActive(false); 

        
        DataController script = GameObject.Find("DataController").GetComponent<DataController>(); 
        //Debug.Log(script.leaderboardList); 

        highscoreEntryList = new List<HighscoreEntry>() {
            new HighscoreEntry { score = 124, name = "AAA" }, 
            new HighscoreEntry { score = 122, name = "AAA" }, 
            new HighscoreEntry { score = 12, name = "AAA" }, 
            new HighscoreEntry { score = 124564, name = "AAA" }, 
            new HighscoreEntry { score = 124, name = "AAA" }, 
            new HighscoreEntry { score = 124, name = "AAA" }, 
            new HighscoreEntry { score = 124, name = "AAA" }, 
            new HighscoreEntry { score = 124, name = "AAA" }, 
            new HighscoreEntry { score = 124, name = "AAA" }, 
            new HighscoreEntry { score = 124, name = "AAA" }, 
        }; 

        // Sort table 
        for (int i = 0; i < highscoreEntryList.Count; i++) {
            for (int j = 0; j < highscoreEntryList.Count; j++) {
                if (highscoreEntryList[j].score < highscoreEntryList[i].score) {
                    // swap 
                    HighscoreEntry tmp = highscoreEntryList[i];
                    highscoreEntryList[i] = highscoreEntryList[j]; 
                    highscoreEntryList[j] = tmp;  
                }
            }
        }

        highscoreEntryTransformList = new List<Transform>();

        foreach (HighscoreEntry highscoreEntry in highscoreEntryList) {
            CreateHighscoreEntryTransform(highscoreEntry, entryContainer, highscoreEntryTransformList); 
        }

        string json = JsonUtility.ToJson(highscoreEntryList); 
        PlayerPrefs.SetString("highscoreTable", "100");


        // Set example user Player Prefs 
        PlayerPrefs.SetString("username", "jmpb1997"); 
        PlayerPrefs.SetString("password", "hello"); 
        
        // ** Load saved highscores
        string username = PlayerPrefs.GetString("username");
        string password = PlayerPrefs.GetString("password"); 


    }

    
    private void CreateModuleButton(List<Leaderboard> leaderboardList) {

    }
    private void CreateHighscoreEntryTransform(HighscoreEntry highscoreEntry, Transform container, List<Transform> transformList) {
            
        float templateHeight = 7.2f; 

        Transform entryTransform = Instantiate(entryTemplate, container); 
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>(); 
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count); 
        entryTransform.gameObject.SetActive(true); 
        
        int rank = transformList.Count + 1; 

        string rankString; 
        
        switch(rank) {
            default: rankString = rank + "TH"; break; 
            case 1: rankString = "1ST"; break; 
            case 2: rankString = "2ND"; break; 
            case 3: rankString = "3RD"; break; 
        }
        
        entryTransform.Find("posText (1)").GetComponent<Text>().text = rankString;
            
        int score = highscoreEntry.score; 

        entryTransform.Find("scoreText (1)").GetComponent<UnityEngine.UI.Text>().text = score.ToString(); 

        string name = highscoreEntry.name; 
        entryTransform.Find("playerText (1)").GetComponent<UnityEngine.UI.Text>().text = name; 

        transformList.Add(entryTransform);
    }

    [System.Serializable]
    private class HighscoreEntry {
        public int score; 
        public string name; 
    }

    private class LeaderboardSelector {
        public string school; 
        //public string _class; 

    }


    public class User {
        public string _id; 
        public string score; 
        public string _class; 
        public string school; 
    }

}
