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

    public PanAndZoom panAndZoom;

    public GameObject buildingParent;

    public CameraMovementController cameraMovementController;

    public float buildingMoveSpeed = 1f;
    public Vector3 buildingPosition = Vector3.zero;

    public BuildingControllerState State { get; private set; }

    public List<GameObject> buildingPrefabs = new List<GameObject>();

    public GameObject inPrototype;

    private bool touchOnPrototype = false;

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
            buildingPosition = grid.FromGridToWorldCoordinates((grid.center));
            panAndZoom.onSwipe += MoveBuilding;
            panAndZoom.onStartTouch += StartMoveBuilding;
            panAndZoom.onEndTouch += EndMoveBuilding;
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
        panAndZoom.onSwipe -= MoveBuilding;
        panAndZoom.onStartTouch -= StartMoveBuilding;
        panAndZoom.onEndTouch -= EndMoveBuilding;
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
        //TODO
        //Debug.Log("wrong coordinates or grid object size");
        return null;
    }

    public void MoveBuilding(Vector2 diff)
    {
        if (touchOnPrototype)
        {
            Vector3 updatedBuildingPosition = buildingPosition 
                + new Vector3(diff.x, 0f, diff.y) * buildingMoveSpeed * Time.deltaTime;
            GridCoordinates updatedCoordinates = GridCoordinates.WorldToGridCoordinates(updatedBuildingPosition);
            GridObject prototypeGridObject = inPrototype.GetComponent<GridObject>();
            if (grid.CanPutObject(prototypeGridObject, updatedCoordinates))
            {
                prototypeGridObject.gridPosition = updatedCoordinates;
                inPrototype.transform.position = grid.FromGridToWorldCoordinates(updatedCoordinates);
                buildingPosition = updatedBuildingPosition;
            }
        }
    }

    public void StartMoveBuilding(Vector2 screenPosition)
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        if (Physics.Raycast(ray, out hit))
        {
            if(hit.transform.gameObject == inPrototype)
            {
                cameraMovementController.blockMovement = true;
                touchOnPrototype = true;
            }
        }
    }

    public void EndMoveBuilding(Vector2 screenPosition)
    {
        cameraMovementController.blockMovement = false;
        touchOnPrototype = false;
    }
}
