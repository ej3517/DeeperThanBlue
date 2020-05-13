using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;
using CodeMonkey.Utils;

public class Level : MonoBehaviour
{
    private const float CAMERA_ORTHO_SIZE = 50f;
    // PIPE
    private const float PIPE_WIDTH = 7.8f;
    private const float PIPE_HEAD_HEIGHT = 3.75f;
    private const float PIPE_MOVE_SPEED = 30f;
    private const float PIPE_DESTROY_X_POSITION = -100f;
    private const float PIPE_SPAWN_X_POSITION = 100f;
    // WATER SURFACE
    private const float WATERSURFACE_WIDTH = 20f;
    private const float WATERSURFACE_HEIGHT = 8f;
    private const float WATERSURFACE_MOVE_SPEED = 30f;
    private const float WATERSURFACE_DESTROY_X_POSITION = -120f;
    private const float WATERSURFACE_SPAWN_X_POSITION = 120f;
    // REEF
    private const float REEF_DIMENSION = 14f;
    private const float REEF_DESTROY_X_POSITION = -120f;
    private const float REEF_SPAWN_X_POSITION = 120f;
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
    private float speed_ring_move_speed = 30f;
    // WaterSurface
    private List<HandleWaterSurface.WaterSurface> waterSurfaceList;
    // CoralReef
    private List<HandleReef.Reef> reefList;
    // Structures and data for speed ring 
    private List<HandleSpeedRing.SpeedRing> speedRingList; 
    private float speedRingSpawnTimer; 
    private float speedRingSpawnTimerMax;
    public LayerMask m_LayerMask;
    // State
    private State state;
    
    
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
            pipe.Move(speed_ring_move_speed);
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
                sr.Move(speed_ring_move_speed);
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
            waterSurface.Move(speed_ring_move_speed);
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
            reef.Move(speed_ring_move_speed);
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
        if (lastReefXPosition < REEF_SPAWN_X_POSITION - REEF_DIMENSION + 1)
        {
            HandleReef.CreateReef(REEF_SPAWN_X_POSITION, -CAMERA_ORTHO_SIZE, reefList);
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
}
 

