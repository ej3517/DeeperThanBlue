using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleObstacles : MonoBehaviour
{
    private const float GARBAGE_SPAWN_X_POSITION = 100f;
    private static Transform garbageTransform; 
    static System.Random random; 
    private static Vector2 screenBounds;
    private static List<Garbage> garbageList; 
    // Start is called before the first frame update
    void Start()
    {
        random = new System.Random();   
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public static void CreateGarbage(List<Garbage> listGarbage)
    {
        float yPosition; 
        bool canSpawnHere = false; 
        // set up new garbage element 
        while(!canSpawnHere)
        {
            switch (random.Next(0, 4))
            {
                case 0: garbageTransform = Instantiate(GameAssets.GetInstance().pfCup); break; 
                case 1: garbageTransform = Instantiate(GameAssets.GetInstance().pfGlass); break;
                case 2: garbageTransform = Instantiate(GameAssets.GetInstance().pfPlastic); break;
                case 3: garbageTransform = Instantiate(GameAssets.GetInstance().pfBottle); break;
                default: Debug.Log("Impossible"); break;
            }
            
            screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
            yPosition = UnityEngine.Random.Range(-screenBounds.y, screenBounds.y);
            garbageTransform.position = new Vector3(GARBAGE_SPAWN_X_POSITION, yPosition);


            SpriteRenderer garbageSpriteRenderer = garbageTransform.GetComponent<SpriteRenderer>();
            garbageSpriteRenderer.size = new Vector2(30, 30);

            Garbage garbage = new Garbage(garbageTransform); 

            canSpawnHere = PreventSpawnOverlap(garbageTransform); 

            if (canSpawnHere)
            {
                Debug.Log("Created Trash");
                
                garbageList.Add(garbage); 
                break;
            } 
            else {
                Debug.Log("Destroyed Ring"); 
                garbage.destroySelf();                
            }
        }
             
    }

    private static bool PreventSpawnOverlap(Transform tmpTransform)
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

    public class Garbage
    {
        public Transform garbageTransform;

        public void SpeedRing (Transform garbageTransform)
        {
            this.garbageTransform = garbageTransform; 
        }

        public void Move(float speed)
        {
            garbageTransform.position += new Vector3(-1, 0, 0) * speed * Time.deltaTime; 
        }

        public float getXPosition()
        {
            return garbageTransform.position.x; 
        }

        public void destroySelf()
        {
            Destroy(garbageTransform.gameObject); 
        }

        public void size(float x)
        {
            Vector3 vec = new Vector3(x, 1, 0); 
            garbageTransform.localScale = vec; 
        }
    }
    
}
