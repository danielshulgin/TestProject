using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public PanAndZoom PanAndZoom;
    public GameObject gridGameObject;
    public GameObject groundGameObject;
    public float groundOffset = 3f;
    [SerializeField]
    private int xLength;
    [SerializeField]
    private int yLength;
    public float CellSideLength;
    public int XLength { get => xLength; private set => xLength = value; }
    public int YLength { get => yLength; private set => yLength = value; }

    [SerializeField]
    private Material gridMaterial;
    [SerializeField]
    private Material groundMaterial;

    public GridCoordinates center;
    public GridObject[,] gridObjects;

    void Start()
    {
        Create();
    }

    private void Create()
    {
        PanAndZoom.boundMaxX = XLength;
        PanAndZoom.boundMaxY = YLength;
        groundGameObject.transform.localScale = 
            new Vector3(0.1f + groundOffset / xLength * 0.2f, 1f, 0.1f + groundOffset / yLength * 0.2f);
        groundGameObject.transform.position = groundGameObject.transform.position
            - new Vector3(groundOffset * 0.01f, 0f, groundOffset * 0.01f);

        gridGameObject.transform.localScale = new Vector3(XLength, 1f, YLength);
        gridMaterial.mainTextureScale = new Vector2(XLength, YLength);
        groundMaterial.mainTextureScale = new Vector2(XLength + groundOffset * 2,
            YLength + groundOffset * 2);
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
        return gridPosition.x < XLength && gridPosition.y < YLength 
            && gridPosition.x >= 0 && gridPosition.y >= 0;
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
    /// <summary>
    /// Check grid object new coordinates updated with diff
    /// </summary>
    /// <param name="gridObject"></param>
    /// <param name="updaetdCoordinates">grid object offset</param>
    /// <returns></returns>
    public bool CanPutObject(GridObject gridObject, GridCoordinates updaetdCoordinates)
    {
        for (int x = updaetdCoordinates.x;
            x < gridObject.xLength + updaetdCoordinates.x; x++)
        {
            for (int y = updaetdCoordinates.y;
                y < gridObject.yLength + updaetdCoordinates.y; y++)
            {
                if (x >= xLength || y >= yLength ||
                    x < 0 || y < 0 || gridObjects[x, y] != null)
                {
                    return false;
                }
            }
        }
        return true;
    }
}
