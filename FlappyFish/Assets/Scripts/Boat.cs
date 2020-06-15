using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boat : MonoBehaviour
{
    // SPEED
    public float boatSpeed;
    private float relativeSpeedOfBoatWrtFish;
    private bool isWaiting;
    // GLOBAL VAR
    private Rigidbody2D boatRigidBody2D;
    private Transform boatTransform;
    
    // State
    private StateController stateControllerScript;

    private void Awake()
    {
        // RigidBody
        boatRigidBody2D = GetComponent<Rigidbody2D>();
        boatRigidBody2D.bodyType = RigidbodyType2D.Static;
        // Transform
        boatTransform = GetComponent<Transform>();
        // Speed
        boatSpeed = 35f;
        isWaiting = false;
        stateControllerScript = GameObject.Find("StateController").GetComponent<StateController>();
    }
    
    private void Update()
    {
        switch (stateControllerScript.currentState)
        {
            case StateController.State.Playing:
                relativeSpeedOfBoatWrtFish = boatSpeed - Level.GetInstance().birdSpeed;
                HandleBoatWait();
                if (!isWaiting)
                {
                    boatRigidBody2D.bodyType = RigidbodyType2D.Dynamic; 
                    Move(relativeSpeedOfBoatWrtFish);
                }
                else if (isWaiting && relativeSpeedOfBoatWrtFish > 0)
                {
                    boatRigidBody2D.bodyType = RigidbodyType2D.Dynamic;
                    Move(relativeSpeedOfBoatWrtFish);
                    isWaiting = false;
                }
                break;
            case StateController.State.WaitingAnswer:
                boatRigidBody2D.bodyType = RigidbodyType2D.Static;
                break;
            case StateController.State.Dead:
                break;
            case StateController.State.Won:
                break;
        }
    }

    private void Move(float speed)
    {
        boatRigidBody2D.velocity = Vector2.right * speed;
    }

    private void HandleBoatWait()
    {
        if (boatTransform.position.x < -110f)
        {
            isWaiting = true;
            boatRigidBody2D.bodyType = RigidbodyType2D.Static; 
        }
    }
}
