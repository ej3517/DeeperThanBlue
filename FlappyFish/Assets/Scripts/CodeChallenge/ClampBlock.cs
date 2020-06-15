
using UnityEngine;
using UnityEngine.UI;

public class ClampBlock : MonoBehaviour
{
    ScrollRect scroll;

    private void Awake()
    {
        scroll = GetComponent<ScrollRect>();
    }


    private void Update()
    {
        //Debug.Log(transform.position.x);
        if (transform.position.x > 250)
        {
            scroll.inertia = false;
            scroll.StopMovement();
            transform.position = new Vector3(250, transform.position.y, transform.position.z);
            scroll.inertia = true;
        }
        else if (transform.position.x < -250)
        {
            scroll.inertia = false;
            scroll.StopMovement();
            transform.position = new Vector3(-250, transform.position.y, transform.position.z);
            scroll.inertia = true;
        }
    }
}
