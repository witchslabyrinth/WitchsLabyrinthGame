using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//allows an interactable object to be rotated in 3 dimensions
public class PreviewObjectFunctionality : MonoBehaviour
{
    private Vector3 mPrevPos;
    private Vector3 mPosDelta;

    [SerializeField]
    private Camera linkedInspectCam;

    [SerializeField]
    private int index;

    [SerializeField]
    private float rotationSpeed = 0.25f;

    private void Start()
    {
        mPrevPos = Vector3.zero;
        mPosDelta = Vector3.zero;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && linkedInspectCam.GetComponent<CameraFollow>().GetObjectIndex() == index)
        {

            mPosDelta = Input.mousePosition - mPrevPos;
            Vector3 angle = mPosDelta * rotationSpeed;

            transform.Rotate(Vector3.up, -Vector3.Dot(angle, linkedInspectCam.transform.right), Space.World); //project left and right mouse movement onto object

            transform.Rotate(linkedInspectCam.transform.right, Vector3.Dot(angle, linkedInspectCam.transform.up), Space.World); //project other mouse movement onto object

        }
        mPrevPos = Input.mousePosition;
    }

    public int GetIndex()
    {
        return index;
    }
}
