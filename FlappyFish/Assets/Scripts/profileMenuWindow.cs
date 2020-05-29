using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class profileMenuWindow : MonoBehaviour
{
    public GameObject ProfileCanvas_highest_scores;
    int counter = 0;
    private void Awake() {


    }

    public void highest_scoresButton()
    {
        //SoundManager.PlaySound(SoundManager.Sound.ButtonClick);
        counter++;
        //Loader.Load(Loader.Scene.ProfileScene);
        if(counter%2 == 0){
            ProfileCanvas_highest_scores.gameObject.SetActive(false);
        }
        else {
            ProfileCanvas_highest_scores.gameObject.SetActive(true);
        }

    }
}
