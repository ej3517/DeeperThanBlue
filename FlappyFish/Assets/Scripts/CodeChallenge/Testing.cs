using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class Testing : MonoBehaviour
{
    // Start is called before the first frame update

    private Grid grid;
    public Transform canvas;
    void Start()
    {
        grid = new Grid(4, 5, 50f, canvas);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            grid.SetValue(UtilsClass.GetMouseWorldPosition(), 56);
        }
    }

}
