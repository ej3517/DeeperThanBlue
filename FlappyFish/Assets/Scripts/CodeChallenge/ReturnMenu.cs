using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ReturnMenu : MonoBehaviour
{
    // Start is called before the first frame update

    public Transform Menu;

    Vector3 oldPos;
    bool clickable = true;
    public void Clicked()
    {
        oldPos = Menu.position;
        Menu.localPosition = new Vector3(0, 0, -100);
        clickable = false;
    }
    public void Close()
    {
        Menu.position = oldPos;
        clickable = true;
    }

    public void ReturnMainMenu()
    {
        Loader.Load(Loader.Scene.MainMenu);
    }
}
