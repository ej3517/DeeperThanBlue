using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleBoat : MonoBehaviour
{
    private const float boatXPosition = -20f; 
    private const float boatYPosition = 40f; 
    private static Vector2 screenBounds;

    public static Boat CreateBoat()
    {
        // set up boat
        Transform boatTransform = Instantiate(GameAssets.GetInstance().pfBoat);
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        boatTransform.position = new Vector3(-screenBounds.x, screenBounds.y - 10f);


        SpriteRenderer boatSpriteRenderer = boatTransform.GetComponent<SpriteRenderer>();
        boatSpriteRenderer.size = new Vector2(30, 30);


        Boat boat = new Boat(boatTransform);
        return boat; 
    }

    public class Boat 
    {
        public Transform boatTransform;

        public Boat(Transform boatTransform)
        {
            this.boatTransform = boatTransform;
        }

        public void Move(float speed)
        {
            boatTransform.position += new Vector3(-1, 0, 0) * speed * Time.deltaTime;
        }

        public float GetXPosition()
        {
            return boatTransform.position.x;
        }

        public void DestroySelf()
        {
            Destroy(boatTransform.gameObject);
        }
    }

}
