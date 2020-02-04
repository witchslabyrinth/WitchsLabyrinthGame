using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoomController : MonoBehaviour
{

    private Camera cam;
    private float targetZoom;
    //private float zoomFactor = 3f;
    //float zoomLerpSpeed = 10;

    // Start is called before the first frame update
    void Start()
    {
        cam = this.GetComponent<Camera>(); //using main camera
    }

    // Update is called once per frame
    void Update()
    {
        float ScrollWheelChange = Input.GetAxis("Mouse ScrollWheel");
        if (GetComponent<CameraFollow>().GetInspectMode() == 1 && ScrollWheelChange != 0)
        {                                                                       //If the scrollwheel has changed
            float R = ScrollWheelChange * 5;                                    //The radius from current camera
            float PosX = cam.transform.eulerAngles.x + 90;              //Get up and down
            float PosY = -1 * (cam.transform.eulerAngles.y - 90);       //Get left to right
            PosX = PosX / 180 * Mathf.PI;                                       //Convert from degrees to radians
            PosY = PosY / 180 * Mathf.PI;                                       //^
            float X = R * Mathf.Sin(PosX) * Mathf.Cos(PosY);                    //Calculate new coords
            float Z = R * Mathf.Sin(PosX) * Mathf.Sin(PosY);                    //^
            float Y = R * Mathf.Cos(PosX);                                      //^
            float CamX = cam.transform.position.x;                      //Get current camera postition for the offset
            float CamY = cam.transform.position.y;                      //^
            float CamZ = cam.transform.position.z;                      //^
            cam.transform.position = new Vector3(CamX + X, CamY + Y, CamZ + Z);//Move the main camera
        }
    }
}