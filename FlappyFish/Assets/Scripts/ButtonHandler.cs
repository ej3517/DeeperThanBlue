using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHandler : MonoBehaviour
{
    public void PlayButton()
    {
        SoundManager.PlaySound(SoundManager.Sound.ButtonClick);
        Loader.Load(Loader.Scene.GameScene);
    }
    
    public void MainMenuButton()
    {
        SoundManager.PlaySound(SoundManager.Sound.ButtonClick);
        Loader.Load(Loader.Scene.MainMenu);
    }

    public void profileButton()
    {
        SoundManager.PlaySound(SoundManager.Sound.ButtonClick);
        Loader.Load(Loader.Scene.ProfileScene);
    }

    public void CodingGameButton()
    {
        SoundManager.PlaySound(SoundManager.Sound.ButtonClick);
        Loader.Load(Loader.Scene.CodingGame);
    }

    public void LeaderBoardButton()
    {
        SoundManager.PlaySound(SoundManager.Sound.ButtonClick);
        Loader.Load(Loader.Scene.LeaderboardScene);
    }

    public void CloseGameButton()
    {
        SoundManager.PlaySound(SoundManager.Sound.ButtonClick);
        Debug.Log("Application Closed - This only works in Release mode");
        Application.Quit();
    }

    public void ToTheWebsite()
    {
        Application.OpenURL("http://flappyfish.eu-gb.cf.appdomain.cloud");
    }
}
