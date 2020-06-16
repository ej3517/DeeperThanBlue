using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingMenu : MonoBehaviour
{
    public Transform codingArea;
    public Transform bin;

    private Vector3 oldPos = new Vector3(4000, 0, -100);
    public void Close()
    {
        transform.position = oldPos;
        bin.GetComponent<Bin>().ConfirmTrue();
        codingArea.GetComponent<CodingArea>().Restart();
        transform.localPosition = oldPos;
    }

    public void ReturnMainMenu()
    {
        Loader.Load(Loader.Scene.MainMenu);
    }
}
