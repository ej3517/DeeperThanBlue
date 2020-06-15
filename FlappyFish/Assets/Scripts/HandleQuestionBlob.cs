using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleQuestionBlob : MonoBehaviour
{
    private const float DEADLY_COIN_DIAMETER = 1.5f;
    
    public static void SpawnQuestion(float _height, float _position, List<QuestionBlob> questionBlobList)
    {
        Transform questionBlobTransform = Instantiate(GameAssets.GetInstance().pfQuestionBlob);
        questionBlobTransform.position = new Vector3(_position, _height);
        
        SpriteRenderer reefSpriteRenderer = questionBlobTransform.GetComponent<SpriteRenderer>();
        reefSpriteRenderer.size = new Vector2(DEADLY_COIN_DIAMETER, DEADLY_COIN_DIAMETER);

        CircleCollider2D reefCircleCollider = questionBlobTransform.GetComponent<CircleCollider2D>();
        reefCircleCollider.radius = DEADLY_COIN_DIAMETER * .5f;
        
        QuestionBlob qb = new QuestionBlob(questionBlobTransform);
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

        public float GetXPosition()
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
