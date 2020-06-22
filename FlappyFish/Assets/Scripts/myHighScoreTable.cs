using MiniJSON;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class myHighScoreTable : MonoBehaviour
{
    public Transform entryTemplate;
    public Transform entryContainer;
    private List<HighscoreEntry> highscoreEntryList;
    private List<Transform> highscoreEntryTransformList;

    public void Awake()
    {
        entryTemplate.gameObject.SetActive(false);

        //Add_highscore(2);

        string jsonString = PlayerPrefs.GetString("highscoreTable");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);

        if (highscores == null)
        {
            return;
        }

        highscoreEntryTransformList = new List<Transform>();
        foreach (HighscoreEntry highscoreEntry in highscores.highscoreEntryList)
        {
            CreateHighscoreEntryTransform(highscoreEntry, entryContainer, highscoreEntryTransformList);
        }
    }

    public void ShowTable()
    {

    }

    private void CreateHighscoreEntryTransform(HighscoreEntry highscoreEntry, Transform container, List<Transform> transformList)
    {

        float templateHeight = 30f;

        Transform entryTransform = Instantiate(entryTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);
        entryTransform.gameObject.SetActive(true);

        int rank = transformList.Count + 1;
        string rankString;
        switch (rank)
        {
            default: rankString = rank + "th"; break;
            case 1: rankString = "1st"; break;
            case 2: rankString = "2nd"; break;
            case 3: rankString = "3rd"; break;
        }
        entryTransform.Find("posText").GetComponent<Text>().text = rankString;

        entryTransform.Find("scoreText").GetComponent<Text>().text = highscoreEntry.score.ToString();
        entryTransform.Find("dateText").GetComponent<Text>().text = highscoreEntry.date;

        transformList.Add(entryTransform);

    }

    private void AddTohighscoreEntryList(ref List<HighscoreEntry> highscoreEntryList, int scoreIn, string dateIn)
    {

        if (highscoreEntryList == null)
        {
            Debug.Log("sono qua");
            highscoreEntryList = new List<HighscoreEntry>();
        }

        // don't add if the score is not in the top 10
        if (highscoreEntryList.Count >= 10 && scoreIn < highscoreEntryList[highscoreEntryList.Count - 1].score)
        {
            return;
        }

        // if the list has less then 10 scores, add it
        if (highscoreEntryList.Count < 10)
        {
            HighscoreEntry newEntry = new HighscoreEntry { score = scoreIn, date = dateIn };
            highscoreEntryList.Add(newEntry);
        }

        // new high score, replace the lowest
        else
        {
            highscoreEntryList[highscoreEntryList.Count - 1].score = scoreIn;
            highscoreEntryList[highscoreEntryList.Count - 1].date = dateIn;
        }

        // order list
        for (int i = 0; i < highscoreEntryList.Count; i++)
        {
            for (int j = i + 1; j < highscoreEntryList.Count; j++)
            {
                if (highscoreEntryList[j].score >= highscoreEntryList[i].score)
                {
                    HighscoreEntry tmp = highscoreEntryList[i];
                    highscoreEntryList[i] = highscoreEntryList[j];
                    highscoreEntryList[j] = tmp;
                }
            }
        }

    }

    public static void Add_highscore(int scoreIn)
    {

        // ** Load saved highscores
        string jsonString = PlayerPrefs.GetString("highscoreTable");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);


        if (highscores == null)
        {
            List<HighscoreEntry> highscoreEntryList = new List<HighscoreEntry>();
            highscores = new Highscores { highscoreEntryList = highscoreEntryList };
        }

        // ** Add new entry
        // don't add if the score is not in the top 10
        if (highscores.highscoreEntryList.Count >= 10 && scoreIn < highscores.highscoreEntryList[highscores.highscoreEntryList.Count - 1].score)
        {
            return;
        }

        string theTime = System.DateTime.Now.ToString("hh:mm");
        string theDate = System.DateTime.Now.ToString("dd/MM/yyyy");
        string dateIn = theDate + " " + theTime;

        // if the list has less then 10 scores, add it
        if (highscores.highscoreEntryList.Count < 10)
        {
            HighscoreEntry newEntry = new HighscoreEntry { score = scoreIn, date = dateIn };
            highscores.highscoreEntryList.Add(newEntry);
        }

        // new high score, replace the lowest
        else
        {
            highscores.highscoreEntryList[highscores.highscoreEntryList.Count - 1].score = scoreIn;
            highscores.highscoreEntryList[highscores.highscoreEntryList.Count - 1].date = dateIn;
        }

        // order list
        for (int i = 0; i < highscores.highscoreEntryList.Count; i++)
        {
            for (int j = i + 1; j < highscores.highscoreEntryList.Count; j++)
            {
                if (highscores.highscoreEntryList[j].score > highscores.highscoreEntryList[i].score)
                {
                    HighscoreEntry tmp = highscores.highscoreEntryList[i];
                    highscores.highscoreEntryList[i] = highscores.highscoreEntryList[j];
                    highscores.highscoreEntryList[j] = tmp;
                }
            }
        }

        // **  Save updated highscores
        string json = JsonUtility.ToJson(highscores);
        PlayerPrefs.SetString("highscoreTable", json);
        PlayerPrefs.Save();
    }

    public static void InstantiateHighscores()
    {
        PlayerPrefs.SetString("highscoreTable", JsonUtility.ToJson(new myHighScoreTable.Highscores()));
        PlayerPrefs.Save();
    }

    private class Highscores
    {
        public List<HighscoreEntry> highscoreEntryList;
    }

    [System.Serializable]
    private class HighscoreEntry
    {
        public int score;
        public string date;
    }
}