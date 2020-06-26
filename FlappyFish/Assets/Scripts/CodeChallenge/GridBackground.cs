using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GridBackground : MonoBehaviour
{
    public float speed = 2f;

    private SpriteRenderer img;
    void Start()
    {
        img = transform.GetComponent<SpriteRenderer>();
        img.color = new Color(0.142f, 0.5182321f, 1);
    }

    float maxRange = 0.85f;
    float minRange = 0.32f;
    bool up = true;
    bool pause = false;
    public void SetPause(bool state)
    {
        pause = state;
    }
    // Update is called once per frame
    void Update()
    {
        if(pause)
        {
            return;
        }
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
    }
}
