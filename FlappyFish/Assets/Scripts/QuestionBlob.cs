using System;
using UnityEngine;


public class QuestionBlob : MonoBehaviour
{
    private Transform questionTransform;
    public QuestionBlob(Transform _questionTransform)
    {
        questionTransform = _questionTransform;
    }

    public void Move(float PIPE_MOVE_SPEED)
    {
        questionTransform.position += new Vector3(-1, 0, 0) * PIPE_MOVE_SPEED * Time.deltaTime;        //RENAME MOVESPEED
    }

    public float getXPosition()
    {
        return questionTransform.position.x;
    }
    public float getDistance(Vector3 _from)
    {
        return Vector3.Distance(_from, questionTransform.position);
    }

    public void destroySelf()
    {
        Destroy(questionTransform.gameObject);
    }

}