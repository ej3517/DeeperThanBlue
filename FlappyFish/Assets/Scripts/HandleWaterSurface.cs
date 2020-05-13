using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleWaterSurface : MonoBehaviour
{
    private const float WATERSURFACE_WIDTH = 20f;
    private const float WATERSURFACE_HEIGHT = 8f;
    private const float WATERSURFACE_DESTROY_X_POSITION = -120f;
    private const float WATERSURFACE_SPAWN_X_POSITION = 120f;
    
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
