using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InputContollerState
{
    Building, Default
}

public class InputController : MonoBehaviour
{
    public PanAndZoom panAndZoom;

    public Grid grid;

    public BuildingController buildingController;

    public InputContollerState state = InputContollerState.Default;

    private void Start()
    {
        panAndZoom.onStartTouch += CreateCheckRay;
    }

    void Update()
    {
        if (Input.touches.Length == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            CreateCheckRay(Input.GetTouch(0).position);
        }
        if (Input.GetButtonDown("Fire1"))
        {
            CreateCheckRay(Input.mousePosition);
        }
    }

    public void CreateCheckRay(Vector2 screenPosition)
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        if (Physics.Raycast(ray, out hit))
        {
            GridObject gridObject = hit.transform.gameObject.GetComponent<GridObject>();
            if (gridObject != null)
            {
                Debug.Log(gridObject);
            }
        }
    }
}
