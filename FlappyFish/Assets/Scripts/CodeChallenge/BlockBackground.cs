using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class BlockBackground : MonoBehaviour
{
    public float speed = 5f;

    private Image img;
    void Start()
    {
        img = transform.GetComponent<Image>();
        img.color = Color.HSVToRGB(244,74,100);
    }

    float maxRange = 93f;
    float minRange = 62f;
    bool up = true;
    // Update is called once per frame
    void Update()
    {
        Color.RGBToHSV(img.color, out float H, out float S, out float V);
        if (S > maxRange)
        {
            up = false;
        }
        else if (S < minRange)
        {
            up = true;
        }

        float rnd = 0;// UnityEngine.Random.Range(-1, 1);
        if (up)
        {
            img.color = Color.HSVToRGB(H, S + speed/100 + rnd / 100000, V);
        }
        else
        {
            img.color = Color.HSVToRGB(H, S - speed/100 + rnd / 100000, V);
        }
        Debug.Log(img.color);
    }
}
