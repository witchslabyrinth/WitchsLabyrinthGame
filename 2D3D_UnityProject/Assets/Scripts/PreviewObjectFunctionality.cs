using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewObjectFunctionality : MonoBehaviour
{

    Vector3 mPrevPos = Vector3.zero;
    Vector3 mPosDelta = Vector3.zero;
    CheckIfInRange check; //constructor for accessing CheckIfInRange pub variables

    public void Start()
    {
        check = Camera.main.GetComponent<CheckIfInRange>(); //add CheckIfInRange variables to check variable
    }
    


    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            float view = Camera.main.fieldOfView;
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 10000.0f))
            {
                if ((hit.collider.gameObject == gameObject) & (check.zoomFlag == 1)) //if the mouse is over an object AND the flag is set to 1 (meaning zoomed in)
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
