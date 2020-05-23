using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAreaGrid : MonoBehaviour
{
    enum Type
    {
        Empty,
        Block,
        Boat,
        End
    };

    private struct GridStruct
    {
        public Type type;
        public Transform grid;
        public Transform content;
    };

    // Start is called before the first frame update
    public Transform gridTransform;
    public Transform block;
    private RectTransform rectTransform;
    private GridStruct[,] map;
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
        char[,] _map;
        _map = new char[map_width, map_height];
        map = new GridStruct[map_width, map_height];
        int y = 0;
        foreach (string line in lines)
        {
            int x = 0;
            foreach (char c in line)
            {
                _map[x, y] = c;
                x++;
            }
            y++;
        }


        // Scale gridsize
        float ratio = rectTransform.sizeDelta.x / map_width;
        float deltaSize_y = rectTransform.sizeDelta.y - (ratio * map_height);
        float deltaSize_x = 0;
        if (deltaSize_y >= 0)
        {
            gridSize = Mathf.FloorToInt(ratio);
        }
        else
        {
            ratio = rectTransform.sizeDelta.y / map_height;
            deltaSize_x = rectTransform.sizeDelta.x - (ratio * map_width);
            deltaSize_y = 0;
            gridSize = Mathf.FloorToInt(ratio);
        }
        
        //Grid position offset
        float x_offset = (-rectTransform.sizeDelta.x + deltaSize_x + ratio) * 0.5f;
        float y_offset = (-rectTransform.sizeDelta.y + deltaSize_y + ratio) * 0.5f;
        
        // Load Map onto Grid
        for (int h = 0; h < map_height; h++)
        {
            for(int w = 0; w < map_width; w++)
            {
                //Create
                Transform instance= Instantiate(gridTransform);
                instance.parent = transform;
                
                //Scale
                SpriteRenderer sr = instance.GetComponent<SpriteRenderer>();
                instance.localScale = new Vector3(ratio/ sr.size.x, ratio/ sr.size.y, 1);

                //Postion
                instance.localPosition = new Vector3(w* ratio+ x_offset, h* ratio+ y_offset, 0);
                
                map[w, h].grid = instance;
                map[w, h].type = Type.Empty;

                if (_map[w, h] == 'x')
                {
                    //Summon block
                    Transform _block = Instantiate(block);
                    SpriteRenderer srBlock = _block.GetComponent<SpriteRenderer>();
                    float _scale = 0.5f;
                    _block.localScale = new Vector3(ratio * _scale / srBlock.size.x, ratio* _scale / srBlock.size.y, 1);
                    _block.parent = instance;
                    _block.localPosition = new Vector3(0, 0, 0);
                    map[w, h].content = _block;
                    map[w, h].type = Type.Block;
                }

            }
        }
        Debug.LogError("Done creating grid");

    }


    // Update positions on events?



}
