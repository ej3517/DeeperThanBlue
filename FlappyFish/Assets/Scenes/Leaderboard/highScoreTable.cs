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
using System.Text.Json; 
using System.Text.Json.Serialization; 
using System.Threading.Tasks;
using System.Linq;
using System; 

public class highScoreTable : MonoBehaviour
{
    private Transform entryContainer;
    private Transform entryTemplate; 

    private List<HighscoreEntry> highscoreEntryList; 
    private List<Transform> highscoreEntryTransformList;  

    private void Awake()
    {
        entryContainer = transform.Find("highScoreContainer");
        entryTemplate = entryContainer.Find("highScoreEntryTemplate");

        entryTemplate.gameObject.SetActive(false); 

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
        PlayerPrefs.SetString("username", "363ee3321fe445453276df76481dde64"); 
        PlayerPrefs.SetString("password", "mor"); 
        
        // ** Load saved highscores
        string username = PlayerPrefs.GetString("username");
        string password = PlayerPrefs.GetString("password"); 

        // Store in object PlayerPrefs components 
        /*UserQuery user = new UserQuery(); 
        user.Selector.Id.Eq = username; 
        user.Selector.Password.Eq = password;*/

        //var jsonTry = JsonConvert.SerialiazeObject(new {selector = user}); 
        //Debug.Log(jsonTry);  

        // Serialize object 
        //string content = System.Text.Json.JsonSerializer.Serialiaze<UserQuery>(user); 
        //Debug.Log(content);

        //Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);

        // ...

        // **  Save updated highscores
        /*string json = JsonUtility.ToJson(highscores);
        PlayerPrefs.SetString("highscoreTable", json);
        PlayerPrefs.Save(); */

        Debug.Log("Before Function"); 
        //GetAllDocs(); 
        Task<string> leaderboard = GetUserLeaderboard();

    }

    private async void GetAllDocs() {
        
        Debug.Log("Hello World!");
        var client = new HttpClient();
        
        // Set header authorization 
        client.DefaultRequestHeaders.Accept.Clear();
        //client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.MediaTypeWithQualityHeaderValue("application/json"));
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", "NWEzOTVkNDktZmJlMy00ZDdkLWE5OTktMjlhMTQ2MzkzMmYxLWJsdWVtaXg6NjFlYTc2M2Y0YzIzNDc0YjYzMjgyNTlkZjI0ZmY3NGU2YWE4MmZmMTU0OTFhZWQ4M2U5ZGE5YWI5OWEzMjU4NQ==");
        //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", "NWEzOTVkNDktZmJlMy00ZDdkLWE5OTktMjlhMTQ2MzkzMmYxLWJsdWVtaXg6NjFlYTc2M2Y0YzIzNDc0YjYzMjgyNTlkZjI0ZmY3NGU2YWE4MmZmMTU0OTFhZWQ4M2U5ZGE5YWI5OWEzMjU4NQ==");

        // Call asynchronous network methods in a try/catch block to handle exceptions.
        try	
        {
            HttpResponseMessage response = await client.GetAsync("https://5a395d49-fbe3-4d7d-a999-29a1463932f1-bluemix.cloudant.com/mydb/_all_docs");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            // Above three lines can be replaced with new helper method below
            // string responseBody = await client.GetStringAsync(uri);

            Debug.Log("Hi mate! " + responseBody);
        }
        catch(HttpRequestException e)
        {
            Debug.Log("\nException Caught!");	
            Debug.Log("Message : " + e.Message);
        }
        
    }

    // Query User Classmates Scores 
    private static async Task<string> GetUserLeaderboard() {
    
        string jsonFile; 
        // read JSON directly from a file
        using (StreamReader r = new StreamReader(@"Assets/Scenes/Leaderboard/selector.json"))
        {
            jsonFile = r.ReadToEnd();
            Debug.Log(jsonFile); 
            //List<Item> items = JsonConvert.DeserializeObject<List<Item>>(json);
        }

        var client = new HttpClient();
        
        // Set header authorization 
        client.DefaultRequestHeaders.Accept.Clear();
        //client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.MediaTypeWithQualityHeaderValue("application/json"));
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", "NWEzOTVkNDktZmJlMy00ZDdkLWE5OTktMjlhMTQ2MzkzMmYxLWJsdWVtaXg6NjFlYTc2M2Y0YzIzNDc0YjYzMjgyNTlkZjI0ZmY3NGU2YWE4MmZmMTU0OTFhZWQ4M2U5ZGE5YWI5OWEzMjU4NQ==");
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));//ACCEPT header
        
        //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", "NWEzOTVkNDktZmJlMy00ZDdkLWE5OTktMjlhMTQ2MzkzMmYxLWJsdWVtaXg6NjFlYTc2M2Y0YzIzNDc0YjYzMjgyNTlkZjI0ZmY3NGU2YWE4MmZmMTU0OTFhZWQ4M2U5ZGE5YWI5OWEzMjU4NQ==");

        // Define query object
        LeaderboardSelector myObject = new LeaderboardSelector(); 
        //myObject._class = "EEE"; 
        myObject.school = "ICL"; 

        // Make it a json 
        string JsonData = JsonConvert.SerializeObject(myObject); 
        Debug.Log(JsonData);  

        string username = PlayerPrefs.GetString("username");
        string payload = "{\"selector\":{\"school\":{\"$eq\":\"ICL\"}, \"_id\":{\"$eq\":\"" + username +  "\"}}}";
        //var payload = jsonFile; 
        // Feed it as content
        Debug.Log(payload);
        HttpContent content = new StringContent(payload, Encoding.UTF8, "application/json");

        Debug.Log(content); 

        // Call asynchronous network methods in a try/catch block to handle exceptions.
        try	
        {
            HttpResponseMessage response = await client.PostAsync("https://5a395d49-fbe3-4d7d-a999-29a1463932f1-bluemix.cloudant.com/mydb/_find", content);
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            // Above three lines can be replaced with new helper method below
            // string responseBody = await client.GetStringAsync(uri);

            Debug.Log(responseBody);
            return responseBody; 
        }
        catch(HttpRequestException e)
        {
            Debug.Log("\nException Caught!");	
            Debug.Log("Message : " + e.Message);
            return e.Message;
        }
    }


    // Insert data into DB 
    private async void Score() {

        // Fetch User Prefs 
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


    public class UserQuery
    {
        [JsonProperty("selector")]
        public Selector Selector { get; set; }
    }

    public class Selector
    {
        [JsonProperty("_id")]
        public Id Id { get; set; }

        [JsonProperty("password")]
        public Id Password { get; set; }
    }

    public class Id
    {
        [JsonProperty("$eq")]
        public string Eq { get; set; }
    }

}
