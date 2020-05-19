using UnityEngine;
using CodeMonkey.Utils;

public class Grid
{
    private int width;
    private int height;
    private float cellSize;
    private int[,] gridArray;
    private Transform transform;


    public Grid(int _width, int _height, float _cellSize, Transform _transform)
    {
        width = _width;
        height = _height;
        cellSize = _cellSize;
        transform = _transform;

        gridArray = new int[width, height];
        
        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for(int y = 0; y< gridArray.GetLength(1); y++)
            {
                TextMesh textMesh = UtilsClass.CreateWorldText(gridArray[x,y].ToString(), transform, GetWorldPosition(x,y)+ new Vector3(cellSize,cellSize)/2, 40, Color.black, TextAnchor.MiddleCenter);
                textMesh.characterSize = 3;
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.black, 100f);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.black, 100f);
            }
        }
        Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.black, 100f);
        Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.black, 100f);

    }

    private Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize;
    }

    
}
