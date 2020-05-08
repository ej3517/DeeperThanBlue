using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHandler : MonoBehaviour
{
    public void playButton()
    {
        Loader.Load(Loader.Scene.GameScene);
    }
    
    public void mainMenuButton()
    {
        Loader.Load(Loader.Scene.MainMenu);
    }
}
