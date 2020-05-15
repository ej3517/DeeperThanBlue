using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDiamond : MonoBehaviour
{

    Level levelScript; 
 
    // Start is called before the first frame update
    void Start()
    {   
        levelScript = GameObject.Find("Level").GetComponent<Level>(); 
    }

    // Update is called once per frame
    void Update()
    {
        if (levelScript.birdSpeed > 30){
            levelScript.birdSpeed -= 0.5f; 
        }
    }

    
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name == "Bird")
        {
            gameObject.active = false; 
            levelScript.birdSpeed = 60; 
        }

    }
}
