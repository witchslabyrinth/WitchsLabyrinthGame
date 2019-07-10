using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public GameObject ghostCamera;
    public GameObject player;
    protected Camera myCam;

    protected Vector3 rightRotation = new Vector3 (0f, -90f, 0f);
    protected Vector3 topRotation = new Vector3 (90f, 0f, 0f);
    protected Vector3 backRotation = new Vector3 (0f, 0f, 0f);

    public float orthoOffset = 2.0f;
    public float headway = 2.0f;

    protected enum CameraViews {
        RIGHT,
        TOP,
        BACK,
        PERSPECTIVE,
    }
    protected CameraViews currentView;

    void Start () {
        this.transform.SetParent (ghostCamera.transform);
        myCam = this.GetComponent<Camera> ();
        currentView = CameraViews.PERSPECTIVE;
    }

    // Update is called once per frame
    void Update () {
        switch (currentView) {
        case CameraViews.PERSPECTIVE:
            PerspectiveUpdate ();
            break;
        case CameraViews.RIGHT:
            RightOrthoUpdate ();
            break;
        case CameraViews.TOP:
            TopOrthoUpdate ();
            break;
        }
    }

    /******  SET VARIOUS CAMERA VIEWS ******/
    public void SetToPerspective () {
        currentView = CameraViews.PERSPECTIVE;
        myCam.orthographic = false;
        this.transform.SetParent (ghostCamera.transform);
        PerspectiveUpdate ();
    }

    public void SetToRightOrtho () {
        currentView = CameraViews.RIGHT;
        myCam.orthographic = true;
        this.transform.eulerAngles = rightRotation;
        RightOrthoUpdate ();
    }

    public void SetToTopOrtho () {
        currentView = CameraViews.TOP;
        myCam.orthographic = true;
        this.transform.eulerAngles = topRotation;
        TopOrthoUpdate ();
    }

    public void SetToBackOrtho () {
        currentView = CameraViews.BACK;
        myCam.orthographic = true;
        this.transform.eulerAngles = backRotation;
        BackOrthoUpdate ();
    }

    /******  UPDATE VARIOUS CAMERA VIEWS ******/
    private void PerspectiveUpdate () {
        this.transform.position = ghostCamera.transform.position;
        this.transform.rotation = ghostCamera.transform.rotation;
    }

    private void RightOrthoUpdate () {
        this.transform.position = new Vector3 (player.transform.position.x + headway, player.transform.position.y + 4f, player.transform.position.z + headway);
    }

    private void TopOrthoUpdate () {
        this.transform.position = new Vector3 (player.transform.position.x, player.transform.position.y + 20f, player.transform.position.z + 2f);
    }

    private void BackOrthoUpdate () {
        this.transform.position = new Vector3 (player.transform.position.x + headway, player.transform.position.y + 4f, player.transform.position.z - orthoOffset);

    }
}