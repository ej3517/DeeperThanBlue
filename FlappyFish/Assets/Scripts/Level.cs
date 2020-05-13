﻿using System.Collections;
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
    private const float REEF_MOVE_SPEED = 30f;
    private const float REEF_DIMENSION = 14f;
    private const float REEF_DESTROY_X_POSITION = -120f;
    private const float REEF_SPAWN_X_POSITION = 120f;
    // BIRD
    private const float BIRD_X_POSITION = 0;

    private static Level instance;

    public static Level GetInstance()
    {
        return instance;
    }
    
    // Pipe 
    private List<Pipe> pipeList;
    private int pipesPassedCount;
    private int pipesSpawned;
    private float gapSize;
    private float pipeSpawnTimer;
    private float pipeSpawnTimerMax;
    // WaterSurface
    private List<WaterSurface> waterSurfaceList;
    // CoralReef
    private List<Reef> reefList;
    // State
    private State state;

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
        pipeList = new List<Pipe>();
        // waterSurface
        waterSurfaceList = new List<WaterSurface>();
        CreateInitialWaterSurface(CAMERA_ORTHO_SIZE);
        // coral reef
        reefList = new List<Reef>();
        CreateInitialReef(-CAMERA_ORTHO_SIZE);
        //difficulty
        SetDifficulty(Difficulty.Easy);
        state = State.WaitingToStart;
    }

    private void Start()
    {
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
            // WATERSURFACE
            HandleWaterSurfaceMovement();
            HandleWaterSurfaceSpawning();
            // REEF
            HandleReefMovement();
            HandleReefSpawning();
        }
    }

    /********************************************* PIPE MOVEMENT *********************************************/
    
    private void HandlePipeMovement()
    {
        for (int i = 0; i<pipeList.Count; i++)
        {
            Pipe pipe = pipeList[i];
            bool isRightToTheBird = pipe.GetXPosition() > BIRD_X_POSITION;
            pipe.Move();
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
    
    /******************************************* WATER SURFACE MOVEMENT *******************************************/

    private void HandleWaterSurfaceMovement()
    {
        for (int i = 0; i < waterSurfaceList.Count; i++)
        {
            WaterSurface waterSurface = waterSurfaceList[i];
            waterSurface.Move();
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
            CreateWaterSurface(WATERSURFACE_SPAWN_X_POSITION, CAMERA_ORTHO_SIZE);
        }
    }
    
    /******************************************* CORAL REEF MOVEMENT *******************************************/

    private void HandleReefMovement()
    {
        for (int i = 0; i < reefList.Count; i++)
        {
            Reef reef = reefList[i];
            reef.Move();
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
            CreateReef(REEF_SPAWN_X_POSITION, -CAMERA_ORTHO_SIZE);
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
                break;
            case Difficulty.Medium:
                gapSize = 40f;
                pipeSpawnTimerMax = 1f;
                break;
            case Difficulty.Hard:
                gapSize = 33f;
                pipeSpawnTimerMax = 1.1f;
                break;
            case Difficulty.Impossible:
                gapSize = 24f;
                pipeSpawnTimerMax = 1.2f;
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

    /**************************************** CREATION OF PIPE ****************************************/
    
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

    public int GetPipesPassedCount()
    {
        return pipesPassedCount;
    }
    
    /************************************ CREATION OF WATERSURFACE ************************************/

    private void CreateInitialWaterSurface(float yPosition)
    {
        float leftMostWaterSurfacePosition = WATERSURFACE_SPAWN_X_POSITION;
        
        // Creation of the Initial Water Line
        while (leftMostWaterSurfacePosition > WATERSURFACE_DESTROY_X_POSITION)
        {
            Transform waterSurfaceTransform = Instantiate(GameAssets.GetInstance().pfWaterSurface);
            waterSurfaceTransform.position = new Vector3(leftMostWaterSurfacePosition, yPosition);
        
            SpriteRenderer WaterSurfaceSpriteRenderer = waterSurfaceTransform.GetComponent<SpriteRenderer>();
            WaterSurfaceSpriteRenderer.size = new Vector2(WATERSURFACE_WIDTH, WATERSURFACE_HEIGHT);

            BoxCollider2D waterSurfaceBoxCollider = waterSurfaceTransform.GetComponent<BoxCollider2D>();
            waterSurfaceBoxCollider.size = new Vector2(WATERSURFACE_WIDTH, WATERSURFACE_HEIGHT * .5f);
            waterSurfaceBoxCollider.offset = new Vector2(0f, - WATERSURFACE_HEIGHT * .5f);

            WaterSurface waterSurface = new WaterSurface(waterSurfaceTransform);
            waterSurfaceList.Add(waterSurface);

            leftMostWaterSurfacePosition -= WATERSURFACE_WIDTH;
        }
    }

    private void CreateWaterSurface(float xPosition, float yPosition)
    {
        // Set up a Water Surface tiled
        Transform waterSurfaceTransform = Instantiate(GameAssets.GetInstance().pfWaterSurface);
        waterSurfaceTransform.position = new Vector3(xPosition, yPosition);
        
        SpriteRenderer waterSurfaceSpriteRenderer = waterSurfaceTransform.GetComponent<SpriteRenderer>();
        waterSurfaceSpriteRenderer.size = new Vector2(WATERSURFACE_WIDTH, WATERSURFACE_HEIGHT);

        BoxCollider2D waterSurfaceBoxCollider = waterSurfaceTransform.GetComponent<BoxCollider2D>();
        waterSurfaceBoxCollider.size = new Vector2(WATERSURFACE_WIDTH, WATERSURFACE_HEIGHT* .5f);
        waterSurfaceBoxCollider.offset = new Vector2(0f, - WATERSURFACE_HEIGHT * .5f);

        WaterSurface waterSurface = new WaterSurface(waterSurfaceTransform);
        waterSurfaceList.Add(waterSurface);
    }
    
    /************************************ CREATION OF CORAL REEF ************************************/

    private void CreateInitialReef(float yPosition)
    {
        float leftMostReef = REEF_SPAWN_X_POSITION;
        //Creation of the initial reef line
        while (leftMostReef > REEF_DESTROY_X_POSITION)
        {
            Transform[] reefTransformsArray = GameAssets.GetInstance().pfReefArray;
            Transform reefTransform = Instantiate(reefTransformsArray[Random.Range(0, reefTransformsArray.Length)]);
            
            reefTransform.position = new Vector3(leftMostReef, yPosition);
            
            SpriteRenderer reefSpriteRenderer = reefTransform.GetComponent<SpriteRenderer>();
            reefSpriteRenderer.size = new Vector2(REEF_DIMENSION, REEF_DIMENSION);
        
            CircleCollider2D reefCircleCollider = reefTransform.GetComponent<CircleCollider2D>();
            reefCircleCollider.radius = REEF_DIMENSION * .5f;

            Reef reef = new Reef(reefTransform);
            reefList.Add(reef);

            leftMostReef -= REEF_DIMENSION;
        }
    }
    

    private void CreateReef(float xPosition, float yPosition)
    {
        Transform[] reefTransformsArray = GameAssets.GetInstance().pfReefArray;
        Transform reefTransform = Instantiate(reefTransformsArray[Random.Range(0, reefTransformsArray.Length)]);
        
        reefTransform.position = new Vector3(xPosition, yPosition);

        SpriteRenderer reefSpriteRenderer = reefTransform.GetComponent<SpriteRenderer>();
        reefSpriteRenderer.size = new Vector2(REEF_DIMENSION, REEF_DIMENSION);
        
        CircleCollider2D reefCircleCollider = reefTransform.GetComponent<CircleCollider2D>();
        reefCircleCollider.radius = REEF_DIMENSION * .5f;

        Reef reef = new Reef(reefTransform);
        reefList.Add(reef);
    }

    /****************************************************************************************************
    ************************************ Represent the Water Surface ************************************
    *****************************************************************************************************/
    
    private class WaterSurface
    {
        private Transform waterSurfaceTransform;

        public WaterSurface(Transform waterSurfaceTransform)
        {
            this.waterSurfaceTransform = waterSurfaceTransform;
        }

        public void Move()
        {
            waterSurfaceTransform.position += new Vector3(-1, 0, 0) * WATERSURFACE_MOVE_SPEED * Time.deltaTime;
        }
        
        public float GetXPosition()
        {
            return waterSurfaceTransform.position.x;
        }

        public void DestroySelf()
        {
            Destroy(waterSurfaceTransform.gameObject);
        }
    }
    
    /****************************************************************************************************
    ************************************** Represent the Coral Reef *************************************
    *****************************************************************************************************/

    private class Reef
    {
        private Transform reefTransform;

        public Reef(Transform reefTransform)
        {
            this.reefTransform = reefTransform;
        }

        public void Move()
        {
            reefTransform.position += new Vector3(-1, 0, 0) * REEF_MOVE_SPEED * Time.deltaTime;
        }

        public float GetXPosition()
        {
            return reefTransform.position.x;
        }

        public void DestroySelf()
        {
            Destroy(reefTransform.gameObject);
        }
    }

    /****************************************************************************************************
    ************************************ Represent a single pipe ****************************************
    *****************************************************************************************************/
    private class Pipe
    {
        private Transform pipeHeadTransform;
        private Transform pipeBodyTransform;

        public Pipe(Transform pipeHeadTransform, Transform pipeBodyTransform)
        {
            this.pipeHeadTransform = pipeHeadTransform;
            this.pipeBodyTransform = pipeBodyTransform;
        }

        public void Move()
        {
            pipeHeadTransform.position += new Vector3(-1, 0, 0) * PIPE_MOVE_SPEED * Time.deltaTime;
            pipeBodyTransform.position += new Vector3(-1, 0, 0) * PIPE_MOVE_SPEED * Time.deltaTime;

        }

        public float GetXPosition()
        {
            return pipeHeadTransform.position.x;
        }

        public void DestroySelf()
        {
            Destroy(pipeHeadTransform.gameObject);
            Destroy(pipeBodyTransform.gameObject);
        }
    }

}
