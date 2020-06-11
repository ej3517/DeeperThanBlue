using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private void FixedUpdate()
    {
        SendInputToServer();
    }

    private static Rigidbody2D rbody;
    private static float _angle = 0;


    private void Awake()
    {
        rbody = GetComponent<Rigidbody2D>();
    }

    private void SendInputToServer()
    {
        bool[] _inputs = new bool[]
        {
            Input.GetKey(KeyCode.W),
            Input.GetKey(KeyCode.S),
            Input.GetKey(KeyCode.A),
            Input.GetKey(KeyCode.D)
        };

        if (_inputs[2] && _inputs[0]) _angle = -45;
        else if (_inputs[3] && _inputs[0]) _angle = 45;
        else if (_inputs[2] && _inputs[1]) _angle = -135;
        else if (_inputs[3] && _inputs[1]) _angle = 135;
        else if (_inputs[2]) _angle = -90;
        else if (_inputs[3]) _angle = 90;
        else if (_inputs[0]) _angle = 0;
        else if (_inputs[1]) _angle = 180;

        rbody.SetRotation(_angle);

        ClientSend.PlayerMovement(_inputs);
    }
}
