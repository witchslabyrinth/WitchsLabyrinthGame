using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//allows an interactable object to be rotated in 3 dimensions
public class PreviewObjectFunctionality : MonoBehaviour
{
    Vector3 mPrevPos = Vector3.zero;
    Vector3 mPosDelta = Vector3.zero;

    [SerializeField]
    private Camera linkedInspectCam;

    public int index;

    void Update()
    {
        if (Input.GetMouseButton(0) && linkedInspectCam.GetComponent<CameraFollow>().objectIndex == index) 
        {
            RaycastHit hit;

            Ray ray = linkedInspectCam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 10000.0f))
            {
                if (hit.collider.gameObject == gameObject) 
                {
                    if (hit.collider.gameObject.GetComponent<PreviewObjectFunctionality>().enabled == true)//check that it's an interactable object
                    {
                        mPosDelta = Input.mousePosition - mPrevPos;

                        transform.Rotate(Vector3.up, -Vector3.Dot(mPosDelta, linkedInspectCam.transform.right), Space.World); //project left and right mouse movement onto object

                        transform.Rotate(linkedInspectCam.transform.right, Vector3.Dot(mPosDelta, linkedInspectCam.transform.up), Space.World); //project other mouse movement onto object
                    }
                }
            }
        }
        mPrevPos = Input.mousePosition;
    }
}
