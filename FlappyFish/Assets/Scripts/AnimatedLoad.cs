using NSubstitute.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimatedLoad : MonoBehaviour
{
    // Start is called before the first frame update
    Text t;
    void Awake()
    {
      
        t = transform.GetComponent<Text>();
        StartCoroutine(CallScene(0));
    }

    // Update is called once per frame
    IEnumerator CallScene(int i)
    {
        switch (i)
        {
            case 0:
                t.text = "LOADING";
                break;
            case 1:
                t.text = "LOADING.";
                break;
            case 2:
                t.text = "LOADING..";
                break;
            case 3:
                t.text = "LOADING...";
                break;
            default:
                i = -1;
                break;
        }
        yield return new WaitForSeconds(0.25f);
        i++;
        StartCoroutine(CallScene(i));
    }
}
