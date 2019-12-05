using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewObjectFunctionality : MonoBehaviour
{

    Vector3 mPrevPos = Vector3.zero;
    Vector3 mPosDelta = Vector3.zero;


    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 10000.0f))
            {
                if (hit.collider.gameObject == gameObject)
                {

                    mPosDelta = Input.mousePosition - mPrevPos;

                    transform.Rotate(Vector3.up, -Vector3.Dot(mPosDelta, Camera.main.transform.right), Space.World); //project left and right mouse movement onto object

                    transform.Rotate(Camera.main.transform.right, Vector3.Dot(mPosDelta, Camera.main.transform.up), Space.World); //project other mouse movement onto object

                }
            }

        }

        mPrevPos = Input.mousePosition;
    }
}
