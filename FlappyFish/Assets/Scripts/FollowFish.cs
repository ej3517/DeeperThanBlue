using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowFish : MonoBehaviour
{


    public Transform targetTransform;
    private Vector3 tempVec3 = new Vector3();

    
    public float diff; 
    // Start is called before the first frame update
    void Start()
    {
 
    }

    // Update is called once per frame

    void LateUpdate() {
        tempVec3.x = targetTransform.position.x;
        tempVec3.y = this.transform.position.y;
        tempVec3.z = this.transform.position.z;

        diff = this.transform.position.x - targetTransform.position.x; 
        
        this.transform.position = tempVec3;
    }
}
