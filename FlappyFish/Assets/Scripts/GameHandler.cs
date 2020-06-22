using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameHandler : MonoBehaviour
{
    private void Start()
    {
        Debug.Log("GameHandler.Start");
        
        Score.Start();

        if (SceneManager.GetActiveScene().name == "GameScene")
        {
            SoundManager.PlaySound(SoundManager.Sound.Background);
        }
    }
}
