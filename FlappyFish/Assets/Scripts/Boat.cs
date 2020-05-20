using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;
using CodeMonkey.Utils;

public class Boat : MonoBehaviour
{
    // SPEED
    private float boatSpeed;
    private float relativeSpeedOfBoatWrtFish;
    private float boatSpeedTimer;
    private float boatSpeedTimerMax = 0.5f;
    private float boatSpeedMultiplier = 1.1f;
    // GLOBAL VAR
    private Rigidbody2D boatRigidBody2D;
    private Transform boatTransform;
    private static Boat instance;

    public static Boat GetInstance()
    {
        return instance;
    }
    
    // State
    private State state;
    private enum State
    {
        WaitingToStart,
        Catching,
        FallingBehind,
        Waiting,
        BirdDied
    }

    private void Awake()
    {
        state = State.WaitingToStart;
        // RigidBody
        boatRigidBody2D = GetComponent<Rigidbody2D>();
        boatRigidBody2D.bodyType = RigidbodyType2D.Static;
        // Transform
        boatTransform = GetComponent<Transform>();
        // Speed
        boatSpeed = 10f;
        boatSpeedTimer = boatSpeedTimerMax;
    }
    
    private void Start(){     
        // Different Event
        Bird.GetInstance().OnStartedPlaying += Bird_OnStartedPlaying;
    }

    private void Bird_OnStartedPlaying(object sender, EventArgs e)
    {
        state = State.Catching;
        boatRigidBody2D.bodyType = RigidbodyType2D.Dynamic;
        CMDebug.TextPopupMouse(GetXPosition().ToString());
    }

    private void Update()
    {
        if (state == State.Catching)
        {
            HandleBoatSpeed();
            // relativeSpeedOfBoatWrtFish = boatSpeed - Level.GetInstance().birdSpeed;
            relativeSpeedOfBoatWrtFish = 35f - Level.GetInstance().birdSpeed;
            Move(relativeSpeedOfBoatWrtFish);
        }
        else if (state == State.FallingBehind)
        {
            HandleBoatSpeed();
            // relativeSpeedOfBoatWrtFish = boatSpeed - Level.GetInstance().birdSpeed;
            relativeSpeedOfBoatWrtFish = 25f - Level.GetInstance().birdSpeed;
            Move(relativeSpeedOfBoatWrtFish);
        }
    }

    private void Move(float speed)
    {
        boatRigidBody2D.velocity = Vector2.right * speed;
    }

    private void HandleBoatSpeed()
    {
        boatSpeedTimer -= Time.deltaTime;
        if (GetXPosition() < -110f)
        {
            state = State.Waiting;
            boatRigidBody2D.bodyType = RigidbodyType2D.Static;
            boatSpeedTimer = boatSpeedTimerMax;
        }
        else if (boatSpeedTimer < 0)
        {
            boatSpeedTimer = boatSpeedTimerMax;
            boatSpeed *= boatSpeedMultiplier;
        }
    }

    private float GetXPosition()
    {
        return boatTransform.position.x;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Bird"))
        {
            boatRigidBody2D.bodyType = RigidbodyType2D.Static;
            state = State.BirdDied;
        }
    }
}
