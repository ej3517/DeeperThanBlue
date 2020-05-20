using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleQuestionBlob : MonoBehaviour
{
    public static void SpawnQuestion(float _height, float _position, List<QuestionBlob> questionBlobList)
    {
        Transform _questionBlob = Instantiate(GameAssets.GetInstance().pfQuestionBlob);
        _questionBlob.position = new Vector3(_position, _height);
        QuestionBlob qb = new QuestionBlob(_questionBlob);
        questionBlobList.Add(qb);
    }
    
    public class QuestionBlob
    {
        private Transform questionTransform;
        public QuestionBlob(Transform questionTransform)
        {
            this.questionTransform = questionTransform;
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
}
