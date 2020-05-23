﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class Level : MonoBehaviour
{
    // Constants for camera 
    private const float CAMERA_ORTHO_SIZE = 50f;

    private Vector2 screenBounds;

    // PIPE
    private const float PIPE_DESTROY_X_POSITION = -100f;
    private const float PIPE_SPAWN_X_POSITION = 120f;
    // WATER SURFACE
    private const float WATERSURFACE_WIDTH = 20f;
    private const float WATERSURFACE_DESTROY_X_POSITION = -120f;
    private const float WATERSURFACE_SPAWN_X_POSITION = 120f;
    // REEF
    private const float REEF_DIMENSION = 14f;
    private const float REEF_DESTROY_X_POSITION = -120f;
    private const float REEF_SPAWN_X_POSITION = 120f;
    private const int REEF_PIPE_MAX_HEIGHT = 5;
    // BIRD
    private const float BIRD_X_POSITION = 0;
    // SPEED DIAMOND
    private static Level instance;

    public static Level GetInstance()
    {
        return instance;
    }
    
    // Pipe 
    private List<HandlePipe.Pipe> pipeList;
    public int pipesPassedCount;
    private int pipesSpawned;
    private float gapSize;
    private float pipeSpawnTimer;
    private float pipeSpawnTimerMax;

    // Questions
    private List<HandleQuestionBlob.QuestionBlob> questionBlobList;
    private static QuestionWindow questionWindow;
    
    // CoralReef
    private List<HandleReef.Reef> reefList;
    // Structures and data for speed ring 
    private List<HandleSpeedRing.SpeedRing> speedRingList; 
    private float speedRingSpawnTimer; 
    private float speedRingSpawnTimerMax;
    
    // Garbage 
    private List<HandleObstacles.Garbage> garbageList; 
    private float garbageSpawnTimer; 
    private float garbageSpawnTimerMax;

    // State

    private State state;
    // SPEED
    public float birdSpeed;
    
    Bird birdScript;

    public enum Difficulty
    {
        Easy,
        Medium,
        Hard,
        Impossible
    }

    private enum State
    {
        WaitingToStart,
        Playing,
        BirdDead,
    }

    private void Awake()
    {
        instance = this;
        // speed
        birdSpeed = 30f;
        // pipe
        pipeList = new List<HandlePipe.Pipe>();
        gapSize = 50f;
        // speed diamond
        speedRingList = new List<HandleSpeedRing.SpeedRing>();
        // coral reef
        reefList = new List<HandleReef.Reef>();
        HandleReef.CreateInitialReef(-CAMERA_ORTHO_SIZE, reefList);
        // garbage obstacles
        garbageList = new List<HandleObstacles.Garbage>();
        //difficulty
        SetDifficulty(Difficulty.Easy);
        state = State.WaitingToStart;

        questionBlobList = new List<HandleQuestionBlob.QuestionBlob>();
        questionWindow = QuestionWindow.getInstance();
    }

    private void Start()
    {
        birdScript = GameObject.Find("Bird").GetComponent<Bird>(); 
        birdScript.speedPoints = 0;
        Bird.GetInstance().OnDied += Bird_OnDied;
        Bird.GetInstance().OnStartedPlaying += Bird_OnStartedPlaying;
    }

    private void Bird_OnStartedPlaying(object sender, System.EventArgs e)
    {
        state = State.Playing;
    }

    private void Bird_OnDied(object sender, System.EventArgs e)
    {
        state = State.BirdDead;
        questionWindow.Hide();
        //TODO: Delete question tokens?
    }

    private void Update()
    {
        if (state == State.Playing)
        {
            // PIPE
            HandlePipeMovement();
            HandlePipeSpawning();

            // SPEED DIAMONDS
            HandleSpeedRingMovement();
            HandleSpeedRingSpawning();

            // REEF
            HandleReefMovement();
            HandleReefSpawning();
            
            // OBSTACLES
            HandleObstaclesMovement(); 
            HandleObstaclesSpawning(); 
            
            // QUESTIONS
            HandleQuestionMovement();
            HandlePopupQuestion();
        }
    }
    
    /************************************ EVOLUTION OF THE DIFFICULTY ************************************/
    
    private void SetDifficulty(Difficulty difficulty)
    {
        switch (difficulty)
        {
            case Difficulty.Easy:
                pipeSpawnTimerMax = 0.8f;
                speedRingSpawnTimerMax = 3.0f; 
                garbageSpawnTimerMax = 3.0f;
                break;
            case Difficulty.Medium:
                pipeSpawnTimerMax = 1f;
                speedRingSpawnTimerMax = 3.0f; 
                garbageSpawnTimerMax = 2.5f;
                break;
            case Difficulty.Hard:
                pipeSpawnTimerMax = 1.1f;
                speedRingSpawnTimerMax = 3.0f;
                garbageSpawnTimerMax = 2.0f;
                break;
            case Difficulty.Impossible:
                pipeSpawnTimerMax = 1.2f;
                speedRingSpawnTimerMax = 3.0f;
                garbageSpawnTimerMax = 1.5f;
                break;
        }
    }

    private Difficulty GetDifficulty()
    {
        if (pipesSpawned >= 30) return Difficulty.Impossible;
        if (pipesSpawned >= 20) return Difficulty.Hard;
        if (pipesSpawned >= 10) return Difficulty.Medium;
        return Difficulty.Easy;
    }

    /********************************************* PIPE MOVEMENT / SCORE *********************************************/
    
    private void HandlePipeMovement()
    {
        for (int i = 0; i<pipeList.Count; i++)
        {
            HandlePipe.Pipe pipe = pipeList[i];
            bool isRightToTheBird = pipe.GetXPosition() > BIRD_X_POSITION;
            pipe.Move(birdSpeed);
            if (isRightToTheBird && pipe.GetXPosition() <= BIRD_X_POSITION)
            {
                // Pipe passed Bird
                SoundManager.PlaySound(SoundManager.Sound.Score);
                pipesPassedCount++;
            }
            if (pipe.GetXPosition() < PIPE_DESTROY_X_POSITION + birdScript.transform.position.x)
            {
                // Destroy Pipe
                pipe.DestroySelf();
                pipeList.Remove(pipe);
                i--;
            }
        }
    }

    
    private int questionGap = 1;
    private void HandlePipeSpawning()
    {
        pipeSpawnTimer -= Time.deltaTime;
        if (pipeSpawnTimer < 0)
        {
            // Time to spawn another Pipe
            pipeSpawnTimer = pipeSpawnTimerMax;

            float heightEdgeLimit = 10f;
            float minHeight = gapSize * .5f + heightEdgeLimit;
            float totalHeight = CAMERA_ORTHO_SIZE * 2f;
            float maxHeight = totalHeight - gapSize * .5f - heightEdgeLimit;

            float height = UnityEngine.Random.Range(minHeight, maxHeight);
            CreateGapPipes(height, gapSize, PIPE_SPAWN_X_POSITION);

            questionGap -= 1;
            if (questionGap == 0)
            {
                questionGap = UnityEngine.Random.Range(2, 5);         // TODO: Move constants
                //SpawnQuestion(height/2- gapSize/2, PIPE_SPAWN_X_POSITION);
                HandleQuestionBlob.SpawnQuestion(height/2 - gapSize/2 , PIPE_SPAWN_X_POSITION, questionBlobList);
               
            }
            CreateGapPipes(height, gapSize, PIPE_SPAWN_X_POSITION + birdScript.transform.position.x);
        }
    }
    
    private void CreateGapPipes(float gapY, float gapSize, float xPosition)
    {
        HandlePipe.CreatePipe(gapY - gapSize * .5f, xPosition, pipeList);
        pipesSpawned++;
        SetDifficulty(GetDifficulty());
    }

    public int GetPipesPassedCount()
    {
        return pipesPassedCount;
    }

    /************************************************   Speed Diamonds Movement ************************************************/
    
    private void HandleSpeedRingMovement()
    {
        for (int i = 0; i < speedRingList.Count; i++)
        {
            HandleSpeedRing.SpeedRing sr = speedRingList[i];  
            if (sr != null)
            {
                bool isRightToTheBird = sr.getXPosition() > BIRD_X_POSITION; 

                // bool to check whether fish touched rigid body in ring 
                bool passedRing = true; 
                sr.Move(birdSpeed);
                if (isRightToTheBird && sr.getXPosition() <= BIRD_X_POSITION && passedRing)
                {
                    // Fish passed inside ring 
                }
                if (sr.getXPosition() < PIPE_DESTROY_X_POSITION + birdScript.transform.position.x)
                {
                    // Destroy ring 
                    sr.destroySelf(); 
                    speedRingList.Remove(sr); 
                    i--; 
                }
            }
        }
    }

    private void HandleSpeedRingSpawning()
    {
        speedRingSpawnTimer -= Time.deltaTime; 
        if (speedRingSpawnTimer < 0)
        {
        
            // Randomly time to generate another ring 
            speedRingSpawnTimer = speedRingSpawnTimerMax + Random.Range(-2,2); 
            HandleSpeedRing.CreateSpeedRing(PIPE_SPAWN_X_POSITION + birdScript.transform.position.x, speedRingList); 
        }

    }

    /************************************ QUESTION MOVEMENT ************************************/
    private void HandleQuestionMovement()
    {
        for (int i = 0; i < questionBlobList.Count; i++)
        {
            HandleQuestionBlob.QuestionBlob question = questionBlobList[i];
            question.Move(birdSpeed);
            if(question.getDistance(Bird.GetInstance().getPosition()) < 9)
            {
                SoundManager.PlaySound(SoundManager.Sound.Question); //TODO: Add sound
                question.Hide();
                i--;
                PopoupQuestion();
            }
            
            //Out of range
            if (question.getXPosition() < PIPE_DESTROY_X_POSITION)
            {
                question.destroySelf();
                questionBlobList.Remove(question);
                i--;
            }
        }
    }
    
    /******************************************* CORAL REEF MOVEMENT *******************************************/

    private void HandleReefMovement()
    {
        for (int i = 0; i < reefList.Count; i++)
        {
            HandleReef.Reef reef = reefList[i];
            reef.Move(birdSpeed);
            if (reef.GetXPosition() < REEF_DESTROY_X_POSITION)
            {
                reef.DestroySelf();
                reefList.Remove(reef);
                i--;
            }
        }
    }

    private void HandleReefSpawning()
    {
        float lastReefXPosition = reefList[reefList.Count - 1].GetXPosition();
        if (lastReefXPosition < REEF_SPAWN_X_POSITION - REEF_DIMENSION * 0.75)
        {
            int randomReefPipeHeight = Random.Range(1, REEF_PIPE_MAX_HEIGHT + 1);
            HandleReef.CreateReef(REEF_SPAWN_X_POSITION, -CAMERA_ORTHO_SIZE, reefList, randomReefPipeHeight);
        }
    }


    /********************************************* GARBAGE OBSTACLES *********************************************/

    private void HandleObstaclesMovement()
    {
        for (int i = 0; i < garbageList.Count; i++)
        {
            HandleObstacles.Garbage garbage = garbageList[i]; 
            if (garbage != null)
            {
                garbage.Move(birdSpeed);
                if (garbage.GetXPosition() < REEF_DESTROY_X_POSITION)
                {
                    garbage.DestroySelf(); 
                    garbageList.Remove(garbage); 
                    i--; 
                }
            }          
        }
    }

    private void HandleObstaclesSpawning()
    {    
        garbageSpawnTimer -= Time.deltaTime; 
        if (garbageSpawnTimer < 0)
        {
            // Randomly time to generate another ring 
            garbageSpawnTimer = garbageSpawnTimerMax + UnityEngine.Random.Range(0,1); 
            HandleObstacles.CreateGarbage(garbageList); 
        }
    }
    
    /********************************************************************** Creation of the QuestionBlob *********************************************************/

    private float displaytime = 1f;

    private void PopoupQuestion()
    {
        // TODO: Get question with answer from server

        // Create a textbox
        questionWindow.displayQuestion();
        displaytime = 1f;
    }

    private void HandlePopupQuestion()
    {
        if (displaytime > 0)
        {
            displaytime -= Time.deltaTime;
            if (displaytime <= 0)
            {
                questionWindow.Hide();
            }
        }
    }
    
}


