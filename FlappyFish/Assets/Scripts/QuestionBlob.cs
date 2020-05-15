using System;
using UnityEngine;


public class QuestionBlob : MonoBehaviour
{
    private Transform questionTransform;
    public QuestionBlob(Transform _questionTransform)
    {
        questionTransform = _questionTransform;
    }

    public void Move(float speed)
    {
        questionTransform.position += new Vector3(-1, 0, 0) * speed * Time.deltaTime;   //RENAME MOVESPEED
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

    public void Hide()
    {
        questionTransform.localScale = new Vector3(1, 0, 0);
    }
}