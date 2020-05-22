using UnityEngine;
using CodeMonkey.Utils;

public class Grid
{
    private int width;
    private int height;
    private float cellSize;
    private int[,] gridArray;
    private TextMesh[,] debugTextArray;

    private Transform transform;


    public Grid(int _width, int _height, float _cellSize, Transform _transform)
    {
        width = _width;
        height = _height;
        cellSize = _cellSize;
        transform = _transform;

        gridArray = new int[width, height];
        debugTextArray = new TextMesh[width, height];
        
        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for(int y = 0; y< gridArray.GetLength(1); y++)
            {
                debugTextArray[x, y] = UtilsClass.CreateWorldText(gridArray[x, y].ToString(), null, GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) / 2, 40, Color.black, TextAnchor.MiddleCenter);
                debugTextArray[x, y].characterSize = 3;
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.black, 100f);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.black, 100f);
            }
        }
        Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.black, 100f);
        Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.black, 100f);

        SetValue(2, 1, 56);
    }

    private Vector3 GetWorldPosition(int x, int y)
    {
        return cellSize * new Vector3(x, y);
    }

    private void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt( worldPosition.x / cellSize);
        y = Mathf.FloorToInt( worldPosition.y / cellSize);
    }

    private Vector3 GetLocalPosition(int x, int y)
    {
        return transform.InverseTransformPoint(new Vector3(x, y,0) * cellSize);
    }

    public void SetValue(int x, int y, int value)
    {
        if (x>=0 && y>=0 && x < width && y < height)
        {
            gridArray[x, y] = value;
            debugTextArray[x, y].text = gridArray[x, y].ToString();
        }
    }

    public void SetValue( Vector3 worldPosition, int value)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        SetValue(x, y, value);
    }
    
}
