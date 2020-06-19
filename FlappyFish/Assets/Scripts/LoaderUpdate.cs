using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoaderUpdate : MonoBehaviour
{
    private DataController dataController;

    void Awake()
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

    //TODO: Add progress bar (maybe slight animation to the text so that it doens't look like it has frozen)
}
