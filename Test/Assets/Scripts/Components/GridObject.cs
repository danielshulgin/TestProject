using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject : MonoBehaviour
{
    public GridCoordinates gridPosition;

    public int xLength;

    public int yLength;

    public int id;

    public void Initialize(GridCoordinates gridPosition, int xLength, int yLength, int id)
    {
        this.gridPosition = gridPosition;
        this.xLength = xLength;
        this.yLength = yLength;
        this.id = id;
    }
}
