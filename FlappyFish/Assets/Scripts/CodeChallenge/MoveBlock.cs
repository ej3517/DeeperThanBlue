using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBlock : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Transform blocksTransform = transform.Find("Blocks");
        Transform codeTransform = transform.Find("CodingArea");
        Transform start = blocksTransform.Find("CONTROL: Start");
        start.transform.SetParent(codeTransform);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
