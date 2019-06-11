using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public Grid grid;

    public BuildingController buildingController;

    public int id;

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
        if (Physics.Raycast(ray, out hit)) ;
            //buildingController.StartBuilding(id);
    }
}
