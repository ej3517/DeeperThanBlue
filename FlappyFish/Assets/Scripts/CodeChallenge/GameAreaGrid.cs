﻿using System;
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

    public Transform goalWindow;

    private RectTransform rectTransform;
    private GridStruct[,] map;
    private int gridSize;

    public Transform codingAreaReferenceTransform;
    private CodingArea codingArea;

    public Transform[] pfReefArray;

    public AudioClip audioWin;
    public AudioClip audioLose;

    AudioSource audioSource;


    Vector2Int fishPosition;
    Vector2Int startPosition;

    class Direction
    {
        private float dir = 0;
        public void setAngle(float _dir)
        {
            dir = _dir;
        }

        public Vector2Int getDirection()
        {
            switch (dir)
            {
                case 0:
                    return new Vector2Int(1, 0);
                case 1:
                    return new Vector2Int(0, -1);
                case 2:
                    return new Vector2Int(-1, 0);
                case 3:
                    return new Vector2Int(0, 1);
                default:
                    //Should not happen
                    return new Vector2Int(-1, -1);
            }
        }
        public Quaternion getAngle()
        {
            switch (dir)
            {
                case 0:
                    return Quaternion.Euler(0,0,0);
                case 1:
                    return Quaternion.Euler(0, 0, 90);
                case 2:
                    return Quaternion.Euler(0, 180, 0); ;
                case 3:
                    return Quaternion.Euler(0, 0, -90);
                default:
                    //Should not happen
                    return Quaternion.Euler(0, 0, 0);
            }
        }
        public void rotateLeft()
        {
            dir += 1;
            if (dir > 3)
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

    int map_width, map_height;
    float ratio;
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        rectTransform = GetComponent<RectTransform>();
        //Load Map --- Ratio 16:9

        string[] lines = Maps.GetRandomMap(); //System.IO.File.ReadAllLines("game2.map");
        if (lines == null)
            throw new System.InvalidOperationException("Could not load game map");
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
        ratio = rectTransform.sizeDelta.x / map_width;
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

        // Draw Grid
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
                instance.localPosition = new Vector3(w * ratio + x_offset, -h * ratio - y_offset, -1);

                map[w, h].grid = instance;
                map[w, h].type = Type.Empty;
                if (_map[w, h] == 'x')
                {
                    //Summon block
                    Transform _block = Instantiate(pfReefArray[UnityEngine.Random.Range(0, pfReefArray.Length)]);
                    SpriteRenderer srBlock = _block.GetComponent<SpriteRenderer>();
                    float _scale = 0.9f;
                    _block.localScale = new Vector3(ratio * _scale / srBlock.size.x, ratio * _scale / srBlock.size.y, 1);
                    _block.parent = map[w, h].grid;
                    _block.localPosition = new Vector3(0, 0, 0);
                    map[w, h].content = _block;
                    map[w, h].type = Type.Block;
                }
                if (_map[w, h] == 's')
                {
                    Transform _fish = Instantiate(fish);
                    SpriteRenderer srBlock = _fish.GetComponent<SpriteRenderer>();
                    float _scale = 0.6f;
                    _fish.localScale = new Vector3(ratio * _scale / srBlock.size.x, ratio * _scale / srBlock.size.y, 1);
                    _fish.parent = map[w, h].grid;
                    _fish.localPosition = new Vector3(0, 0, 0);
                    map[w, h].content = _fish;
                    map[w, h].type = Type.Fish;
                    fishPosition = new Vector2Int(w, h);
                    startPosition = new Vector2Int(w, h);
                    fishDirection = new Direction();
                }
                if (_map[w, h] == 'e')
                {
                    Transform _end = Instantiate(goal);
                    SpriteRenderer srBlock = _end.GetComponent<SpriteRenderer>();
                    float _scale = 0.7f;
                    _end.localScale = new Vector3(ratio * _scale / srBlock.size.x, ratio * _scale / srBlock.size.y, 1);
                    _end.parent = map[w, h].grid;
                    _end.localPosition = new Vector3(0, 0, 0);
                    map[w, h].content = _end;
                    map[w, h].type = Type.End;

                }
            }
        }

        Debug.Log("Created grid...");


        codingArea = codingAreaReferenceTransform.GetComponent<CodingArea>();
        codingArea.OnButtonStart += CodingArea_OnButtonStart;
        //codingArea.ActionEvent += CodingArea_ActionEvent;
        codingArea.ResetEvent += CodingArea_ResetEvent;
    }


    // Update positions on events? Place the events class in the grid GameArea bject
    private void CodingArea_OnButtonStart(object sender, EventArgs e)
    {
        Debug.Log("Start button pressed");
    }

    public bool CodingAreaInstruction(CodingArea.BlockCommand instructionType)
    {
        bool validMove = true;
        switch (instructionType)
        {
            case CodingArea.BlockCommand.Forward:
                //Check if block in front is Air
                bool end = false ;
                Vector2Int blockInFront = fishPosition + fishDirection.getDirection();
                if(blockInFront.x < 0 || blockInFront.x >= map_width || blockInFront.y < 0 || blockInFront.y >= map_height)
                {
                    Debug.LogError("Invalid movement");
                    PlayInvalidMove();
                    validMove = false;
                }
                else if (map[blockInFront.x, blockInFront.y].type == Type.Block)
                {
                    Debug.LogError("Invalid movement");
                    PlayInvalidMove();
                    validMove = false;
                }
                else if (map[blockInFront.x, blockInFront.y].type == Type.End)
                {
                    end = true;
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
                    map[fishPosition.x, fishPosition.y].content = fish;
                    map[fishPosition.x, fishPosition.y].type = Type.Fish;
                    //Create new fish
                }
                if(end)
                {
                    Debug.Log("Reached target");
                    goalWindow.localPosition = new Vector3(0, 0, -100);
                    PlayEnd();
                    validMove = false;
                }
                
                //Move display of fish
                break;
            case CodingArea.BlockCommand.TurnLeft:
                fishDirection.rotateLeft();

                map[fishPosition.x, fishPosition.y].content.localRotation = fishDirection.getAngle();
                break;
            case CodingArea.BlockCommand.TurnRight:
                fishDirection.rotateRight();
                map[fishPosition.x, fishPosition.y].content.localRotation = fishDirection.getAngle();
                break;
            default:
                Debug.LogError("Invalid event type");
                break;

        }
        return validMove;
    }

    private void CodingArea_ResetEvent(object sender, EventArgs e)
    {
        Transform _fish = map[fishPosition.x, fishPosition.y].content;
        map[fishPosition.x, fishPosition.y].type = Type.Empty;
        map[fishPosition.x, fishPosition.y].content = null;
        _fish.SetParent(map[startPosition.x, startPosition.y].grid);
        _fish.localPosition = new Vector3(0, 0, 0);
        fishDirection.setAngle(0);
        _fish.localRotation = fishDirection.getAngle();
        map[startPosition.x, startPosition.y].content = _fish;
        map[startPosition.x, startPosition.y].type = Type.Fish;
        fishPosition = startPosition;


    }

    private void PlayEnd()
    {
        audioSource.PlayOneShot(audioWin, 0.7F);
        StartCoroutine(FlashColour(new Color(0, 1, 0), 1));
    }
    private void PlayInvalidMove()
    {
        audioSource.PlayOneShot(audioLose, 0.7F);
        StartCoroutine(FlashColour(new Color(1, 0, 0), 1));
    }

    private IEnumerator FlashColour(Color c, float duration)
    {
        for (int h = 0; h < map_height; h++)
        {
            for (int w = 0; w < map_width; w++)
            {
                map[w, h].grid.GetComponent<GridBackground>().SetPause(true);
                map[w, h].grid.GetComponent<SpriteRenderer>().color = c;
            }
        }
        yield return new WaitForSeconds(duration);
        for (int h = 0; h < map_height; h++)
        {
            for (int w = 0; w < map_width; w++)
            {
                map[w, h].grid.GetComponent<GridBackground>().SetPause(false);
                map[w, h].grid.GetComponent<SpriteRenderer>().color = new Color(0.142f, 0.5182321f, 1); ;
            }
        }
    }

}
