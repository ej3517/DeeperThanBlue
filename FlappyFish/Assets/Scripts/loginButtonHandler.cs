using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class loginButtonHandler : MonoBehaviour
{

    public GameObject usernameInputFieldText;
    public GameObject schoolInputFieldText;
    string theUsername;
    string thePassword;
    public GameObject usernameWindow;
    public GameObject schoolWindow;
    private void Awake() {

    }

    public void usernameConfirm(){
        // SoundManager.PlaySound(SoundManager.Sound.ButtonClick);
        
        // Save username inputs
        theUsername = usernameInputFieldText.GetComponent<Text>().text;
        Debug.Log("the username is " + theUsername); 
        PlayerPrefs.SetString("username", theUsername);
        PlayerPrefs.Save();
        // SAVE HERE 

        // Hide username window 
        usernameWindow.SetActive(false);

        // Show school window
        schoolWindow.SetActive(true);
    }

    public void passwordConfirm(){
        // SoundManager.PlaySound(SoundManager.Sound.ButtonClick);

        // Save school inputs 
        thePassword = schoolInputFieldText.GetComponent<Text>().text;
        Debug.Log("the password is " + thePassword);
        PlayerPrefs.SetString("password", thePassword);
        PlayerPrefs.Save();
        // CHECK PASSWORD

        // Load main menu scene
        Loader.Load(Loader.Scene.Persistent);
    }

    public void tomenuButton(){
        // SoundManager.PlaySound(SoundManager.Sound.ButtonClick);
        Loader.Load(Loader.Scene.LoginScene);
    }
}
