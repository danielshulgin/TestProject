using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum BuildingControllerState
{
    InPrototype, Disabled
}

public class BuildingController : MonoBehaviour
{
    public Grid grid;

    public GameObject buildingParent;

    public BuildingControllerState State { get; private set; }

    public List<GameObject> buildingPrefabs = new List<GameObject>();

    public GameObject inPrototype;

    private int currentId;

    private void Start()
    {
        State = BuildingControllerState.Disabled;
    }

    public void StartBuilding(int id)
    {
        GameObject selctedBuildingPref = buildingPrefabs.Find(
        prefab => prefab.GetComponent<GridObject>().id == id);
        if (selctedBuildingPref != null)
        {
            currentId = id;
            State = BuildingControllerState.InPrototype;

            CreatePrototype(selctedBuildingPref, grid.center);
        }
        else
        {
            Debug.Log("Wrong id!!!");
        }
    }

    public void ApplyBuilding()
    {
        if (grid.PutObjectOnGrid(inPrototype.GetComponent<GridObject>()))
        {
            inPrototype = null;
            StartBuilding(currentId);
        }
    }

    public void EndBuilding()
    {
        State = BuildingControllerState.Disabled;
        if (inPrototype != null)
        {
            Destroy(inPrototype);
        }
    }

    private void CreatePrototype(GameObject selctedBuildingPref, GridCoordinates position)
    {
        inPrototype = CreateBuilding(selctedBuildingPref, position);
    }

    public GameObject CreateBuilding(GameObject prefab, GridCoordinates position)
    {
        GridObject gridObject = prefab.GetComponent<GridObject>();
        gridObject.gridPosition = position; 
        if (grid.CanPutObject(gridObject)) {
            GameObject building = Instantiate(prefab);
            building.transform.position = grid.FromGridToWorldCoordinates(position);
            building.transform.SetParent(buildingParent.transform);
            return building;
        }
        Debug.Log("wrong coordinates or grid object size");
        return null;
    }
}
