using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsometricPlayerMovementController : MonoBehaviour
{

    public float movementSpeed = 1f;
    //IsometricCharacterRenderer isoRenderer;

    Rigidbody2D rbody;

    private void Awake()
    {
        rbody = GetComponent<Rigidbody2D>();
        //isoRenderer = GetComponentInChildren<IsometricCharacterRenderer>();
    }
    
    enum direction
    {
        north,
        east,
        south,
        west,
    };

    private float angle = 0;

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 currentPos = rbody.position;
        float currentAng = rbody.rotation;
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector2 inputVector = new Vector2(horizontalInput, verticalInput);
        inputVector = Vector2.ClampMagnitude(inputVector, 1);
        Vector2 movement = inputVector * movementSpeed;
        Vector2 newPos = currentPos + movement * Time.fixedDeltaTime;
        //isoRenderer.SetDirection(movement);
        if (horizontalInput > 0 && verticalInput > 0) angle = -45;
        else if (horizontalInput < 0 && verticalInput > 0) angle = 45;
        else if (horizontalInput > 0 && verticalInput < 0) angle = -135;
        else if (horizontalInput < 0 && verticalInput < 0) angle = 135;
        else if (horizontalInput > 0) angle = -90;
        else if (horizontalInput < 0) angle = 90;
        else if (verticalInput > 0) angle = 0;
        else if (verticalInput < 0) angle = 180;




        rbody.SetRotation(angle);
        rbody.MovePosition(newPos);
    }
}
