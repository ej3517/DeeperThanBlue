using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleReef : MonoBehaviour
{
    private const float REEF_DIMENSION = 14f;
    private const float REEF_DESTROY_X_POSITION = -120f;
    private const float REEF_SPAWN_X_POSITION = 120f;
    
    public static void CreateInitialReef(float yPosition, List<Reef> reefList)
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
    
    public static void CreateReef(float xPosition, float yPosition, List<Reef> reefList)
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
    
    public class Reef
    {
        private Transform reefTransform;

        public Reef(Transform reefTransform)
        {
            this.reefTransform = reefTransform;
        }

        public void Move(float speed)
        {
            reefTransform.position += new Vector3(-1, 0, 0) * speed * Time.deltaTime;
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
}
