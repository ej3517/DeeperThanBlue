using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class profileButtonHandler : MonoBehaviour
{
    public GameObject ProfileCanvas_highest_scores;
    public GameObject ProfileCanvas_statistics;
    public GameObject ProfileCanvas_reset;

    private void Awake() {

        ProfileCanvas_statistics.SetActive(false);
        ProfileCanvas_reset.SetActive(false);

        ProfileCanvas_highest_scores.SetActive(true);

    }

    public void highest_scoresButton(){
        
        //SoundManager.PlaySound(SoundManager.Sound.ButtonClick);
        
        if(ProfileCanvas_statistics.activeSelf){
            ProfileCanvas_statistics.SetActive(false);
        }

        if(ProfileCanvas_highest_scores.activeSelf){
            ProfileCanvas_highest_scores.SetActive(true);
            // wait;
        }
        else {
            ProfileCanvas_highest_scores.SetActive(true);
        }

    }

    public void statisticsButton(){

        //SoundManager.PlaySound(SoundManager.Sound.ButtonClick);

        if(ProfileCanvas_highest_scores.activeSelf){
            ProfileCanvas_highest_scores.SetActive(false);
        }

        if(ProfileCanvas_statistics.activeSelf){
            ProfileCanvas_statistics.SetActive(true);
            // wait;
        }
        else {
            ProfileCanvas_statistics.SetActive(true);
        }
    }

    public void tomenuButton(){
        // SoundManager.PlaySound(SoundManager.Sound.ButtonClick);
        Loader.Load(Loader.Scene.MainMenu);
    }

    public void resetHighscoresButton(){   

        // SoundManager.PlaySound(SoundManager.Sound.ButtonClick);
        ProfileCanvas_reset.SetActive(true); 

    }


    public void resetButton(){

        // SoundManager.PlaySound(SoundManager.Sound.ButtonClick);

        string usernameString = PlayerPrefs.GetString("username");
        string passwordString = PlayerPrefs.GetString("password");

        PlayerPrefs.DeleteAll();

        PlayerPrefs.SetString("username", usernameString);
        PlayerPrefs.Save();
        PlayerPrefs.SetString("password", passwordString);
        PlayerPrefs.Save();

        PlayerPrefs.SetString("timesWon", 0.ToString());
        PlayerPrefs.Save();
        PlayerPrefs.SetString("timesLost", 0.ToString());
        PlayerPrefs.Save();

        PlayerPrefs.SetString("timesRight", 0.ToString());
        PlayerPrefs.Save();
        PlayerPrefs.SetString("timesWrong", 0.ToString());
        PlayerPrefs.Save();

        Loader.Load(Loader.Scene.ProfileScene);

    }

    public void noresetButton(){

        // SoundManager.PlaySound(SoundManager.Sound.ButtonClick);
        ProfileCanvas_reset.SetActive(false);

    }
}