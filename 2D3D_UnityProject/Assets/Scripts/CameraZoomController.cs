using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoomController : MonoBehaviour
{

    private Camera cam;
    private float targetZoom;
    private float zoomFactor = 3f;
    float zoomLerpSpeed = 10;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main; //using main camera
        targetZoom = cam.orthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        float scrollData;
        scrollData = Input.GetAxis("Mouse ScrollWheel");
        //Debug.Log(scrollData);
        targetZoom = targetZoom - scrollData * zoomFactor; //uses zoomFactor and scrollData to modify the speed of zoom
        targetZoom = Mathf.Clamp(targetZoom, .5f, 2f); //adds a restriction to the amount that can be zoomed in
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetZoom, Time.deltaTime * zoomLerpSpeed);  //creates the smooth movement each frame
    }
}
