using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHandler : MonoBehaviour
{
    public class RestartButton : MonoBehaviour
    {
        public void restartButton()
        {
            Loader.Load(Loader.Scene.GameScene);
        }
    }
}
