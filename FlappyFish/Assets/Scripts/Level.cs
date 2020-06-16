using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI; 

public class Level : MonoBehaviour
{
    private const int REEF_PIPE_MAX_HEIGHT = 5;
    private const float BIRD_X_POSITION = 0;
    private static Level instance;

    public QuizGameController quizGameController;

    public static Level GetInstance()
    {
        return instance;
    }
    
    /*********** GROUND *********/
    // Pipe 
    private List<HandlePipe.Pipe> pipeList;
    public int pipesPassedCount;
    private int pipesSpawned;
    private float pipeSpawnTimer;
    private float pipeSpawnTimerMax;
    // CoralReef
    private List<HandleReef.Reef> reefList;
    
    /*********** FLOATING OBJECTS : QUESTION/SPEED_RING/GARBAGE *********/
    // Timer
    private bool hasJustStarted;
    private float spawnFloatingTimer;
    private float spawnFloatingTimerMax;
    private float randomSelector;
    // List of floating objects
    private List<HandleQuestionBlob.QuestionBlob> questionBlobList;
    private List<HandleSpeedRing.SpeedRing> speedRingList;
    private List<HandleObstacles.Garbage> garbageList;
    
    // SPEED
    public float birdSpeed;
    
    // STATE 
    private StateController stateControllerScript;
    
    Bird birdScript;

    public enum Difficulty
    {
        Easy,
        Medium,
        Hard,
        Impossible
    }

    private void Awake()
    {
        instance = this;
        // speed
        birdSpeed = 30f;
        // List of objects
        pipeList = new List<HandlePipe.Pipe>();
        reefList = new List<HandleReef.Reef>();

        HandleReef.CreateInitialReef(-MyGlobals.CAMERA_ORTHO_SIZE, reefList);
        speedRingList = new List<HandleSpeedRing.SpeedRing>();
        garbageList = new List<HandleObstacles.Garbage>();
        questionBlobList = new List<HandleQuestionBlob.QuestionBlob>();
        hasJustStarted = true;
        
        //difficulty
        SetDifficulty(Difficulty.Easy);
        pipeSpawnTimerMax = 1.2f;

        // state
        stateControllerScript = GameObject.Find("StateController").GetComponent<StateController>();
        birdScript = GameObject.Find("Bird").GetComponent<Bird>();
    }
    
    private void Update()
    {
        if (stateControllerScript.currentState == StateController.State.Playing)
        {
            // PIPE AND REEF
            HandlePipeMovement();
            HandlePipeSpawning();
            HandleReefMovement();
            HandleReefSpawning();

            // FLOATING OBJECTS SPAWNING
            HandleFloatingObjectSpawning();
            // FLOATING OBJECTS MOVEMENT 
            HandleSpeedRingMovement();
            HandleObstaclesMovement();
            HandleQuestionMovement();
        }
    }
    
    /************************************ EVOLUTION OF THE DIFFICULTY ************************************/
    
    private void SetDifficulty(Difficulty difficulty)
    {
        switch (difficulty)
        {
            case Difficulty.Easy:
                spawnFloatingTimerMax = 0.6f; break;
            case Difficulty.Medium:
                spawnFloatingTimerMax = 0.5f; break;
            case Difficulty.Hard:
                spawnFloatingTimerMax = 0.45f;break;
            case Difficulty.Impossible:
                spawnFloatingTimerMax = 0.4f;break;
        }
    }

    private Difficulty GetDifficulty()
    {
        if (pipesSpawned >= 30) return Difficulty.Impossible;
        if (pipesSpawned >= 20) return Difficulty.Hard;
        if (pipesSpawned >= 10) return Difficulty.Medium;
        return Difficulty.Easy;
    }
    
    /********************************************* OBJECTS SPAWNING *********************************************/
    
    private void HandleFloatingObjectSpawning()
    {
        spawnFloatingTimer -= Time.deltaTime;
        if (spawnFloatingTimer < 0)
        {
            spawnFloatingTimer = spawnFloatingTimerMax + Random.Range(-0.2f, 0.2f);
            randomSelector = Random.Range(0f, 1f);
            if (hasJustStarted)
            {
                HandleSpeedRing.CreateSpeedRing(MyGlobals.SPAWN_X_POSITION + birdScript.transform.position.x, speedRingList); // first object must be a speed ring
                hasJustStarted = false;
            }
            else if (0f <= randomSelector && randomSelector < 0.60f) // Trash
            {
                HandleObstacles.CreateGarbage(garbageList);
            }
            else if (0.60f <= randomSelector && randomSelector < 0.90f) // Speed Ring
            {
                HandleSpeedRing.CreateSpeedRing(MyGlobals.SPAWN_X_POSITION + birdScript.transform.position.x, speedRingList);
            }
            else // Deadly Question
            {
                float minHeight = 10f;
                float maxHeight = 40f;
                float height = Random.Range(minHeight, maxHeight);
                HandleQuestionBlob.SpawnQuestion(height/2 , MyGlobals.SPAWN_X_POSITION, questionBlobList);
            }
        }
    }
    
