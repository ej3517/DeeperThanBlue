using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CodingAreaBackground : MonoBehaviour
{
    public float speed = 0.2f;

    private Image img;
    void Start()
    {
        img = transform.GetComponent<Image>();
    }

    float maxRange = 0.3f;
    float minRange = 0.05f;
    bool up = true;
    // Update is called once per frame
    void Update()
    {
        if (img.color.g > maxRange)
        {
            up = false;
        }
        else if (img.color.g < minRange)
        {
            up = true;
        }

        float rnd = 0;// UnityEngine.Random.Range(-1, 1);
        if (up)
        {
            img.color = new Color(img.color.r, img.color.g + speed / 1000 + rnd / 100000, img.color.b);
        }
        else
        {
            img.color = new Color(img.color.r, img.color.g - speed / 1000 + rnd / 100000, img.color.b);
        }

    }
}
