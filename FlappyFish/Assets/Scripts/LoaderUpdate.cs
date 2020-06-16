using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoaderUpdate : MonoBehaviour
{
    private DataController dataController;

    void Start()
    {
        dataController = FindObjectOfType<DataController>();
    }

    private void Update()
    {
        if (dataController.isFinishedFetching)
        {
            Loader.LoadTargetScene();
        }
    }
}
