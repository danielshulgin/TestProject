using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField]
    private int xLength;
    [SerializeField]
    private int yLength;
    public float CellSideLength;
    public int XLength { get => xLength; private set => xLength = value; }
    public int YLength { get => yLength; private set => yLength = value; }

    [SerializeField]
    private Material gridMaterial;

    public GridCoordinates center;

    public GridObject[,] gridObjects;

    public GameObject gridGameObject;

    void Start()
    {
        Create();
    }

    private void Create()
    {
        gridGameObject.transform.localScale = new Vector3(XLength, 1f, YLength);
        gridMaterial.mainTextureScale = new Vector2(XLength, YLength);
        gridObjects = new GridObject[XLength, YLength];
        center = new GridCoordinates(XLength / 2, YLength / 2);
    }

    public void GridTouch(Vector3 position)
    {
        Debug.Log(GridCoordinates.WorldToGridCoordinates(position));
    }

    public bool PutObjectOnGrid(GridObject gridObject)
    {
        if (CanPutObject(gridObject))
        {
            for (int x = gridObject.gridPosition.x; 
                x < gridObject.gridPosition.x + gridObject.xLength; x++)
            {
                for (int y = gridObject.gridPosition.y; 
                    y < gridObject.gridPosition.y + gridObject.yLength; y++)
                {
                    gridObjects[x, y] = gridObject;
                }
            }
            return true;
        }
        return false;
    }

    public void DeleteObjectOnGrid(GridObject gridObject)
    {
        for (int x = gridObject.gridPosition.x;
            x < gridObject.gridPosition.x + gridObject.xLength; x++)
        {
            for (int y = gridObject.gridPosition.y;
                y < gridObject.gridPosition.y + gridObject.yLength; y++)
            {
                gridObjects[x, y] = null;
            }
        }
    }

    public bool GridCoordinatesExist(GridObject gridObject)
    {
        return gridObject.gridPosition.x < XLength && gridObject.gridPosition.y < YLength;
    }

    public bool GridCoordinatesExist(GridCoordinates gridPosition)
    {
        return gridPosition.x < XLength && gridPosition.y < YLength;
    }

    public Vector3 FromGridToWorldCoordinates(GridCoordinates gridCoordinates)
    {
        return new Vector3(gridCoordinates.x * CellSideLength, 0f, gridCoordinates.y * CellSideLength);
    }

    public bool CanPutObject(GridObject gridObject)
    {
        if (GridCoordinatesExist(gridObject))
        {
            for (int x = gridObject.gridPosition.x; 
                x < gridObject.gridPosition.x + gridObject.xLength; x++)
            {
                for (int y = gridObject.gridPosition.y; 
                    y < gridObject.gridPosition.y + gridObject.yLength; y++)
                {
                    if (x >= xLength || y >= yLength || gridObjects[x, y] != null)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        return false;
    }
}
