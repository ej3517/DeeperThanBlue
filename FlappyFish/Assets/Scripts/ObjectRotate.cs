using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotate : MonoBehaviour
{
    Transform obj;
    private void Start()
    {
        obj = GetComponent<Transform>();
    }
    private void FixedUpdate()
    {
        float angleOld = obj.rotation.y;
        float angleNew = angleOld + 1f;
        if(angleNew>365)
        {
            angleNew -= 365;
        }
        obj.Rotate(0f, angleNew, 0.0f);
    }
    
}
