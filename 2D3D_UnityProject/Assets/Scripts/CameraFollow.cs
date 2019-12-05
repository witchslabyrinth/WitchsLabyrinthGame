using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    //public float Speed = 200f;
    Quaternion rotTarget;
    int cameraSpeed = 30; //use this to control camera move speed!


    void Update()
    {
        if (Input.GetMouseButton(0)) //check for left click
        {
            RaycastHit hit;

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 10000.0f)) //check if left mouse clicked on an object
            {
                var FollowPos = hit.transform; //what to rotate to if raycast hit an object

                rotTarget = Quaternion.LookRotation(FollowPos.position - Camera.main.transform.position); //set up the smooth interval that Quaternion.RotateTowards uses later as an endpoint 

            }
        }
        Camera.main.transform.rotation = Quaternion.RotateTowards(Camera.main.transform.rotation, rotTarget, Time.deltaTime * cameraSpeed); //point to rotate to at each update/frame

    }
}
