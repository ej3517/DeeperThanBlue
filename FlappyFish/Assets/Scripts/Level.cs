﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;
using CodeMonkey.Utils;

public class Level : MonoBehaviour
{
    private const float CAMERA_ORTHO_SIZE = 50f;
    // PIPE
    private const float PIPE_DESTROY_X_POSITION = -100f;
    private const float PIPE_SPAWN_X_POSITION = 100f;
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
    private const float SPEED_RING_DESTROY_X_POSITION = -100f;
    private const float SPEED_RING_SPAWN_X_POSITION = 100f;
    private const float RING_HEIGHT = 2f; 
    private const float RING_WIDTH = 2f; 
    private static Level instance;

    public static Level GetInstance()
    {
        return instance;
    }
    
    private Vector2 screenBounds;
    // Pipe 
    private List<HandlePipe.Pipe> pipeList;
    public int pipesPassedCount;
    private int pipesSpawned;
    private float gapSize;
    private float pipeSpawnTimer;
    private float pipeSpawnTimerMax;
    // WaterSurface
    private List<HandleWaterSurface.WaterSurface> waterSurfaceList;
    // CoralReef
    private List<HandleReef.Reef> reefList;
    // Structures and data for speed ring 
    private List<HandleSpeedRing.SpeedRing> speedRingList; 
    private float speedRingSpawnTimer; 
    private float speedRingSpawnTimerMax;

    // Boat
    private HandleBoat.Boat boat; 
    public LayerMask m_LayerMask;

    // Garbage 
    private List<HandleObstacles.Garbage> garbageList; 
    private float garbageSpawnTimer; 
    private float garbageSpawnTimerMax;

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
        // boat 
        boat = HandleBoat.CreateBoat(); 
        // garbage obstacles
        garbageList = new List<HandleObstacles.Garbage>(); 


        //difficulty
        SetDifficulty(Difficulty.Easy);
        state = State.WaitingToStart;
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

            // BOAT 
            //boat.Move(birdSpeed); 

            // OBSTACLES
            HandleObstaclesMovement(); 
            HandleObstaclesSpawning(); 
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
                garbageSpawnTimerMax = 3.0f; 
                break;
            case Difficulty.Medium:
                gapSize = 40f;
                pipeSpawnTimerMax = 1f;
                speedRingSpawnTimerMax = 3.0f; 
                garbageSpawnTimerMax = 2.5f; 
                break;
            case Difficulty.Hard:
                gapSize = 33f;
                pipeSpawnTimerMax = 1.1f;
                speedRingSpawnTimerMax = 3.0f;
                garbageSpawnTimerMax = 2.0f;  
                break;
            case Difficulty.Impossible:
                gapSize = 24f;
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
            if (pipe.GetXPosition() < PIPE_DESTROY_X_POSITION)
            {
                // Destroy Pipe
                pipe.DestroySelf();
                pipeList.Remove(pipe);
                i--;
            }
        }
    }

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


    /********************************************* GARBAGE OBSTACLES *********************************************/

    private void HandleObstaclesMovement()
    {
        for (int i = 0; i < garbageList.Count; i++)
        {
            HandleObstacles.Garbage garbage = garbageList[i]; 
            if (garbage != null)
            {
                garbage.Move(birdSpeed);
                if (garbage.getXPosition() < REEF_DESTROY_X_POSITION)
                {
                    garbage.destroySelf(); 
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
    /********************************************************************** Creation of Speed Diamond *********************************************************/
    
    private void CreateSpeedRing (float xPosition)
    {
        bool canSpawnHere = false; 


        Debug.Log("Create Ring"); 

        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z)); 
        float yPosition; 
        Transform sr = Instantiate(GameAssets.GetInstance().pfSpeedRing);
        while (!canSpawnHere)
        {
            yPosition = UnityEngine.Random.Range(-screenBounds.y, screenBounds.y); 

            
            sr.position = new Vector3(xPosition - 20f, yPosition); 
            HandleSpeedRing.SpeedRing ring = new HandleSpeedRing.SpeedRing(sr); 

           
            canSpawnHere = PreventSpawnOverlap(ring.speedRingTransform);
            if (canSpawnHere)
            {
                Debug.Log("Created Ring");
                speedRingList.Add(ring); 
                break;
            } 
        }
    }

    private bool PreventSpawnOverlap(Transform tmpTransform)
    { 
        Collider2D colliders;  
        colliders = Physics2D.OverlapBox(tmpTransform.position, tmpTransform.localScale * 2, 0f, m_LayerMask); 

        if (colliders == null)
        {
            Debug.Log("Can spawn"); 
            return true; 
        }
        else
        {
            Debug.Log("Can't spawn");
            return false; 
        }
    }
}
 

