using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleReef : MonoBehaviour
{
    private const float REEF_DIMENSION = 12f;
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
            reefCircleCollider.radius = REEF_DIMENSION * .4f;

            Transform[] reefArray = {reefTransform};
            Reef reef = new Reef(reefArray);
            reefList.Add(reef);

            leftMostReef -= REEF_DIMENSION * 0.75f;
        }
    }
    
    public static void CreateReef(float xPosition, float yPosition, List<Reef> reefList, int pipeHeight)
    {
        Transform[] reefTransformsArray = GameAssets.GetInstance().pfReefArray;
        Transform[] reefPipeArray = new Transform[pipeHeight];
        
        for (int i = 0; i < pipeHeight; i++)
        {
            Transform reefTransform = Instantiate(reefTransformsArray[Random.Range(0, reefTransformsArray.Length)]);
            reefTransform.position = new Vector3(xPosition, yPosition + i * REEF_DIMENSION * .75f);

            SpriteRenderer reefSpriteRenderer = reefTransform.GetComponent<SpriteRenderer>();
            reefSpriteRenderer.size = new Vector2(REEF_DIMENSION, REEF_DIMENSION);

            CircleCollider2D reefCircleCollider = reefTransform.GetComponent<CircleCollider2D>();
            reefCircleCollider.radius = REEF_DIMENSION * .4f;

            reefPipeArray[i] = reefTransform;
        }
        
        Reef reef = new Reef(reefPipeArray);
        reefList.Add(reef);
    }
    
    public class Reef
    {
        private Transform[] reefTransformArray;

        public Reef(Transform[] reefTransformArray)
        {
            this.reefTransformArray = reefTransformArray;
        }

        public void Move(float speed)
        {
            foreach (Transform reefTransform in reefTransformArray)
            {
                reefTransform.position += new Vector3(-1, 0, 0) * speed * Time.deltaTime;
            }
        }

        public float GetXPosition()
        {
            return reefTransformArray[0].position.x;
        }

        public void DestroySelf()
        {
            foreach (Transform reefTransform in reefTransformArray)
            {
                Destroy(reefTransform.gameObject);
            }
        }
    }
}
