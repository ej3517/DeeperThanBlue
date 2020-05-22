using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAreaGrid : MonoBehaviour
{
    // Start is called before the first frame update

    private RectTransform rectTransform;
    private char[,] map;
    private int gridSize;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        //Load Map --- Ratio 16:9
        int map_width, map_height;
        string[] lines = System.IO.File.ReadAllLines("game1.map");
        if (lines == null)
            throw new System.InvalidOperationException("Could not load game1.map");
        map_height = lines.Length;
        map_width = lines[0].Length;

        map = new char[map_width, map_height];
        int y = 0;
        foreach (string line in lines)
        {
            int x = 0;
            foreach (char c in line)
            {
                map[x, y] = c;
                x++;
            }
            y++;
        }


        // Scale gridsize
        float ratio = rectTransform.sizeDelta.x / map_width;
        float deltaSize = rectTransform.sizeDelta.y - (ratio * map_height);
        Debug.LogError("CHECK "+ ratio.ToString()+ " " + deltaSize.ToString());
        if(deltaSize >= 0)
        {
            gridSize = Mathf.FloorToInt(ratio);
        }
        else
        {
            ratio = rectTransform.sizeDelta.y / map_height;
            deltaSize = rectTransform.sizeDelta.x - (ratio * map_width);
            Debug.LogError("CHECK " + ratio.ToString() + " " + deltaSize.ToString());
            gridSize = Mathf.FloorToInt(ratio);
        }

        

        // DrawGrid


        // Load Map onto Grid
    }


    // Update positions on events?



}
