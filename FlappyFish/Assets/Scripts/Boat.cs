using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boat : MonoBehaviour
{
    //CONSTANT
    private const float BOAT_SPEED_MULTIPLIER = 1.1f;
    // SPEED
    private float boatSpeed;
    private float relativeSpeedOfBoatWrtFish;
    private float boatSpeedTimer;
    private float boatSpeedTimerMax = 5f;
    // GLOBAL VAR
    private Rigidbody2D boatRigidBody;
    private static Boat instance;
    
    private void Awake()
    {
        boatRigidBody = GetComponent<Rigidbody2D>();
        boatSpeed = 10f;
        boatSpeedTimer = boatSpeedTimerMax;
    }

    private void Update()
    {
        HandleBoatSpeed();
        relativeSpeedOfBoatWrtFish = boatSpeed - Level.GetInstance().birdSpeed;
        Move(relativeSpeedOfBoatWrtFish);
    }

    private void Move(float speed)
    {
        boatRigidBody.velocity = Vector2.right * speed;
    }

    private void HandleBoatSpeed()
    {
        boatSpeedTimer -= Time.deltaTime;
        Debug.Log(boatSpeedTimer.ToString()); 
        if (boatSpeedTimer < 0)
        {
            boatSpeedTimer = boatSpeedTimerMax;
            boatSpeed *= BOAT_SPEED_MULTIPLIER;
        }
    }
}
