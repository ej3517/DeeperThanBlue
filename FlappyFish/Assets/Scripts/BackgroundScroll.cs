using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{
    Material material;
    Vector2 offset;
    private float xVelocity = 0.06f;
    
    private void Awake()
    {
        material = GetComponent<Renderer>().material;
    }
    
    void Start()
    {
        offset = new Vector2(xVelocity, 0);
    }

    void Update()
    {
        material.mainTextureOffset += offset * Time.deltaTime;
    }
}
