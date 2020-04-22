﻿using System.Collections;
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
    private float lastY;

    [SerializeField]
    private Transform cameraPivot;

    public GameObject characterBody;

    void Start()
    {
        // Set target direction for the character body to its inital state.
        if (characterBody)
            targetCharacterDirection = characterBody.transform.localRotation.eulerAngles;

        lastY = 0.5f;
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


        // Transform y position of camera based on y input
        _mouseAbsolute.y = Mathf.Clamp(_mouseAbsolute.y, verticalClampMin, verticalClampMax);





        //float yChange = verticalClampMax - _mouseAbsolute.y + verticalClampMin;
        float yChange =  _mouseAbsolute.y - lastY;
        Debug.Log(yChange);
        Quaternion rot = Quaternion.AngleAxis(-yChange, characterBody.transform.right);
        Vector3 projected = rot * (transform.position - cameraPivot.position);
        projected = projected.normalized;
        Vector3 newPosition = cameraPivot.position + (projected * distanceFromPivot);
        Debug.DrawRay(cameraPivot.position, projected);


        RaycastHit hit;
        if (Physics.Raycast(cameraPivot.position, projected, out hit, distanceFromPivot))
        {
            // If the raycast hit something, put the camera at the point of collision
            transform.position = hit.point;
        }
        else
        {
            // If the raycast didn't hit something then put it back in normal spot
            transform.position = newPosition;
        }




        //Vector3 newPosition = new Vector3(transform.localPosition.x, verticalClampMax - _mouseAbsolute.y + verticalClampMin, transform.localPosition.z);
        //transform.localPosition = newPosition;




        //// Raycast from pivot to camera to see if something is in the way
        //RaycastHit hit;
        //if (Physics.Raycast(cameraPivot.position, transform.position - cameraPivot.position, out hit, distanceFromPivot))
        //{
        //    // If the raycast hit something, put the camera at the point of collision
        //    transform.position = hit.point;
        //}
        //else
        //{
        //    // If the raycast didn't hit something then put it back in normal spot
        //    Vector3 pointToCamera = (transform.position - cameraPivot.position).normalized;
        //    pointToCamera *= distanceFromPivot;
        //    transform.position = cameraPivot.position + pointToCamera;
        //}

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