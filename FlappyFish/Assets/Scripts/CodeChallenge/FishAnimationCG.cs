using System.Collections;
using UnityEngine;

public class FishAnimationCG : MonoBehaviour
{
    // Start is called before the first frame update

    public Transform Eye;
    public Transform Fin;
    public Transform Tail;

    //RectTransform Eye;
    //RectTransform Fin;
    //RectTransform Tail;
    void Awake()
    {
        //Eye = EyePF.GetComponent<RectTransform>();
        //Fin = FinPF.GetComponent<RectTransform>();
        //Tail = TailPF.GetComponent<RectTransform>();
        //Fin.localEulerAngles = new Vector3(0,0,350);
    }

    // Update is called once per frame
    void Update()
    {
       // MoveFin();
    }

    bool up;
    void MoveFin()
    {
        float maxAngle = 350;
        float minAngle = 325;
        float speed = 0.1f;

        if (Fin.localEulerAngles.z < minAngle && minAngle - 20 < Fin.localEulerAngles.z )
        {
            up = true;
        }
        else if (Fin.localEulerAngles.z-5 > maxAngle && maxAngle + 20 > Fin.localEulerAngles.z)
        {
            up = false;
        }
        if (up)
        {
            Fin.Rotate(0, 0, speed);
        }
        else
        {
            Fin.Rotate(0, 0, speed); //= Quaternion.Euler(0, 0, Fin.rotation.z - 0.01f);
        }
        Debug.Log($"{Fin.localEulerAngles.z} {up}");
    }
}
