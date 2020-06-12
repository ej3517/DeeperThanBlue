using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using CloudantInterface; 
using System.Threading.Tasks;
using Newtonsoft.Json; 
using System.Threading;

public class DataController : MonoBehaviour
{
    public RoundData[] allRoundData;
    public Interface interfaceLink; 

    public List<Leaderboard> leaderboardList = new List<Leaderboard>(); 
    // Start is called before the first frame update
    async void Start()
    {
        Debug.Log("Start");

        DontDestroyOnLoad(gameObject);
        Loader.Load(Loader.Scene.MainMenu);

        // Get login 
        PlayerPrefs.SetString("username", "jmpb1997");
        string gameUser = PlayerPrefs.GetString("username"); 



        // Fetch class in which participates 
        // Blocked async function
        interfaceLink = new Interface();
        var classesJson = await interfaceLink.GetSignUpClasses(gameUser);
        UserSpecs specs = JsonConvert.DeserializeObject<UserSpecs>(classesJson); 
        
        Debug.Log(classesJson); 


        // Fetch leaderboard from each corresponding classTag 

        // Need to fetch leaderboard for each classTag user is on 
        // At every iteration we pick the score from N entry of score 
        foreach(string _class in specs.docs[0].classTag) {
            // fetch Leaderboard
            var leaderboardJson = await interfaceLink.GetLeaderboard(_class, specs.docs[0].school); 
            Thread.Sleep(1100); 
            Leaderboard leaderboard = JsonConvert.DeserializeObject<Leaderboard>(leaderboardJson); 
            leaderboardList.Add(leaderboard); 
        }
     


        foreach (string _class in specs.docs[0].classTag) {
            // fetch Question
            var questionJson = await interfaceLink.GetQuestions(_class, specs.docs[0].school); 
            Thread.Sleep(1100); 
            Debug.Log(questionJson); 
        }
        


        // Fetch questions from each signed up module 

        Loader.Load(Loader.Scene.MainMenu);

    }

    public RoundData GetCurrentRoundData()
    {
        return allRoundData[0];
    }


    // Students specs -- school in which he studies and classes 
    public class UserSpecs
    {
        public IList<StudyItem> docs { get; set; }
    }

    public class StudyItem
    {
        public IList<string> classTag { get; set; } 
        public string school { get; set; }
    }
}
