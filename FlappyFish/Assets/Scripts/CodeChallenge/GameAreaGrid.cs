using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class GameAreaGrid : MonoBehaviour
{
    enum Type
    {
        Empty,
        Block,
        Fish,
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
    public Transform fish;
    public Transform goal;

    private RectTransform rectTransform;
    private GridStruct[,] map;
    private int gridSize;

    public Transform codingAreaReferenceTransform;
    private CodingArea codingArea;

    public Transform[] pfReefArray;

    Vector2Int fishPosition;
    
    class Direction
    {
        private float dir = 0;

        public Vector2Int getDirection()
        {
            switch(dir)
            {
                case 0:
                    return new Vector2Int(1, 0);
                case 1:
                    return new Vector2Int(0, 1);
                case 2:
                    return new Vector2Int(-1, 0);
                case 3:
                    return new Vector2Int(0, -1);
                default:
                    //Should not happen
                    return new Vector2Int(-1, -1);
             }
        }
        public float getAngle()
        {
            switch (dir)
            {
                case 0:
                    return 0;
                case 1:
                    return 90;
                case 2:
                    return 180;
                case 3:
                    return -90;
                default:
                    //Should not happen
                    return -1;
            }
        }
        public void rotateLeft()
        {
            dir += 1;
            if(dir >3)
            {
                dir -= 4;
            }
        }
        public void rotateRight()
        {
            dir -= 1;
            if (dir < 0)
            {
                dir += 4;
            }
        }

    }
    Direction fishDirection;

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
            for (int w = 0; w < map_width; w++)
            {
                //Create
                Transform instance = Instantiate(gridTransform);
                instance.SetParent(transform);

                //Scale
                SpriteRenderer sr = instance.GetComponent<SpriteRenderer>();
                instance.localScale = new Vector3(ratio / sr.size.x, ratio / sr.size.y, 1);

                //Postion
                instance.localPosition = new Vector3(w * ratio + x_offset, -h * ratio - y_offset, 0);

                map[w, h].grid = instance;
                map[w, h].type = Type.Empty;

                if (_map[w, h] == 'x')
                {
                    //Summon block
                    Transform _block = Instantiate(pfReefArray[UnityEngine.Random.Range(0, pfReefArray.Length)]);
                    SpriteRenderer srBlock = _block.GetComponent<SpriteRenderer>();
                    float _scale = 0.9f;
                    _block.localScale = new Vector3(ratio * _scale / srBlock.size.x, ratio * _scale / srBlock.size.y, 1);
                    _block.parent = instance;
                    _block.localPosition = new Vector3(0, 0, 0);
                    map[w, h].content = _block;
                    map[w, h].type = Type.Block;
                }
                if (_map[w, h] == 's')
                {
                    Transform _fish = Instantiate(fish);
                    SpriteRenderer srBlock = _fish.GetComponent<SpriteRenderer>();
                    float _scale = 1f;
                    _fish.localScale = new Vector3(ratio * _scale / srBlock.size.x, ratio * _scale / srBlock.size.y, 1);
                    _fish.parent = instance;
                    _fish.localPosition = new Vector3(0, 0, 0);
                    map[w, h].content = _fish;
                    map[w, h].type = Type.Fish;
                    fishPosition = new Vector2Int(w, h);
                    fishDirection = new Direction();
                }

            }
        }
        Debug.Log("Created grid...");


        codingArea = codingAreaReferenceTransform.GetComponent<CodingArea>();
        codingArea.OnButtonStart += CodingArea_OnButtonStart;
        codingArea.ActionEvent += CodingArea_ActionEvent;
    }


    // Update positions on events? Place the events class in the grid GameArea bject
    private void CodingArea_OnButtonStart(object sender, EventArgs e)
    {
        Debug.Log("Start button pressed");
    }

    private void CodingArea_ActionEvent(object sender, CodingArea.CodingArgs arg)
    {
        Debug.LogWarning("Test" + arg.instructionType);
        switch (arg.instructionType)
        {
            case CodingArea.BlockCommand.Forward:
                //Check if block in front is Air
                Vector2Int blockInFront = fishPosition + fishDirection.getDirection();
                    Debug.LogWarning("Check cell " + blockInFront);
                if(map[blockInFront.x,blockInFront.y].type == Type.Block)
                {
                    Debug.LogError("Invalid movement");
                }
                else
                {
                    map[fishPosition.x, fishPosition.y].type = Type.Empty;
                    //Destroy old fish
                    Transform fish = map[fishPosition.x, fishPosition.y].content;
                    map[fishPosition.x, fishPosition.y].content = null;
                    //Move fish
                    fishPosition = blockInFront;
                    fish.SetParent(map[fishPosition.x, fishPosition.y].grid);
                    fish.localPosition = new Vector3(0, 0, 0);
                    map[fishPosition.x, fishPosition.y].type = Type.Fish;
                    map[fishPosition.x, fishPosition.y].content = fish;
                    //Create new fish
                }
                //Move display of fish
                break;
            case CodingArea.BlockCommand.TurnLeft:
                fishDirection.rotateLeft();
                Debug.LogError("Direction " + fishDirection.getDirection());
                break;
            case CodingArea.BlockCommand.TurnRight:
                fishDirection.rotateRight();
                break;
            default:
                Debug.LogError("Invalid event type");
                break;

        }
    }


}
