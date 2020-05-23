using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{
    Material material;
    Vector2 offset;
    private const float X_VELOCITY = 0.02f;
    
    private void Awake()
    {
        material = GetComponent<Renderer>().material;
    }
    
    void Start()
    {
        offset = new Vector2(X_VELOCITY, 0);
    }

    void Update()
    {
        material.mainTextureOffset += offset * Time.deltaTime;
    }
}
