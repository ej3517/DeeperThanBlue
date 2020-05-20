using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boat : MonoBehaviour
{
    // SPEED
    private float boatSpeed;
    private float relativeSpeedOfBoatWrtFish;
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
        Moving,
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
        boatSpeed = 35f;
    }
    
    private void Start(){     
        // Different Event
        Bird.GetInstance().OnStartedPlaying += Bird_OnStartedPlaying;
    }

    private void Bird_OnStartedPlaying(object sender, EventArgs e)
    {
        state = State.Moving;
        boatRigidBody2D.bodyType = RigidbodyType2D.Dynamic;
    }

    private void Update()
    {
        switch (state)
        {
            case State.Moving:
                HandleBoatSpeed();
                relativeSpeedOfBoatWrtFish = boatSpeed - Level.GetInstance().birdSpeed;
                Move(relativeSpeedOfBoatWrtFish);
                break;
            case State.Waiting:
                relativeSpeedOfBoatWrtFish = boatSpeed - Level.GetInstance().birdSpeed;
                if (relativeSpeedOfBoatWrtFish > 0)
                {
                    state = State.Moving;
                    boatRigidBody2D.bodyType = RigidbodyType2D.Dynamic;
                }
                break;
            case State.BirdDied:
                break;
        }
    }

    private void Move(float speed)
    {
        boatRigidBody2D.velocity = Vector2.right * speed;
    }

    private void HandleBoatSpeed()
    {
        if (GetXPosition() < -110f)
        {
            state = State.Waiting;
            boatRigidBody2D.bodyType = RigidbodyType2D.Static;
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
