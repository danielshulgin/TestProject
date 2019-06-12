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

    public float interpolationMinimumSpeed = 0.3f;
    public float minSpeedToEnterInterpolation = 0.5f;
    public float postInterpolationSpeed = 0.05f;

    public bool blockMovement = false;

    private Vector2 lastDiff;

    private void Start()
    {
        panAndZoom.onSwipe += MoveCamera;
        //panAndZoom.onEndTouch += ResetLastDiff;
        panAndZoom.onEndTouch += PostMove;
    }

    public void MoveCamera(Vector2 diff)
    {
        if (!blockMovement)
        {
            diff = Vector2.Lerp(lastDiff, diff, interpolation);
            lastDiff = diff;
            transform.position = transform.position - new Vector3(diff.x * speed * Time.deltaTime, 0f, diff.y * speed * Time.deltaTime);
        }
    }

    public void PostMove(Vector2 vector2)
    {
        if (minSpeedToEnterInterpolation < lastDiff.magnitude)
        {
            StopCoroutine(PostSwapMove());
            StartCoroutine(PostSwapMove());
        }
    }

    IEnumerator PostSwapMove()
    {
        while(lastDiff.magnitude > interpolationMinimumSpeed && !blockMovement)
        {
            transform.position = transform.position - 
                new Vector3(lastDiff.x * Time.deltaTime, 0f, lastDiff.y * Time.deltaTime) * speed;
            lastDiff = Vector2.Lerp(lastDiff, Vector2.zero, postInterpolationSpeed);
            yield return null;
        }
        lastDiff = new Vector2();
    }
}
