using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : MonoBehaviour
{
    private Transform pipeHeadTransform;
    private Transform pipeBodyTransform;

    public Pipe(Transform pipeHeadTransform, Transform pipeBodyTransform)
    {
        this.pipeHeadTransform = pipeHeadTransform;
        this.pipeBodyTransform = pipeBodyTransform;
    }

    public void Move(float pipe_move_speed)
    {
        pipeHeadTransform.position += new Vector3(-1, 0, 0) * pipe_move_speed * Time.deltaTime;
        pipeBodyTransform.position += new Vector3(-1, 0, 0) * pipe_move_speed * Time.deltaTime;

    }

    public float GetXPosition()
    {
        return pipeHeadTransform.position.x;
    }

    public void DestroySelf()
    {
        Destroy(pipeHeadTransform.gameObject);
        Destroy(pipeBodyTransform.gameObject);
    }
}
