using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CallScene());
    }

    // Update is called once per frame
    IEnumerator CallScene()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(Loader.Scene.LoginScene.ToString());
    }
}
