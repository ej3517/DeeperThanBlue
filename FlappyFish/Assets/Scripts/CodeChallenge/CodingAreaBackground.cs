using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CodingAreaBackground : MonoBehaviour
{
    public float speed = 0.5f;
    
    private Image img;
    void Start()
    {
        img = transform.GetComponent<Image>();
    }

    float maxRange = 0.3f;
    float minRange = 0.1f;
    // Update is called once per frame
    void Update()
    {
        Debug.Log(img.color);
    }
}
