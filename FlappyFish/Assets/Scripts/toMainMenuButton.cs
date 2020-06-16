using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class toMainMenuButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void toMainMenuScene(){
        // SoundManager.PlaySound(SoundManager.Sound.ButtonClick);
        Loader.Load(Loader.Scene.MainMenu);
    }
}
