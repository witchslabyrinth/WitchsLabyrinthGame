using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckIfInRange : MonoBehaviour
{
    float startFov;
 

    public void Start()
    {
    startFov = Camera.main.fieldOfView;
    }

    

    public float distance; //max detection distance for interaction

    public int zoomFlag = 0; //0 = zoomed out, 1 = zoomed in
    int zoomInMax = 30; //the maximum zoomed in value for the field of view value
    int zoomOutMax = 60; //the maximum zoomed out value for the field of view value
    float lerpSpeed = 1f; //(.5 - 1] increase speed, [0 - .5) decrease speed

    Vector3 mPrevPos = Vector3.zero;
    Vector3 mPosDelta = Vector3.zero;

    //public float startFov = Camera.main.fieldOfView;

    void Update()
    {
        RaycastHit hit;
        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, distance))//if camera is viewing object in range 
        {
            //here is where I would put the code to change the color of the object to show that it's interactable
            if (Input.GetKeyDown(KeyCode.E) & (zoomFlag == 0))
            { //if the inspect key is pressed and you're zoomed out
                while (startFov > zoomInMax)
                {
                    startFov -= 1;
                }

                zoomFlag = 1;
                //Debug.Log("Inspecting now"); 
                //switch to 'inspect view' here
                //Camera.main.fieldOfView = 40; //zoom in
            }
            
            else if (Input.GetKeyDown(KeyCode.E) & (zoomFlag == 1))
            { //if the inspect key is pressed and you're zoomed in
                while (startFov < zoomOutMax)
                {
                    startFov += 1;
                }

                zoomFlag = 0;
            }              

        }

        Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, startFov, lerpSpeed * Time.deltaTime);

    }
}