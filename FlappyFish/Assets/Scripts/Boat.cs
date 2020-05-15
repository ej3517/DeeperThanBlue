using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boat : MonoBehaviour
{
    public float boatSpeed = 5f;
    private Rigidbody2D boatRigidBody;
    
    private void Awake()
    {
        boatRigidBody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Move(boatSpeed);
    }

    private void Move(float horizontalSpeed)
    {
        boatRigidBody.velocity = Vector2.right * horizontalSpeed;
    }
}
