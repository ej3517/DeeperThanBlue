using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;
using UnityEditorInternal;

public class Bird : MonoBehaviour
{
    private const float JUMP_AMOUNT = 100f;

    private static Bird instance;

    public static Bird GetInstance()
    {
        return instance;
    }

    public event EventHandler OnDied;
    public event EventHandler OnStartedPlaying;
    
    private Rigidbody2D birdrigidbody2D;
    private State state;
    
    private enum State
    {
        WaitingToStart,
        Playing,
        Dead,
    }

    private void Awake()
    {
        instance = this;
        birdrigidbody2D = GetComponent<Rigidbody2D>();
        birdrigidbody2D.bodyType = RigidbodyType2D.Static;
        state = State.WaitingToStart;
    }

    private void Update()
    {
        switch (state)
        {
            case State.WaitingToStart:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    state = State.Playing;
                    birdrigidbody2D.bodyType = RigidbodyType2D.Dynamic;
                    if (OnStartedPlaying!= null) OnStartedPlaying(this, EventArgs.Empty);
                    Jump();
                }
                break;
            case State.Playing:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Jump();
                }
                break;
            case State.Dead:
                break;
        }
    }

    private void Jump()
    {
        SoundManager.PlaySound(SoundManager.Sound.FishSwim);
        birdrigidbody2D.velocity = Vector2.up * JUMP_AMOUNT;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.name == "pfSpeedRing(Clone)")
        {
            //collider.gameObject.isCaught = true; 
            //Level.SpeedRing instance = new Level.SpeedRing(collider.gameObject.transform, false);
            //GameObject sr = GetComponent<Level.SpeedRing>(); 
            //instance = collider.gameObject; 
            //Destroy(collider.gameObject); 
            //instance.destroySelf(); 
            //Destroy(collider.gameObject); 
        }
        else {
            birdrigidbody2D.bodyType = RigidbodyType2D.Static;
            SoundManager.PlaySound(SoundManager.Sound.Lose);
            if (OnDied != null) OnDied(this, EventArgs.Empty);
        }

    }


}
