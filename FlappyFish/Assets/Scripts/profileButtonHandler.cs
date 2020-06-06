using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class profileButtonHandler : MonoBehaviour
{
    public GameObject ProfileCanvas_highest_scores;
    public GameObject ProfileCanvas_wrong_questions;
    public GameObject ProfileCanvas_statistics;

    private static int counter1;
    private static int counter2;
    private static int counter3;
    private void Awake() {
        ProfileCanvas_highest_scores.SetActive(true);
        ProfileCanvas_wrong_questions.SetActive(false);
        ProfileCanvas_statistics.SetActive(false);
        counter1 = 0;
        counter2 = 0;
        counter3 = 0;
    }

    public void highest_scoresButton(){
        
        //SoundManager.PlaySound(SoundManager.Sound.ButtonClick);
        
        /*counter1++;
        if(counter1%2 == 0){
            ProfileCanvas_highest_scores.SetActive(false);
        }*/
        //else {
            if(ProfileCanvas_statistics.activeSelf){
                counter2++;
                ProfileCanvas_statistics.SetActive(false);
            }

            if(ProfileCanvas_wrong_questions.activeSelf){
                counter3++;
                ProfileCanvas_wrong_questions.SetActive(false);
            }

            if(ProfileCanvas_highest_scores.activeSelf){
                ProfileCanvas_highest_scores.SetActive(true);
                // wait;
                //System.Threading.Thread.Sleep(10000);

                //ProfileCanvas_highest_scores.SetActive(true);
            }
            else {
                counter2++;
                ProfileCanvas_highest_scores.SetActive(true);
            }
            //ProfileCanvas_highest_scores.SetActive(true);
        //}
    }

    public void statisticsButton(){

        //SoundManager.PlaySound(SoundManager.Sound.ButtonClick);

        counter2++;
        if(counter2%2 == 0){
            ProfileCanvas_statistics.SetActive(false);
        }
        else {
            if(ProfileCanvas_highest_scores.activeSelf){
                counter1++;
                ProfileCanvas_highest_scores.SetActive(false);
            }
            if(ProfileCanvas_wrong_questions.activeSelf){
                counter3++;
                ProfileCanvas_wrong_questions.SetActive(false);
            }
            ProfileCanvas_statistics.SetActive(true);
        }
    }

    public void wrong_questionsButton(){

        // SoundManager.PlaySound(SoundManager.Sound.ButtonClick);

        counter3++;
        if(counter3%2 == 0){
            ProfileCanvas_wrong_questions.SetActive(false);
        }
        else {
            if(ProfileCanvas_highest_scores.activeSelf){
                counter1++;
                ProfileCanvas_highest_scores.SetActive(false);
            }
            if(ProfileCanvas_statistics.activeSelf){
                counter2++;
                ProfileCanvas_statistics.SetActive(false);
            }
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
        Loader.Load(Loader.Scene.ProfileScene);
    }
}