using System;
using UnityEngine;


public class Bird : MonoBehaviour
{
    // public 
    public QuestionWindow questionWindow;
    public QuizGameController quizGameController;
    public Animator animator;
    public Transform instructionBox;
    // private
    private const float JUMP_AMOUNT = 28f;

    private static Bird instance;

    public static Bird GetInstance()
    {
        return instance;
    }

    private Rigidbody2D birdrigidbody2D;

    private Level levelScript;
    private StateController stateControllerScript;
    private bool birdMoving = true;
    private bool jumping = true;

    private void Awake()
    {
        instance = this;            // I don't think this will work. The instance needs to be declared in a constructor
        birdrigidbody2D = GetComponent<Rigidbody2D>();
        birdrigidbody2D.bodyType = RigidbodyType2D.Static;
        levelScript = GameObject.Find("Level").GetComponent<Level>();
        stateControllerScript = GameObject.Find("StateController").GetComponent<StateController>();
    }

    private void Update()
    {


        switch (stateControllerScript.currentState)
        {
            case StateController.State.WaitingToStart:
                instructionBox.localScale = new Vector3(1, 1, 1);
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    stateControllerScript.currentState = StateController.State.Playing;
                    birdrigidbody2D.bodyType = RigidbodyType2D.Dynamic;
                    Jump();
                }
                break;
            case StateController.State.Playing:
                instructionBox.localScale = new Vector3(0, 0, 0);
                birdrigidbody2D.bodyType = RigidbodyType2D.Dynamic;
                animator.SetBool("birdMoving", birdMoving);
                if (Input.GetKey(KeyCode.Space))
                {
                    Jump();
                    animator.SetFloat("velocityUp", birdrigidbody2D.velocity[1]);
                }
                else
                {
                    animator.SetFloat("velocityUp", birdrigidbody2D.velocity[1]);
                }
                break;
            case StateController.State.WaitingAnswer:
                birdrigidbody2D.bodyType = RigidbodyType2D.Static;
                animator.SetBool("birdMoving", !birdMoving);
                break;
            case StateController.State.Dead:
                questionWindow.Hide();
                animator.SetBool("birdMoving", !birdMoving);
                break;
            case StateController.State.Won:
                questionWindow.Hide();
                animator.SetBool("birdMoving", !birdMoving);
                break;
        }
    }

    private void Jump()
    {
        birdrigidbody2D.velocity = Vector2.up * JUMP_AMOUNT;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("SpeedRing"))
        {
            col.gameObject.SetActive(false);
            stateControllerScript.currentState = StateController.State.WaitingAnswer;
            quizGameController.GetEasyQuestion();
            questionWindow.Show();
            SoundManager.PlaySound(SoundManager.Sound.Question);
        }
        else if (col.gameObject.CompareTag("QuestionBlob"))
        {
            col.gameObject.SetActive(false);
            quizGameController.GetHardQuestion();
            stateControllerScript.currentState = StateController.State.WaitingAnswer;
            questionWindow.Show();
            SoundManager.PlaySound(SoundManager.Sound.Question);
        }
        else if (col.gameObject.CompareTag("Reef"))
        {
            Jump();
        }
        else if (col.gameObject.CompareTag("Obstacles"))
        {
            col.gameObject.SetActive(false);
            levelScript.birdSpeed -= MyGlobals.SPEED_OBSTACLE_REDUCTION;
            SoundManager.PlaySound(SoundManager.Sound.Trash);
        }
        else if (col.gameObject.CompareTag("Boat"))
        {
            stateControllerScript.currentState = StateController.State.Dead;
            birdrigidbody2D.bodyType = RigidbodyType2D.Static;
        }

    }
}
