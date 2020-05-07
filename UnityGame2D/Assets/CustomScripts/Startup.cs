using UnityEngine;
using UnityEditor;

using UnityEngine.SceneManagement;


public class Startup
{
    [RuntimeInitializeOnLoadMethod]
    static void OnRuntimeMethodLoad()
    {
        string startScene = "MainMenu";
        SceneManager.LoadScene(startScene);
        int c = SceneManager.sceneCount;
        for (int i = 0; i < c; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (scene.name != startScene)
            {
                SceneManager.UnloadSceneAsync(scene);
            }
        }
    }

}
