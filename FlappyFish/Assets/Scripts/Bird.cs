using System;
using UnityEngine;


public class Bird : MonoBehaviour
{
    private const float JUMP_AMOUNT = 28f;
    // Variation of speed with SPEED RING
    public float speedRingBoost;
    public float speedObstacleReduction;
    // Gravity direction
    private bool lastBoundTouchedIsSurface;
    // Question
    private QuestionWindow questionWindow;
    
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
        speedRingBoost = 5f;
        speedObstacleReduction = 5f;
        lastBoundTouchedIsSurface = false;
        levelScript = GameObject.Find("Level").GetComponent<Level>();
        questionWindow = QuestionWindow.GetInstance();
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
                    Jump(lastBoundTouchedIsSurface);
                }
                break;
            case State.Playing:
                if (Input.GetKey(KeyCode.Space))
                {
                    Jump(lastBoundTouchedIsSurface);
                }
                break;
            case State.Dead:
                questionWindow.Hide();
                break;
        }
    }

    private void Jump(bool jumpDown)
    {
        SoundManager.PlaySound(SoundManager.Sound.FishSwim);
        if (jumpDown)
        {
            birdrigidbody2D.velocity = Vector2.down * JUMP_AMOUNT;
        }
        else
        {
            birdrigidbody2D.velocity = Vector2.up * JUMP_AMOUNT;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("SpeedRing"))
        {
            questionWindow.DisplayQuestion();
            col.gameObject.SetActive(false); 
            levelScript.birdSpeed += speedRingBoost; 
            speedPoints++;
        }
        else if (col.gameObject.CompareTag("Reef")||col.gameObject.CompareTag("Pipe"))
        {
            Jump(false); // JUMP UP
        }
        else if (col.gameObject.CompareTag("Ground"))
        {
            if (!lastBoundTouchedIsSurface) {
                birdrigidbody2D.gravityScale *= -1;
                lastBoundTouchedIsSurface = true; }
        }
        else if (col.gameObject.CompareTag("WaterSurface"))
        {
            if (lastBoundTouchedIsSurface) {
                birdrigidbody2D.gravityScale *= -1;
                lastBoundTouchedIsSurface = false; }
        }
        else if (col.gameObject.CompareTag("Obstacles"))
        {
            col.gameObject.SetActive(false);
            levelScript.birdSpeed -= speedObstacleReduction;
        }
        else if (col.gameObject.CompareTag("QuestionBlob"))
        {
            col.gameObject.SetActive(false);
            questionWindow.DisplayQuestion();
        }
        else{
            birdrigidbody2D.bodyType = RigidbodyType2D.Static;
            SoundManager.PlaySound(SoundManager.Sound.Lose);
            if (OnDied != null) OnDied(this, EventArgs.Empty);
            state = State.Dead;
        }

    }

    public Vector3 getPosition()
    {
        return birdrigidbody2D.position;
    }
}
