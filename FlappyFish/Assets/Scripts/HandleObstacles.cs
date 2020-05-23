using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleObstacles : MonoBehaviour
{
    private static Transform garbageTransform;
    private static List<Garbage> garbageList; 
    private static LayerMask m_LayerMask;
    // Start is called before the first frame update
    void Start()
    {
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

        switch (Random.Range(0,4))
        {
            case 0: garbageTransform = Instantiate(GameAssets.GetInstance().pfCup); break; 
            case 1: garbageTransform = Instantiate(GameAssets.GetInstance().pfGlass); break;
            case 2: garbageTransform = Instantiate(GameAssets.GetInstance().pfPlastic); break;
            case 3: garbageTransform = Instantiate(GameAssets.GetInstance().pfBottle); break;
            default: Debug.Log("Impossible"); break;
        }
       
        while(!canSpawnHere)
        {
            
            // yPosition = Random.Range(-screenBounds.y, screenBounds.y);
            yPosition = Random.Range(MyGlobals.MAX_HEIGHT_GROUND, MyGlobals.SURFACE_POSITION);
            garbageTransform.position = new Vector3(MyGlobals.SPAWN_X_POSITION - 20f, yPosition);
            garbageTransform.rotation = Quaternion.Euler(0, 0, UnityEngine.Random.Range(0f, 360f)); 
            
            Garbage garbage = new Garbage(garbageTransform); 
            
            canSpawnHere = PreventSpawnOverlap(garbageTransform); 
       
            if (canSpawnHere)
            {
                listGarbage.Add(garbage); 
                break;
            } 
        }
             
    }

    private static bool PreventSpawnOverlap(Transform tmpTransform)
    { 
        Collider2D colliders;  
        colliders = Physics2D.OverlapBox(tmpTransform.position, tmpTransform.localScale, 0f, m_LayerMask);
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

        public Garbage(Transform garbageTransform)
        {
            this.garbageTransform = garbageTransform; 
        }

        public void Move(float speed)
        {
            garbageTransform.position += new Vector3(-1, 0, 0) * speed * Time.deltaTime; 
        }

        public float GetXPosition()
        {
            return garbageTransform.position.x; 
        }

        public void DestroySelf()
        {
            Destroy(garbageTransform.gameObject); 
        }
        
    }
    
}
