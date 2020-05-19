using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleObstacles : MonoBehaviour
{
    private const float GARBAGE_SPAWN_X_POSITION = 120f;
    private static Transform garbageTransform; 
    static System.Random random; 
    private static Vector2 screenBounds;
    private static List<Garbage> garbageList; 
    private static LayerMask m_LayerMask;
    // Start is called before the first frame update
    void Start()
    {
        random = new System.Random();  
        m_LayerMask = LayerMask.GetMask("Objects"); 
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
       
       
        switch (UnityEngine.Random.Range(0,4))
        {
            case 0: garbageTransform = Instantiate(GameAssets.GetInstance().pfCup); break; 
            case 1: garbageTransform = Instantiate(GameAssets.GetInstance().pfGlass); break;
            case 2: garbageTransform = Instantiate(GameAssets.GetInstance().pfPlastic); break;
            case 3: garbageTransform = Instantiate(GameAssets.GetInstance().pfBottle); break;
            default: Debug.Log("Impossible"); break;
        }
            
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z)); 

        while(!canSpawnHere)
        {
            
            yPosition = UnityEngine.Random.Range(-screenBounds.y, screenBounds.y);
            garbageTransform.position = new Vector3(GARBAGE_SPAWN_X_POSITION - 20f, yPosition);
            garbageTransform.rotation = Quaternion.Euler(0, 0, UnityEngine.Random.Range(0f, 360f)); 


            //SpriteRenderer garbageSpriteRenderer = garbageTransform.GetComponent<SpriteRenderer>();
            //garbageSpriteRenderer.size = new Vector2(30, 30);

            Garbage garbage = new Garbage(garbageTransform); 
            
            canSpawnHere = PreventSpawnOverlap(garbageTransform); 
       
            if (canSpawnHere)
            {
                Debug.Log("Created Trash");
                
                listGarbage.Add(garbage); 
                break;
            } 
        }
             
    }

    private static bool PreventSpawnOverlap(Transform tmpTransform)
    { 
        Collider2D colliders;  
        colliders = Physics2D.OverlapBox(tmpTransform.position, tmpTransform.localScale, 0f, m_LayerMask); 
        Debug.Log(tmpTransform.localScale);
        if (colliders == null)
        {
            return true; 
        }
        else
        {
            Debug.Log("Can't spawn");
            return false; 
        }
    }

    public class Garbage
    {
        public Transform garbageTransform;

        public Garbage(Transform garbageTransform)
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
