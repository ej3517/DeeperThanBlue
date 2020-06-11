using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using CloudantInterface; 
using System.Threading.Tasks;

public class DataController : MonoBehaviour
{
    public RoundData[] allRoundData;
    public Interface interfaceLink; 
    // Start is called before the first frame update
    void Start()
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
        var classesJson = interfaceLink.GetSignUpClasses(gameUser).ConfigureAwait(true).GetAwaiter().GetResult();
        Debug.Log(classesJson); 


        // Fetch leaderboard from each corresponding class 

        // Fetch questions from each signed up module 

        Loader.Load(Loader.Scene.MainMenu);

    }

    public RoundData GetCurrentRoundData()
    {
        return allRoundData[0];
    }
}
