using System;
using UnityEngine;


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


    Level levelScript; 

    // Variables for application of speed boost 
    public Vector2 diamondForce; 
    private Vector2 m_startForce; 

    public int speedPoints; 
 
    
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
        speedPoints = 0; 

        levelScript = GameObject.Find("Level").GetComponent<Level>(); 
    }

    private void Update()
    {
        switch (state)
        {
            case State.WaitingToStart:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    speedPoints = 0; 
                    
                    state = State.Playing;
                    birdrigidbody2D.bodyType = RigidbodyType2D.Dynamic;
                    if (OnStartedPlaying!= null) OnStartedPlaying(this, EventArgs.Empty);
                    Jump();
                }
                break;
            case State.Playing:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (levelScript.birdSpeed > 30){
                        levelScript.birdSpeed -= 0.5f; 
                    }
                    else if (levelScript.birdSpeed < 30){
                        levelScript.birdSpeed += 0.5f; 
                    }
    
                    Debug.Log("Playing"); 
                    Jump();
                }
                break;
            case State.Dead:
                break;
        }

        if (birdrigidbody2D.position.y < -50 || birdrigidbody2D.position.y > 50)
        {
            if (OnDied != null) OnDied(this, EventArgs.Empty);
        }
    }

    private void Jump()
    {
        SoundManager.PlaySound(SoundManager.Sound.FishSwim);
        birdrigidbody2D.velocity = Vector2.up * JUMP_AMOUNT;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("SpeedRing"))
        {
            collider.gameObject.active = false; 
            levelScript.birdSpeed = 60; 
            speedPoints++;
        }
        else if (collider.gameObject.CompareTag("Reef"))
        {
            Jump();
        }
        else if (collider.gameObject.CompareTag("Obstacles"))
        {
            collider.gameObject.active = false; 
            levelScript.birdSpeed = 20; 
        }
        else {
            birdrigidbody2D.bodyType = RigidbodyType2D.Static;
            SoundManager.PlaySound(SoundManager.Sound.Lose);
            if (OnDied != null) OnDied(this, EventArgs.Empty);
        }

    }

    

    public Vector3 getPosition()
    {
        return birdrigidbody2D.position;
    }
}
