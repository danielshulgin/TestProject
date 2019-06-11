using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    public BuildingController buildingController;

    public Grid grid;

    [Range(0f, 1f)]
    public float filledPart;

    public List<Zone> zones;

    public List<ItemForGeneration> itemsForGeneration;

    private GameObject[] itemsRandomized;

    void Start()
    {
        itemsRandomized = RandomizeArray(itemsForGeneration);
        Create();
    }

    public void Create()
    {
        foreach (var zone in zones)
        {
            for (int x = 0; x < zone.xLength; x++)
            {
                for (int y = 0; y < zone.yLength; y++)
                {
                    GridCoordinates position = new GridCoordinates(x, y) + zone.position;
                    if (Random.Range(0f, 1f) < filledPart && grid.GridCoordinatesExist(position))
                    {
                        GameObject itemPref = itemsRandomized[Random.Range(0, itemsRandomized.Length)];
                        GameObject item = 
                            buildingController.CreateBuilding(itemPref, position);
                        if(item != null)
                            grid.PutObjectOnGrid(item.GetComponent<GridObject>());
                    }
                }
            }
        }
    }

    private GameObject[] RandomizeArray(List<ItemForGeneration> startList)
    {
        int resultLength = 0;
        foreach (ItemForGeneration item in startList)
        {
            resultLength += item.frequencyFactor;
        }
        GameObject[] result = new GameObject[resultLength];
        int curIndex = 0;
        foreach (ItemForGeneration item in startList)
        {
            for (int i = 0; i < item.frequencyFactor; i++)
            {
                result[curIndex] = item.itemPref;
                curIndex++;
            }
        }
        return result;
    }
}

[System.Serializable]
public class Zone{
    public int xLength = 1;
    public int yLength = 1;
    public GridCoordinates position;
}

[System.Serializable]
public class ItemForGeneration
{
    public GameObject itemPref;
    [Range(1, 10)]
    public int frequencyFactor;
}
