using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public GameObject ghostCamera;
    public GameObject player;
    protected Camera myCam;

    protected Vector3 rightRotation = new Vector3(0f, -90f, 0f);
    protected Vector3 topRotation = new Vector3(90f, 0f, 0f);
    protected Vector3 backRotation = new Vector3(0f, 0f, 0f);

    public float headway = 5.0f;
    public float orthoOffset = 5.0f;

    protected enum CameraViews {
        RIGHT,
        TOP,
        BACK,
        PERSPECTIVE,
    }
    protected CameraViews currentView;

    void Start()
    {
        this.transform.SetParent(ghostCamera.transform);
        myCam = this.GetComponent<Camera>();
        currentView = CameraViews.PERSPECTIVE;
    }

    // Update is called once per frame
    void Update()
    {
        switch(currentView) {
            case CameraViews.PERSPECTIVE:
                PerspectiveUpdate();
                break;
            case CameraViews.RIGHT:
                RightOrthoUpdate();
                break;
            case CameraViews.TOP:
                TopOrthoUpdate();
                break;
        }
    }

    /******  SET VARIOUS CAMERA VIEWS ******/
    void SetToPerspective() 
    {
        currentView = CameraViews.PERSPECTIVE;
        myCam.orthographic = false;
        this.transform.SetParent(ghostCamera.transform);
        PerspectiveUpdate();
    }

    void SetToRightOrtho() 
    {
        currentView = CameraViews.RIGHT;
        myCam.orthographic = true;
        this.transform.eulerAngles = rightRotation;
        this.transform.SetParent(player.transform);
        RightOrthoUpdate();
    }

    void SetToTopOrtho()
    {
        currentView = CameraViews.TOP;
        myCam.orthographic = true;
        this.transform.eulerAngles = topRotation;
        this.transform.SetParent(player.transform);
        TopOrthoUpdate();
    }

    /******  UPDATE VARIOUS CAMERA VIEWS ******/
    void PerspectiveUpdate() {
        this.transform.position = ghostCamera.transform.position;
        this.transform.rotation = ghostCamera.transform.rotation;
    }

    void RightOrthoUpdate() {
        this.transform.position = new Vector3(player.transform.position.x + orthoOffset, player.transform.position.y, player.transform.position.z + headway);
    }

    void TopOrthoUpdate() {
        this.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + orthoOffset, player.transform.position.z + headway);
    }
}
