using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//allows the camera to turn towards an interactable object that was clicked and set a 'zoomed in' flag


public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private int inspectMode = 0; // 0 is not in inspect mode, 1 is in inspect mode

    /// <summary>
    /// ID of object currently being inspected
    /// -1 when no object selected
    /// </summary>
    private int objectIndex = -1;

    // /// <summary>
    // /// Store the position of the camera before inspecting
    // /// </summary>
    // private Vector3 startCamPos;

    // /// <summary>
    // /// Store the rotation of the camera before inspecting
    // /// </summary>
    // private Quaternion startCamRot;

    /// <summary>
    /// store the positino of the inspectable object before inspecting
    /// </summary>
    private Vector3 startObjPos;

    /// <summary>
    /// stores the rotaion of the inspectable object before inspecting
    /// </summary>
    private Quaternion startObjRot;


    Quaternion rotTarget;
    public RaycastHit hit;

    private GameObject inspectObj;

    [SerializeField]
    private GameObject inspectLoc;

    [SerializeField]
    private float moveTime;

    int cameraSpeed = 30; //use this to control camera move speed

    void Update()
    {
        //Cursor.lockState = CursorLockMode.None;
        GameManager.SetCursorActive(true);
        if (Input.GetMouseButton(0) & inspectMode == 0) //check for left click
        {
            if (Physics.Raycast(GetComponent<Camera>().ScreenPointToRay(Input.mousePosition), out hit, 10000.0f)) //check if left mouse clicked on an object
            {
                inspectObj = hit.collider.gameObject;

                if (inspectObj.GetComponent<PreviewObjectFunctionality>() != null)
                {
                    inspectMode = 1; //change to 1 (enter inspect mode)
                    objectIndex = inspectObj.GetComponent<PreviewObjectFunctionality>().GetIndex();

                    startObjPos = inspectObj.transform.position;
                    startObjRot = inspectObj.transform.rotation;

                    // startCamPos = transform.position;
                    // startCamRot = transform.rotation;

                    var FollowPos = hit.transform; //what to rotate to if raycast hit an object

                    // rotTarget = Quaternion.LookRotation(FollowPos.position - transform.position); //set up the smooth interval that Quaternion.RotateTowards uses later as an endpoint 
                    inspectObj.GetComponent<PreviewObjectFunctionality>().MoveToCamera(inspectLoc.transform.position, moveTime);
                }
            }
        }

        // if (inspectMode == 1)
        // {
        //     transform.rotation = Quaternion.RotateTowards(transform.rotation, rotTarget, Time.deltaTime * cameraSpeed); //point to rotate to at each update/frame
        // }

        if (Input.GetKeyDown(KeyCode.Space) & inspectMode == 1)
        {
            ResetCamAndObj();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Actor player = PlayerController.Instance.GetPlayer();
            player.Enable();

            if(inspectMode == 1)
            {
                ResetCamAndObj();
            }

            gameObject.SetActive(false);
        }
    }

    private void ResetCamAndObj()
    {
        inspectMode = 0;
        objectIndex = -1;
        // transform.position = startCamPos;
        // transform.rotation = startCamRot;
        // inspectObj.transform.position = startObjPos;
        // inspectObj.transform.rotation = startObjRot;

        inspectObj.GetComponent<PreviewObjectFunctionality>().ResetObject(moveTime);
    }

    public int GetInspectMode()
    {
        return inspectMode;
    }

    public int GetObjectIndex()
    {
        return objectIndex;
    }
}
