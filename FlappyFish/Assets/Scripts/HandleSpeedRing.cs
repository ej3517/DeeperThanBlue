using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleSpeedRing : MonoBehaviour
{
    // private Vector2 screenBounds;
    public static void CreateSpeedRing (float xPosition, List<SpeedRing> speedRingList)
    {
        bool canSpawnHere = false;

        // screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z)); 
        float yPosition; 
        Transform speedRingTransform = Instantiate(GameAssets.GetInstance().pfSpeedRing);
        while (!canSpawnHere)
        {
            // yPosition = Random.Range(-screenBounds.y, screenBounds.y); 
            yPosition = Random.Range(MyGlobals.MAX_HEIGHT_GROUND, MyGlobals.SURFACE_POSITION);

            
            speedRingTransform.position = new Vector3(xPosition - 20f, yPosition); 
            SpeedRing ring = new SpeedRing(speedRingTransform); 
            
            canSpawnHere = PreventSpawnOverlap(ring.speedRingTransform);
            if (canSpawnHere)
            {
                speedRingList.Add(ring); 
                break;
            } 
        }
    }
    
    private static bool PreventSpawnOverlap(Transform tmpTransform)
    { 
        Collider2D colliders;  
        colliders = Physics2D.OverlapBox(tmpTransform.position, tmpTransform.localScale * 2, 0f); 

        if (colliders == null)
        {
            return true; 
        }
        else
        {
            return false; 
        }
    }
    
    public class SpeedRing
    {
        public Transform speedRingTransform;

        public SpeedRing(Transform speedRingTransform)
        {
            this.speedRingTransform = speedRingTransform; 
        }

        public void Move(float speed)
        {
            speedRingTransform.position += new Vector3(-1, 0, 0) * speed * Time.deltaTime; 
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
}
