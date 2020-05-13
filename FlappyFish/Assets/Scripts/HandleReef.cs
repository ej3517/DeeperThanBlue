using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleReef : MonoBehaviour
{
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
