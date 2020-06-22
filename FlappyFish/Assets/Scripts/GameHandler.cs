using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameHandler : MonoBehaviour
{
    private string previousScene; 

    private void Start()
    {
        Debug.Log("GameHandler.Start");
        
        Score.Start();

        if (SceneManager.GetActiveScene().name == "GameScene")
        {
            SoundManager.PlaySound(SoundManager.Sound.Background);
        }
    }

    private void Update()
    {
        // Check if scene changed 
        if (SceneManager.GetActiveScene().name == previousScene)
        {
            // Do nothing
        } else if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            
            // Update the highscore from player
        }
    }
}
