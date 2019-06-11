using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct GridCoordinates
{
    public int x;
    public int y;

    public GridCoordinates(int x = 0, int y = 0)
    {
        this.x = x;
        this.y = y;
    }

    public static GridCoordinates WorldToGridCoordinates(Vector3 worldPosition)
    {
        return new GridCoordinates((int)worldPosition.x, (int)worldPosition.z);
    }

    public override string ToString()
    {
        return "( " + x + ", " + y + " )";
    }

    public static GridCoordinates operator +(GridCoordinates first, GridCoordinates second)
    {
        return new GridCoordinates(first.x + second.x, first.y + second.y);
    }

    public static GridCoordinates operator -(GridCoordinates first, GridCoordinates second)
    {
        return new GridCoordinates(first.x - second.x, first.y - second.y);
    }
}
