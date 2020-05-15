using System.Collections;
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
    private const float PIPE_SPAWN_X_POSITION = 100f;
    // Constants for speed ring 
    private const float SPEED_RING_MOVE_SPEED = 30f;
    private const float SPEED_RING_DESTROY_X_POSITION = -100f;
    private const float SPEED_RING_SPAWN_X_POSITION = 100f;
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
    private const float RING_HEIGHT = 2f; 
    private const float RING_WIDTH = 2f; 
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
    private List<QuestionBlob> questionBlobList;
    private static QuestionWindow questionWindow;

    // WaterSurface
    private List<HandleWaterSurface.WaterSurface> waterSurfaceList;
    // CoralReef
    private List<HandleReef.Reef> reefList;
    // Structures and data for speed ring 
    private List<HandleSpeedRing.SpeedRing> speedRingList; 
    private float speedRingSpawnTimer; 
    private float speedRingSpawnTimerMax;

    // Boat
    public LayerMask m_LayerMask;
    // State

    private State state;
    // SPEED
    public float birdSpeed = 30f;
    
    
    FollowFish cameraScript; 
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
        // pipe
        pipeList = new List<HandlePipe.Pipe>();
        // waterSurface
        waterSurfaceList = new List<HandleWaterSurface.WaterSurface>();
        HandleWaterSurface.CreateInitialWaterSurface(CAMERA_ORTHO_SIZE, waterSurfaceList);
        // speed diamond
        speedRingList = new List<HandleSpeedRing.SpeedRing>();
        // coral reef
        reefList = new List<HandleReef.Reef>();
        HandleReef.CreateInitialReef(-CAMERA_ORTHO_SIZE, reefList);
        //difficulty

        SetDifficulty(Difficulty.Easy);
        state = State.WaitingToStart;

        questionBlobList = new List<QuestionBlob>();
        questionWindow = QuestionWindow.getInstance();
    }

    private void Start()
    {
        cameraScript = Camera.main.GetComponent<FollowFish>();
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

            // WATERSURFACE
            HandleWaterSurfaceMovement();
            HandleWaterSurfaceSpawning();

            // REEF
            HandleReefMovement();
            HandleReefSpawning();
            
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
                gapSize = 50f;
                pipeSpawnTimerMax = 0.8f;
                speedRingSpawnTimerMax = 3.0f; 
                break;
            case Difficulty.Medium:
                gapSize = 40f;
                pipeSpawnTimerMax = 1f;
                speedRingSpawnTimerMax = 3.0f; 
                break;
            case Difficulty.Hard:
                gapSize = 33f;
                pipeSpawnTimerMax = 1.1f;
                speedRingSpawnTimerMax = 3.0f; 
                break;
            case Difficulty.Impossible:
                gapSize = 24f;
                pipeSpawnTimerMax = 1.2f;
                speedRingSpawnTimerMax = 3.0f; 
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
                SpawnQuestion(height/2 - gapSize/2 , PIPE_SPAWN_X_POSITION);
               
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
                    Debug.Log("Passed in ring"); 
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
        bool canSpawnHere = false; 
        speedRingSpawnTimer -= Time.deltaTime; 
        if (speedRingSpawnTimer < 0)
        {
        
            // Randomly time to generate another ring 
            speedRingSpawnTimer = speedRingSpawnTimerMax + UnityEngine.Random.Range(-2,2); 
            CreateSpeedRing(PIPE_SPAWN_X_POSITION + birdScript.transform.position.x); 
        }

    }

    /******************************************* WATER SURFACE MOVEMENT *******************************************/

    private void HandleWaterSurfaceMovement()
    {
        for (int i = 0; i < waterSurfaceList.Count; i++)
        {
            HandleWaterSurface.WaterSurface waterSurface = waterSurfaceList[i];
            waterSurface.Move(birdSpeed);
            if (waterSurface.GetXPosition() < WATERSURFACE_DESTROY_X_POSITION)
            {
                waterSurface.DestroySelf();
                waterSurfaceList.Remove(waterSurface);
                i--;
            }
        }
    }
    
    /************************************ QUESTION MOVEMENT ************************************/
    private void HandleQuestionMovement()
    {
        for (int i = 0; i < questionBlobList.Count; i++)
        {
            QuestionBlob question = questionBlobList[i];
            question.Move(birdSpeed);
            if(question.getDistance(Bird.GetInstance().getPosition()) < 9)
            {
                SoundManager.PlaySound(SoundManager.Sound.Question); //TODO: Add sound
                question.destroySelf();
                questionBlobList.Remove(question);
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

    private void HandleWaterSurfaceSpawning()
    {
        // Spawn a new water surface after the last one);
        float lastWaterSurfaceXPosition = waterSurfaceList[waterSurfaceList.Count - 1].GetXPosition();
        if (lastWaterSurfaceXPosition < WATERSURFACE_SPAWN_X_POSITION - WATERSURFACE_WIDTH + 1)
        {
            HandleWaterSurface.CreateWaterSurface(WATERSURFACE_SPAWN_X_POSITION, CAMERA_ORTHO_SIZE, waterSurfaceList);
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

    /********************************************************************** Creation of Speed Diamond *********************************************************/
    
    private void CreateSpeedRing (float xPosition)
    {
        bool canSpawnHere = false; 

        while (!canSpawnHere)
        {
            Debug.Log("Create Ring"); 

            screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z)); 
            float yPosition; 
            yPosition = UnityEngine.Random.Range(-screenBounds.y, screenBounds.y); 

            Transform sr = Instantiate(GameAssets.GetInstance().pfSpeedRing);
            sr.position = new Vector3(xPosition - 2, yPosition); 
            HandleSpeedRing.SpeedRing ring = new HandleSpeedRing.SpeedRing(sr); 


            canSpawnHere = PreventSpawnOverlap(ring.speedRingTransform);
            if (canSpawnHere)
            {
                Debug.Log("Created Ring");
                
                speedRingList.Add(ring); 
                break;
            } 
            else {
                Debug.Log("Destroyed Ring"); 
                ring.destroySelf();                
            }
        }
    }

    private bool PreventSpawnOverlap(Transform tmpTransform)
    { 
        Collider2D colliders;  
        colliders = Physics2D.OverlapBox(tmpTransform.position, tmpTransform.localScale * 2, 0f, m_LayerMask); 

        if (colliders == null)
        {
            return true; 
        }
        else
        {
            return false; 
        }
    }

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

    private void SpawnQuestion(float _height, float _position)
    {
        Transform _questionBlob = Instantiate(GameAssets.GetInstance().pfQuestionBlob);
        _questionBlob.position = new Vector3(_position, _height);
        QuestionBlob qb = new QuestionBlob(_questionBlob);
        questionBlobList.Add(qb);
    }

}


