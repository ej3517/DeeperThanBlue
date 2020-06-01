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

    private static Bird instance;

    public static Bird GetInstance()
    {
        return instance;
    }
    
    private Rigidbody2D birdrigidbody2D;

    private Level levelScript;
    private StateController stateControllerScript;
    
    private void Awake()
    {
        instance = this;
        birdrigidbody2D = GetComponent<Rigidbody2D>();
        birdrigidbody2D.bodyType = RigidbodyType2D.Static;
        speedRingBoost = 5f;
        speedObstacleReduction = 5f;
        levelScript = GameObject.Find("Level").GetComponent<Level>();
        stateControllerScript = GameObject.Find("StateController").GetComponent<StateController>(); //
    }

    private void Update()
    {
        switch (stateControllerScript.currentState)
        {
            case StateController.State.WaitingToStart:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    stateControllerScript.currentState = StateController.State.Playing;
                    birdrigidbody2D.bodyType = RigidbodyType2D.Dynamic;
                    Jump();
                }
                break;
            case StateController.State.Playing:
                birdrigidbody2D.bodyType = RigidbodyType2D.Dynamic;
                if (Input.GetKey(KeyCode.Space))
                {
                    Jump();
                }
                break;
            case StateController.State.WaitingAnswer:
                birdrigidbody2D.bodyType = RigidbodyType2D.Static;
                break;
            case StateController.State.Dead:
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
            stateControllerScript.currentState = StateController.State.WaitingAnswer;
            questionWindow.Show();
            levelScript.birdSpeed += speedRingBoost;
        }
        else if (col.gameObject.CompareTag("QuestionBlob"))
        {
            col.gameObject.SetActive(false);
            stateControllerScript.currentState = StateController.State.WaitingAnswer;
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
            stateControllerScript.currentState = StateController.State.Dead;
            birdrigidbody2D.bodyType = RigidbodyType2D.Static;
            SoundManager.PlaySound(SoundManager.Sound.Lose);
        }

    }
}
