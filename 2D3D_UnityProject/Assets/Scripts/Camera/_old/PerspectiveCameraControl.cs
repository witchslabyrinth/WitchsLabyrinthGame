using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerspectiveCameraControl : MonoBehaviour
{
    public Camera Camera { get; private set; }

    private Vector2 _mouseAbsolute;
    private Vector2 _smoothMouse;

    [SerializeField]
    private Vector2 clampInDegrees = new Vector2(360, 180);
    [SerializeField]
    private Vector2 sensitivity = new Vector2(2, 2);
    [SerializeField]
    private Vector2 smoothing = new Vector2(3, 3);
    [SerializeField]
    private Vector2 targetDirection;
    public Vector2 targetCharacterDirection;

    [SerializeField]
    private float verticalClampMax;
    [SerializeField]
    private float verticalClampMin;
    [SerializeField]
    private float distanceFromPivot;
    [SerializeField]
    private float smoothingFactor;
    private float lastY;
    // Set lastY to this value on start as this is the value given to it on the first frame
    private const float INITIAL_Y = 0.5f;

    private int CAMERA_MASK;

    [SerializeField]
    private Transform cameraPivot;

    public GameObject characterBody;

    void Start()
    {
        CAMERA_MASK = LayerMask.GetMask("CameraCollision");
        // Set target direction for the character body to its inital state.
        if (characterBody)
            targetCharacterDirection = characterBody.transform.localRotation.eulerAngles;

        lastY = INITIAL_Y;
    }

    public void CameraUpdate()
    {
        // Allow the script to clamp based on a desired target value.
        var targetCharacterOrientation = Quaternion.Euler(targetCharacterDirection);

        // Get raw mouse input for a cleaner reading on more sensitive mice.
        var mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

        // Scale input against the sensitivity setting and multiply that against the smoothing value.
        mouseDelta = Vector2.Scale(mouseDelta, new Vector2(sensitivity.x * smoothing.x, sensitivity.y * smoothing.y));

        // Interpolate mouse movement over time to apply smoothing delta.
        _smoothMouse.x = Mathf.Lerp(_smoothMouse.x, mouseDelta.x, 1f / smoothing.x);
        _smoothMouse.y = Mathf.Lerp(_smoothMouse.y, mouseDelta.y, 1f / smoothing.y);

        // Find the absolute mouse movement value from point zero.
        _mouseAbsolute += _smoothMouse;

        // Clamp and apply the local x value first, so as not to be affected by world transforms.
        if (clampInDegrees.x < 360)
            _mouseAbsolute.x = Mathf.Clamp(_mouseAbsolute.x, -clampInDegrees.x * 0.5f, clampInDegrees.x * 0.5f);

        // Clamp y value
        _mouseAbsolute.y = Mathf.Clamp(_mouseAbsolute.y, verticalClampMin, verticalClampMax);

        // Get amount mouse has moved since last frame
        float yChange =  _mouseAbsolute.y - lastY;
        // Get pitch rotation based on yChange and apply it to vector pointing from the pivot to the camera
        Quaternion rot = Quaternion.AngleAxis(-yChange, characterBody.transform.right);
        Vector3 pivotToCamera = rot * (transform.position - cameraPivot.position);
        pivotToCamera = pivotToCamera.normalized;

        // New position is pivotToCamera vector multiplied by distanceFromPivot
        Vector3 newPosition = cameraPivot.position + (pivotToCamera * distanceFromPivot);

        // Check if there is an object in between camera and pivot
        RaycastHit hit;
        if (Physics.Raycast(cameraPivot.position, pivotToCamera, out hit, distanceFromPivot, CAMERA_MASK))
        {
            // If the raycast hit something, put the camera at the point of collision
            transform.position = Vector3.Lerp(transform.position, hit.point, smoothingFactor);
            //transform.position = hit.point;
        }
        else
        {
            // If the raycast didn't hit something then put it in normal spot
            transform.position = Vector3.Lerp(transform.position, newPosition, smoothingFactor);
            //transform.position = newPosition;
        }

        // Point camera at pivot
        transform.LookAt(cameraPivot);

        // If there's a character body that acts as a parent to the camera
        if (characterBody)
        {
            var yRotation = Quaternion.AngleAxis(_mouseAbsolute.x, Vector3.up);
            characterBody.transform.localRotation = yRotation * targetCharacterOrientation;
        }
        else
        {
            var yRotation = Quaternion.AngleAxis(_mouseAbsolute.x, transform.InverseTransformDirection(Vector3.up));
            transform.localRotation *= yRotation;
        }

        lastY = _mouseAbsolute.y;
    }
}