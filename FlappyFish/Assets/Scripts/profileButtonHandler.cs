using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class profileButtonHandler : MonoBehaviour
{
    public GameObject ProfileCanvas_highest_scores;
    public GameObject ProfileCanvas_wrong_questions;
    public GameObject ProfileCanvas_statistics;

    private void Awake() {
        ProfileCanvas_highest_scores.SetActive(true);
        ProfileCanvas_wrong_questions.SetActive(false);
        ProfileCanvas_statistics.SetActive(false);
    }

    public void highest_scoresButton(){
        
        //SoundManager.PlaySound(SoundManager.Sound.ButtonClick);
        
        if(ProfileCanvas_statistics.activeSelf){
            ProfileCanvas_statistics.SetActive(false);
        }

        if(ProfileCanvas_wrong_questions.activeSelf){
            ProfileCanvas_wrong_questions.SetActive(false);
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

        if(ProfileCanvas_wrong_questions.activeSelf){
            ProfileCanvas_wrong_questions.SetActive(false);
        }

        if(ProfileCanvas_statistics.activeSelf){
            ProfileCanvas_statistics.SetActive(true);
            // wait;
        }
        else {
            ProfileCanvas_statistics.SetActive(true);
        }
    }

    public void wrong_questionsButton(){

        // SoundManager.PlaySound(SoundManager.Sound.ButtonClick);

        if(ProfileCanvas_highest_scores.activeSelf){
            ProfileCanvas_highest_scores.SetActive(false);
        }

        if(ProfileCanvas_statistics.activeSelf){
            ProfileCanvas_statistics.SetActive(false);
        }

        if(ProfileCanvas_wrong_questions.activeSelf){
            ProfileCanvas_wrong_questions.SetActive(true);
            // wait;
        }
        else {
            ProfileCanvas_wrong_questions.SetActive(true);
        }

    }

    public void tomenuButton(){
        // SoundManager.PlaySound(SoundManager.Sound.ButtonClick);
        Loader.Load(Loader.Scene.MainMenu);
    }

    public void resetHighscoresButton(){
        // SoundManager.PlaySound(SoundManager.Sound.ButtonClick);
        PlayerPrefs.DeleteAll();

        PlayerPrefs.SetString("timesWon", 0.ToString());
        PlayerPrefs.Save();
        PlayerPrefs.SetString("timesLost", 0.ToString());
        PlayerPrefs.Save();
        
        Loader.Load(Loader.Scene.ProfileScene);
    }
}