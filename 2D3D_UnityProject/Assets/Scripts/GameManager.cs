using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    // TODO: reference these as Cameras or CameraControllers rather than just GameObjects
    public GameObject mainCamera;
    public GameObject ghostCamera;


    // TODO: change this from a GameObject to a more specific type (like PlayerController)
    public GameObject player;

    protected CameraController camControl;
    protected PlayerController playerControl;
    protected PerspectiveCameraControl perspControl;

    public static GameManager instance = null;
    void Awake () {
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy (gameObject);
        }
        DontDestroyOnLoad (gameObject);
    }

    void Start () {
        camControl = mainCamera.GetComponent<CameraController> ();
        playerControl = player.GetComponent<PlayerController> ();
        perspControl = ghostCamera.GetComponent<PerspectiveCameraControl> ();
    }

    void Update () {
        if (Input.GetKeyDown (KeyCode.UpArrow)) {
            // Update movement type
            playerControl.SetMovementType(new TopDownMovement());
            
            // Update camera
            camControl.SetToTopOrtho ();
            perspControl.enabled = false;
        } 
        else if (Input.GetKeyDown (KeyCode.RightArrow)) {
            // Update movement type
            playerControl.SetMovementType(new SideViewMovement());
            
            // Update camera
            camControl.SetToRightOrtho ();
            perspControl.enabled = false;
        } 
        else if (Input.GetKeyDown (KeyCode.LeftArrow)) {
            // TODO: get camera component earlier/somewhere else
            // Update movement type
            playerControl.SetMovementType(new PerspectiveMovement(ghostCamera.GetComponent<Camera>()));

            // Update camera
            camControl.SetToPerspective ();
            perspControl.enabled = true;
        } 
        else if (Input.GetKeyDown (KeyCode.DownArrow)) {
            // Update movement type
            playerControl.SetMovementType(new BackViewMovement());
            
            // Update camera
            camControl.SetToBackOrtho ();
            perspControl.enabled = false;
        }
    }
}