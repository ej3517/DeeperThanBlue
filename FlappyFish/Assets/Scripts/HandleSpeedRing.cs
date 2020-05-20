using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleSpeedRing : MonoBehaviour
{
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
