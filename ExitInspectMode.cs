using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitInspectMode : MonoBehaviour
{

    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            if (Camera.main.GetComponent<CameraFollow>().inspectMode == 1) //is camera in inspect mode?
            {
                //check if raycast matches name var stored in cameraFollow script
                // Debug.Log("Exiting Inspect Mode");
                //Camera.main.GetComponent<CameraFollow>().inspectMode = 0;
                //Camera.main.transform.position = new Vector3(Camera.main.GetComponent<CameraFollow>().startCamPosX, Camera.main.GetComponent<CameraFollow>().startCamPosY, Camera.main.GetComponent<CameraFollow>().startCamPosZ);
                //Camera.main.GetComponent<CameraFollow>().hit.collider.transform.position = new Vector3(Camera.main.GetComponent<CameraFollow>().startObjPosX, Camera.main.GetComponent<CameraFollow>().startObjPosY, Camera.main.GetComponent<CameraFollow>().startObjPosZ);

            }



        }
    }
}
