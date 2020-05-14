using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class Level : MonoBehaviour
{
    // Constants for camera 
    private const float CAMERA_ORTHO_SIZE = 50f;

    private Vector2 screenBounds;
    // Constants for pipe

    // PIPE

    //Maybe not needed?
    //private const float PIPE_WIDTH = 7.8f;
    //private const float PIPE_HEAD_HEIGHT = 3.75f;
    //private const float PIPE_MOVE_SPEED = 30f;
    private const float PIPE_DESTROY_X_POSITION = -100f;
    private const float PIPE_SPAWN_X_POSITION = 100f;


    // Constants for bird - in this case fish 
    private const float BIRD_X_POSITION = 0;    // -- fish position will not be constant. will need to be changed 

    // Constants for speed ring 
    private const float SPEED_RING_MOVE_SPEED = 30f;
    private const float SPEED_RING_DESTROY_X_POSITION = -100f;
    private const float SPEED_RING_SPAWN_X_POSITION = 100f;
    private const float RING_HEIGHT = 2f; 
    private const float RING_WIDTH = 2f; 
    //public Collider2D colliders; 

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
    private List<HandlePipe.Pipe> pipeList;   //Using this instead?
    public int pipesPassedCount;
    // Structures and data required for pipe    
    // Pipe 

    private List<QuestionBlob> questionBlobList;
    //private List<Pipe> pipeList;

    private int pipesSpawned;
    private float gapSize;
    private float pipeSpawnTimer;
    private float pipeSpawnTimerMax;

    // Structures and data for speed ring 
    private List<SpeedRing> speedRingList; 
    private float speedRingSpawnTimer; 
    private float speedRingSpawnTimerMax; 

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
    public LayerMask m_LayerMask;

    private static QuestionWindow questionWindow;
    public float displaytime;

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
        //pipeList = new List<Pipe>();
        questionBlobList = new List<QuestionBlob>();
        questionWindow = QuestionWindow.getInstance();


        speedRingList = new List<SpeedRing>(); 

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
        questionWindow.Hide();
        //TODO: Delete question tokens?
    }

    private void Update()
    {
        if (state == State.Playing)
        {
            // PIPE
            HandlePipeMovement();
            HandleQuestionMovement();
            HandlePipeSpawning();
            HandlePopupQuestion();
            //HandleQuestionSpawning();

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


    private void HandleSpeedRingMovement()
    {
        for (int i = 0; i < speedRingList.Count; i++)
        {
            SpeedRing sr = speedRingList[i];  
            if (sr != null)
            {
                bool isRightToTheBird = sr.getXPosition() > BIRD_X_POSITION; 

                // bool to check whether fish touched rigid body in ring 
                bool passedRing = true; 
                sr.Move(); 
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

    private void HandleQuestionMovement()
    {
        for (int i = 0; i < questionBlobList.Count; i++)
        {
            QuestionBlob question = questionBlobList[i];
            bool isRightToTheBird = question.getXPosition() > BIRD_X_POSITION;
            question.Move();
            
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
/*
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


    private void SpawnQuestion(float _height, float _position)
    {
        Transform _questionBlob = Instantiate(GameAssets.GetInstance().pfQuestionBlob);
        _questionBlob.position = new Vector3(_position, _height);
        QuestionBlob qb = new QuestionBlob(_questionBlob); 
        questionBlobList.Add(qb);
    }
    */

    /**************************************** CREATION OF PIPE ****************************************/
    
    /*
    private void CreateGapPipes(float gapY, float gapSize, float xPosition)
    {
        CreatePipe(gapY - gapSize * .5f, xPosition);

        pipesSpawned++;
        SetDifficulty(GetDifficulty());
    }
    
    private void CreatePipe(float height, float xPosition)
    {
        // set up pipe head
        Transform pipeHead = Instantiate(GameAssets.GetInstance().pfPipeHead);
        float pipeHeadYPosition;
        pipeHeadYPosition = -CAMERA_ORTHO_SIZE + height - PIPE_HEAD_HEIGHT * .5f;
        pipeHead.position = new Vector3(xPosition, pipeHeadYPosition);

        // set up pipe body
        Transform pipeBody = Instantiate(GameAssets.GetInstance().pfPipeBody);

        float PipeBodyYPosition;
        PipeBodyYPosition = -CAMERA_ORTHO_SIZE;
     
        pipeBody.position = new Vector3(xPosition, PipeBodyYPosition);

        SpriteRenderer pipeBodySpriteRenderer = pipeBody.GetComponent<SpriteRenderer>();
        pipeBodySpriteRenderer.size = new Vector2(PIPE_WIDTH, height);

        BoxCollider2D pipeBodyBoxCollider = pipeBody.GetComponent<BoxCollider2D>();
        pipeBodyBoxCollider.size = new Vector2(PIPE_WIDTH, height);
        pipeBodyBoxCollider.offset = new Vector2(0f, height * .5f);

        Pipe pipe = new Pipe(pipeHead, pipeBody);
        pipeList.Add(pipe);
    }


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
            SpeedRing ring = new SpeedRing(sr); 


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

    //public int GetPipesScore()

    public int GetPipesPassedCount()

    {
        return pipesPassedCount;
    }
    */
    /************************************ CREATION OF WATERSURFACE ************************************/
    /*
    private void CreateInitialWaterSurface(float yPosition)
    {
        float leftMostWaterSurfacePosition = WATERSURFACE_SPAWN_X_POSITION;
        
        // Creation of the Initial Water Line
        while (leftMostWaterSurfacePosition > WATERSURFACE_DESTROY_X_POSITION)
*/
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
}
 

    /* 
     * represents speed rings 
    */ 
/*
    public class SpeedRing
    {
        public Transform speedRingTransform; 
    
        
        public SpeedRing(Transform speedRingTransform)
        {
            this.speedRingTransform = speedRingTransform; 
        }

        public void Move()
        {
            speedRingTransform.position += new Vector3(-1, 0, 0) * PIPE_MOVE_SPEED * Time.deltaTime; 
        }

        public float getXPosition()
        {
            return speedRingTransform.position.x; 
        }

        public void destroySelf()
        {
            Destroy(speedRingTransform.gameObject); 
        }

        public void size(float x)
        {
            Vector3 vec = new Vector3(x, 1, 0); 
            speedRingTransform.localScale = vec; 
        }
    }
    
    private class QuestionBlob
    {
        private Transform questionTransform;
        public QuestionBlob(Transform _questionTransform)
        {
            questionTransform = _questionTransform;
        }

        public void Move()
        {
            questionTransform.position += new Vector3(-1, 0, 0) * PIPE_MOVE_SPEED * Time.deltaTime;        //RENAME MOVESPEED
        }

        public float getXPosition()
        {
            return questionTransform.position.x;
        }
        public float getDistance(Vector3 _from)
        {
            return Vector3.Distance(_from, questionTransform.position);
        }

        public void destroySelf()
        {
            Destroy(questionTransform.gameObject);
        }

    }
    
    public static bool GetQuestion;

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
*/
