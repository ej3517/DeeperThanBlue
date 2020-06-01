using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class profileButtonHandler : MonoBehaviour
{
    public GameObject ProfileCanvas_highest_scores;
    public GameObject ProfileCanvas_wrong_questions;
    public GameObject ProfileCanvas_statistics;

    int counter1 = 0;
    int counter2 = 0;
    int counter3 = 0;
    private void Awake() {
        ProfileCanvas_highest_scores.SetActive(false);
        ProfileCanvas_wrong_questions.SetActive(false);
        ProfileCanvas_statistics.SetActive(false);
    }

    public void highest_scoresButton()
    {
        //SoundManager.PlaySound(SoundManager.Sound.ButtonClick);
        counter1++;
        //Loader.Load(Loader.Scene.ProfileScene);
        if(counter1%2 == 0){
            ProfileCanvas_highest_scores.SetActive(false);
        }
        else {
            ProfileCanvas_highest_scores.SetActive(true);
            //if one is active th others need to be closed + COUNTER
        }
    }

    public void statisticsButton()
    {
        //SoundManager.PlaySound(SoundManager.Sound.ButtonClick);
        counter2++;
        //Loader.Load(Loader.Scene.ProfileScene);
        if(counter2%2 == 0){
            ProfileCanvas_statistics.SetActive(false);
        }
        else {
            ProfileCanvas_statistics.SetActive(true);
            //if one is active th others need to be closed + COUNTER
        }
    }

        public void wrong_questionsButton()
    {
        //SoundManager.PlaySound(SoundManager.Sound.ButtonClick);
        counter3++;
        //Loader.Load(Loader.Scene.ProfileScene);
        if(counter3%2 == 0){
            ProfileCanvas_wrong_questions.SetActive(false);
        }
        else {
            ProfileCanvas_wrong_questions.SetActive(true);
            //if one is active th others need to be closed
        }
    }

    //theTime = System.DateTime.Now.ToString("hh:mm:ss");
    //theDate = System.DateTime.Now.ToString("MM/dd/yyyy");
    //theMonth = System.DateTime.Now.get_Month();
    //theDay = System.DateTime.Now.get_Day();
}