using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;
using CodeMonkey.Utils;

public class Level : MonoBehaviour
{
    private const float CAMERA_ORTHO_SIZE = 50f;
    private const float PIPE_WIDTH = 7.8f;
    private const float PIPE_HEAD_HEIGHT = 3.75f;
    private const float PIPE_MOVE_SPEED = 30f;
    private const float PIPE_DESTROY_X_POSITION = -100f;
    private const float PIPE_SPAWN_X_POSITION = 100f;
    private const float BIRD_X_POSITION = 0;

    private static Level instance;

    public static Level GetInstance()
    {
        return instance;
    }

    private List<QuestionBlob> questionBlobList;
    private List<Pipe> pipeList;
    private int pipesPassedCount;
    private int pipesSpawned;
    private float pipeSpawnTimer;
    private float pipeSpawnTimerMax;
    private float gapSize;
    private State state;

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
        pipeList = new List<Pipe>();
        questionBlobList = new List<QuestionBlob>();
        questionWindow = QuestionWindow.getInstance();
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
        questionWindow.Hide();
        //TODO: Delete question tokens?
    }

    private void Update()
    {
        if (state == State.Playing)
        {
            HandlePipeMovement();
            HandleQuestionMovement();
            HandlePipeSpawning();
            HandlePopupQuestion();
            //HandleQuestionSpawning();
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
        }
    }

    private void HandlePipeMovement()
    {
        for (int i = 0; i<pipeList.Count; i++)
        {
            Pipe pipe = pipeList[i];
            bool isRightToTheBird = pipe.getXPosition() > BIRD_X_POSITION;
            pipe.Move();
            if (isRightToTheBird && pipe.getXPosition() <= BIRD_X_POSITION && pipe.IsBottom())
            {
                // Pipe passed Bird
                SoundManager.PlaySound(SoundManager.Sound.Score);
                pipesPassedCount++;
            }
            if (pipe.getXPosition() < PIPE_DESTROY_X_POSITION)
            {
                // Destroy Pipe
                pipe.destroySelf();
                pipeList.Remove(pipe);
                i--;
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


    private void SpawnQuestion(float _height, float _position)
    {
        Transform _questionBlob = Instantiate(GameAssets.GetInstance().pfQuestionBlob);
        _questionBlob.position = new Vector3(_position, _height);
        QuestionBlob qb = new QuestionBlob(_questionBlob); 
        questionBlobList.Add(qb);
    }


    private void CreateGapPipes(float gapY, float gapSize, float xPosition)
    {
        CreatePipe(gapY - gapSize * .5f, xPosition, true);
        CreatePipe(CAMERA_ORTHO_SIZE*2f - gapY - gapSize * .5f, xPosition, false);
        pipesSpawned++;
        SetDifficulty(GetDifficulty());
    }

    private void CreatePipe(float height, float xPosition, bool createBottom)
    {
        // set up pipe head
        Transform pipeHead = Instantiate(GameAssets.GetInstance().pfPipeHead);
        float pipeHeadYPosition;
        if (createBottom)
        {
            pipeHeadYPosition = -CAMERA_ORTHO_SIZE + height - PIPE_HEAD_HEIGHT * .5f;
        }
        else
        {
            pipeHeadYPosition = +CAMERA_ORTHO_SIZE - height + PIPE_HEAD_HEIGHT * .5f;
        }
        pipeHead.position = new Vector3(xPosition, pipeHeadYPosition);

        // set up pipe body
        Transform pipeBody = Instantiate(GameAssets.GetInstance().pfPipeBody);

        float PipeBodyYPosition;
        if (createBottom)
        {
            PipeBodyYPosition = -CAMERA_ORTHO_SIZE;
        }
        else
        {
            PipeBodyYPosition = +CAMERA_ORTHO_SIZE;
            pipeBody.localScale = new Vector3(1, -1, 1);
        }
        pipeBody.position = new Vector3(xPosition, PipeBodyYPosition);

        SpriteRenderer pipeBodySpriteRenderer = pipeBody.GetComponent<SpriteRenderer>();
        pipeBodySpriteRenderer.size = new Vector2(PIPE_WIDTH, height);

        BoxCollider2D pipeBodyBoxCollider = pipeBody.GetComponent<BoxCollider2D>();
        pipeBodyBoxCollider.size = new Vector2(PIPE_WIDTH, height);
        pipeBodyBoxCollider.offset = new Vector2(0f, height * .5f);

        Pipe pipe = new Pipe(pipeHead, pipeBody, createBottom);
        pipeList.Add(pipe);
    }

    public int GetPipesScore()
    {
        return pipesSpawned;
    }

    public int GetPipesPassedCount()
    {
        return pipesPassedCount;
    }

    /*
     * represent a single pipe
     */
    private class Pipe
    {
        private Transform pipeHeadTransform;
        private Transform pipeBodyTransform;
        private bool isBottom;

        public Pipe(Transform pipeHeadTransform, Transform pipeBodyTransform, bool isBottom)
        {
            this.pipeHeadTransform = pipeHeadTransform;
            this.pipeBodyTransform = pipeBodyTransform;
            this.isBottom = isBottom;
        }

        public void Move()
        {
            pipeHeadTransform.position += new Vector3(-1, 0, 0) * PIPE_MOVE_SPEED * Time.deltaTime;
            pipeBodyTransform.position += new Vector3(-1, 0, 0) * PIPE_MOVE_SPEED * Time.deltaTime;

        }

        public float getXPosition()
        {
            return pipeHeadTransform.position.x;
        }

        public bool IsBottom()
        {
            return isBottom;
        }

        public void destroySelf()
        {
            Destroy(pipeHeadTransform.gameObject);
            Destroy(pipeBodyTransform.gameObject);
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

