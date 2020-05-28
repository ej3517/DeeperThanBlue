using System;
using UnityEngine;


public class Bird : MonoBehaviour
{
    // public 
    public QuestionWindow questionWindow;
    // private
    private const float JUMP_AMOUNT = 28f;
    // Variation of speed with SPEED RING
    public float speedRingBoost;
    public float speedObstacleReduction;
    // Question
    private static Bird instance;

    public static Bird GetInstance()
    {
        return instance;
    }

    public event EventHandler OnDied;
    public event EventHandler OnStartedPlaying;

    private Rigidbody2D birdrigidbody2D;
    private State state;
    
    private Level levelScript;
    private Boat boatScript;
    
    
    private enum State
    {
        WaitingToStart,
        Playing,
        WaitingAnswer,
        Dead,
    }

    private void Awake()
    {
        instance = this;
        birdrigidbody2D = GetComponent<Rigidbody2D>();
        birdrigidbody2D.bodyType = RigidbodyType2D.Static;
        state = State.WaitingToStart;
        speedRingBoost = 5f;
        speedObstacleReduction = 5f;
        levelScript = GameObject.Find("Level").GetComponent<Level>();
        boatScript = GameObject.Find("Boat").GetComponent<Boat>();
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
                if (Input.GetKey(KeyCode.Space))
                {
                    Jump();
                }
                break;
            case State.WaitingAnswer:
                break;
            case State.Dead:
                questionWindow.Hide();
                break;
        }
    }

    private void Jump ()
    {
        SoundManager.PlaySound(SoundManager.Sound.FishSwim);
        birdrigidbody2D.velocity = Vector2.up * JUMP_AMOUNT;

    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("SpeedRing"))
        {
            col.gameObject.SetActive(false);
            birdrigidbody2D.bodyType = RigidbodyType2D.Static;
            levelScript.birdSpeed += speedRingBoost;
            state = State.WaitingAnswer;
            levelScript.state = Level.State.WaitingAnswer;
            boatScript.state = Boat.State.WaitingAnswer;
            questionWindow.Show();
        }
        else if (col.gameObject.CompareTag("QuestionBlob"))
        {
            col.gameObject.SetActive(false);
            birdrigidbody2D.bodyType = RigidbodyType2D.Static;
            state = State.WaitingAnswer;
            boatScript.state = Boat.State.WaitingAnswer;
            levelScript.state = Level.State.WaitingAnswer;
            questionWindow.Show();
        }
        else if (col.gameObject.CompareTag("Reef"))
        {
            Jump();
        }
        else if (col.gameObject.CompareTag("Obstacles"))
        {
            col.gameObject.SetActive(false);
            levelScript.birdSpeed -= speedObstacleReduction;
        }
        else if (col.gameObject.CompareTag("Boat")){
            birdrigidbody2D.bodyType = RigidbodyType2D.Static;
            SoundManager.PlaySound(SoundManager.Sound.Lose);
            if (OnDied != null) OnDied(this, EventArgs.Empty);
            state = State.Dead;
        }

    }
}
