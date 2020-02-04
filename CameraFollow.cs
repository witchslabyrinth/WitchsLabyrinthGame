using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//allows the camera to turn towards an interactable object that was clicked and set a 'zoomed in' flag


public class CameraFollow : MonoBehaviour
{ 
    public int inspectMode = 0; // 0 is not in inspect mode, 1 is in inspect mode

    /*
    public float startCamPosX = Camera.main.transform.position.x;
    public float startCamPosY = Camera.main.transform.position.z;
    public float startCamPosZ = Camera.main.transform.position.z;
    public float startCamRotX = Camera.main.transform.rotation.x;
    public float startCamRotY = Camera.main.transform.rotation.y;
    public float startCamRotZ = Camera.main.transform.rotation.z;

    public float startObjPosX;
    public float startObjPosY;
    public float startObjPosZ;
    public float startObjRotX;
    public float startObjRotY;
    public float startObjRotZ;
    
    */


    Quaternion rotTarget;
    public RaycastHit hit;

    int cameraSpeed = 30; //use this to control camera move speed
    void Update()
    {
        if (Input.GetMouseButton(0) & inspectMode == 0) //check for left click
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 10000.0f)) //check if left mouse clicked on an object
            {
                if (hit.collider.gameObject.GetComponent<PreviewObjectFunctionality>().enabled == true)//check if object is interactable
                {
                    inspectMode = 1; //change to 1 (enter inspect mode)

                    

                    /*
                    startObjPosX = hit.collider.gameObject.transform.position.x;
                    startObjPosY = hit.collider.gameObject.transform.position.y;
                    startObjPosZ = hit.collider.gameObject.transform.position.z;
                    startObjRotX = hit.collider.gameObject.transform.rotation.x;
                    startObjRotY = hit.collider.gameObject.transform.rotation.y;
                    startObjRotZ = hit.collider.gameObject.transform.rotation.z;
                    
                    */

                    var FollowPos = hit.transform; //what to rotate to if raycast hit an object

                    rotTarget = Quaternion.LookRotation(FollowPos.position - Camera.main.transform.position); //set up the smooth interval that Quaternion.RotateTowards uses later as an endpoint 
                }
                   

            }
        }

        Camera.main.transform.rotation = Quaternion.RotateTowards(Camera.main.transform.rotation, rotTarget, Time.deltaTime * cameraSpeed); //point to rotate to at each update/frame

        /*
        if (Input.GetMouseButton(0) & inspectMode == 1)
        {
            inspectMode = 0;
            //Camera.main.transform.position = new Vector3(startCamPosX, startCamPosY, startCamPosZ);
            //Camera.main.transform.rotation = Quaternion.Euler(startCamRotX, startCamRotY, startCamRotZ);

            hit.collider.gameObject.transform.position = new Vector3(startObjPosX, startObjPosY, startObjPosZ);
            hit.collider.gameObject.transform.rotation = Quaternion.Euler(startObjRotX, startObjRotY, startObjRotZ);
  
        }
        */

    }
}
