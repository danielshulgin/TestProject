using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovementController : MonoBehaviour
{
    [SerializeField]
    private PanAndZoom panAndZoom;
    [SerializeField]
    private float speed = 1f;
    [SerializeField]
    private float interpolation = 0.5f;

    private Vector2 lastDiff;

    private void Start()
    {
        panAndZoom.onSwipe += MoveCamera;
        panAndZoom.onEndTouch += ResetLastDiff;
    }

    void Update()
    {
        
    }

    public void MoveCamera(Vector2 diff)
    {
        diff = Vector2.Lerp(lastDiff, diff, interpolation);
        transform.position = transform.position - new Vector3(diff.x * speed * Time.deltaTime, 0f, diff.y * speed * Time.deltaTime);
    }

    public void ResetLastDiff(Vector2 vector2)
    {
        lastDiff = Vector2.zero;
    }
}
