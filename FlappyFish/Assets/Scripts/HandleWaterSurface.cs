using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleWaterSurface : MonoBehaviour
{
    private const float WATERSURFACE_WIDTH = 20f;
    private const float WATERSURFACE_HEIGHT = 8f;
    private const float WATERSURFACE_DESTROY_X_POSITION = -120f;
    private const float WATERSURFACE_SPAWN_X_POSITION = 120f;
    
    public static void CreateInitialWaterSurface(float yPosition, List<WaterSurface> waterSurfaceList)
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

    public static void CreateWaterSurface(float xPosition, float yPosition, List<WaterSurface> waterSurfaceList)
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
    
    public class WaterSurface
    {
        private Transform waterSurfaceTransform;

        public WaterSurface(Transform waterSurfaceTransform)
        {
            this.waterSurfaceTransform = waterSurfaceTransform;
        }

        public void Move(float speed)
        {
            waterSurfaceTransform.position += new Vector3(-1, 0, 0) * speed * Time.deltaTime;
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
}
