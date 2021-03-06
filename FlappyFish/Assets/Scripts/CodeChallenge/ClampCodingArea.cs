﻿using UnityEngine;
using UnityEngine.UI;

public class ClampCodingArea : MonoBehaviour
{
    ScrollRect scroll;

    private void Awake()
    {
        scroll = GetComponent<ScrollRect>();
    }


    private void Update()
    {
        if (transform.position.y < -0)
        {
            scroll.inertia = false;
            scroll.StopMovement();
            transform.position = new Vector3(transform.position.x, -0, transform.position.z);
            scroll.inertia = true;
        }
        else if (transform.position.y > 3120)
        {
            scroll.inertia = false;
            scroll.StopMovement();
            transform.position = new Vector3(transform.position.x, 3120, transform.position.z);
            scroll.inertia = true;
        }
    }
}