    private void HandlePipeSpawning()
    {
        pipeSpawnTimer -= Time.deltaTime;
        if (pipeSpawnTimer < 0)
        {
            pipeSpawnTimer = pipeSpawnTimerMax;
            
            float minHeight = 10f;
            float maxHeight = 40f;

            float height = Random.Range(minHeight, maxHeight);
            CreateGapPipes(height, MyGlobals.SPAWN_X_POSITION);
            
            CreateGapPipes(height, MyGlobals.SPAWN_X_POSITION + birdScript.transform.position.x);
        }
    }

    /********************************************* PIPE MOVEMENT / SCORE *********************************************/
    
    private void HandlePipeMovement()
    {
        for (int i = 0; i<pipeList.Count; i++)
        {
            HandlePipe.Pipe pipe = pipeList[i];
            bool isRightToTheBird = pipe.GetXPosition() > BIRD_X_POSITION;
            pipe.Move(birdSpeed);
            if (isRightToTheBird && pipe.GetXPosition() <= BIRD_X_POSITION) {
                // Pipe passed Bird
                SoundManager.PlaySound(SoundManager.Sound.Score);
                pipesPassedCount++;
            }
            if (pipe.GetXPosition() < MyGlobals.DESTROY_X_POSITION + birdScript.transform.position.x) {
                // Destroy Pipe
                pipe.DestroySelf();
                pipeList.Remove(pipe);
                i--;
            }
            if (pipesPassedCount == 5) {
                quizGameController.playerScore += MyGlobals.POINTS_FOR_PASSED_PIPES;
                pipesPassedCount = 0;
            }
        }
    }

    private void CreateGapPipes(float gapY, float xPosition)
    {
        HandlePipe.CreatePipe(gapY, xPosition, pipeList);
        pipesSpawned++;
        SetDifficulty(GetDifficulty());
    }

    /************************************************   Speed Diamonds Movement ************************************************/
    
    private void HandleSpeedRingMovement()
    {
        for (int i = 0; i < speedRingList.Count; i++)
        {
            HandleSpeedRing.SpeedRing sr = speedRingList[i];  
            if (sr != null)
            {
                bool isRightToTheBird = sr.GetXPosition() > BIRD_X_POSITION; 

                // bool to check whether fish touched rigid body in ring 
                bool passedRing = true; 
                sr.Move(birdSpeed);
                if (isRightToTheBird && sr.GetXPosition() <= BIRD_X_POSITION && passedRing)
                {
                    // Fish passed inside ring 
                }
                if (sr.GetXPosition() < MyGlobals.DESTROY_X_POSITION + birdScript.transform.position.x)
                {
                    // Destroy ring 
                    sr.destroySelf(); 
                    speedRingList.Remove(sr); 
                    i--; 
                }
            }
        }
    }

    /************************************ QUESTION MOVEMENT ************************************/
    
    private void HandleQuestionMovement()
    {
        for (int i = 0; i < questionBlobList.Count; i++)
        {
            HandleQuestionBlob.QuestionBlob question = questionBlobList[i];
            question.Move(birdSpeed);
            
            if (question.GetXPosition() < MyGlobals.DESTROY_X_POSITION) //Out of range
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
            if (reef.GetXPosition() < MyGlobals.DESTROY_X_POSITION)
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
        if (lastReefXPosition < MyGlobals.SPAWN_X_POSITION - MyGlobals.REEF_DIMENSION * 0.75)
        {
            int randomReefPipeHeight = Random.Range(1, REEF_PIPE_MAX_HEIGHT + 1);
            HandleReef.CreateReef(MyGlobals.SPAWN_X_POSITION, -MyGlobals.CAMERA_ORTHO_SIZE, reefList, randomReefPipeHeight);
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
                if (garbage.GetXPosition() < MyGlobals.DESTROY_X_POSITION)
                {
                    garbage.DestroySelf(); 
                    garbageList.Remove(garbage); 
                    i--; 
                }
            }          
        }
    }
}


