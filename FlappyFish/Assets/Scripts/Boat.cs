using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boat : MonoBehaviour
{
    // SPEED
    private float boatSpeed;
    private float relativeSpeedOfBoatWrtFish;
    // GLOBAL VAR
    private Rigidbody2D boatRigidBody2D;
    private Transform boatTransform;
    
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
        Bird.GetInstance().OnDied += Bird_OnDied;
    }

    private void Bird_OnDied(object sender, EventArgs e)
    {
        state = State.BirdDied;
        boatRigidBody2D.bodyType = RigidbodyType2D.Static;
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
                    boatRigidBody2D.bodyType = RigidbodyType2D.Dynamic;
                    Move(relativeSpeedOfBoatWrtFish);
                    state = State.Moving;
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
        if (boatTransform.position.x < -110f)
        {
            state = State.Waiting;
            boatRigidBody2D.bodyType = RigidbodyType2D.Static;
        }
    }
}
