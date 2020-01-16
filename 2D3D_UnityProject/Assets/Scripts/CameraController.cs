using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manipulates the position/rotation of the camera based on the currently-selected camera perspective
/// </summary>
[RequireComponent(typeof(Camera))]
public class CameraController : Singleton<CameraController> {
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

    void Start () 
    {
        // Parent self to Actor > GhostCamera object
        Actor player = PlayerController.Instance.GetActor();
        this.transform.SetParent (player.ghostCamera.transform);

        if(!TryGetComponent(out myCam)) {
            Debug.LogError("ERROR: Camera component not found on CameraController");
        }

        // Default to 3D perspective view
        currentView = CameraViews.PERSPECTIVE;
    }

    /// <summary>
    /// Updates camera position/rotation relative to provided (player-controlled) Actor
    /// </summary>
    /// <param name="player">Actor currently controlled by player</param>
    public void CameraUpdate (Actor player) {
        switch (currentView) 
        {
            case CameraViews.PERSPECTIVE:
                PerspectiveUpdate(player);
                break;
            case CameraViews.RIGHT:
                OrthographicUpdate(player, new Vector3(headway, 4f, headway));
                break;
            case CameraViews.TOP:
                OrthographicUpdate(player, new Vector3(0, 20f, 2f));
                break;
            case CameraViews.BACK:
                OrthographicUpdate(player, new Vector3(headway, 4f, -orthoOffset));
                break;
        }
    }

    /******  SET VARIOUS CAMERA VIEWS ******/
    public void SetToPerspective () {
        currentView = CameraViews.PERSPECTIVE;
        myCam.orthographic = false;
    }

    public void SetToRightOrtho () {
        currentView = CameraViews.RIGHT;
        myCam.orthographic = true;
        this.transform.eulerAngles = rightRotation;
    }

    public void SetToTopOrtho () {
        currentView = CameraViews.TOP;
        myCam.orthographic = true;
        this.transform.eulerAngles = topRotation;
    }

    public void SetToBackOrtho () {
        currentView = CameraViews.BACK;
        myCam.orthographic = true;
        this.transform.eulerAngles = backRotation;
    }

    /******  UPDATE VARIOUS CAMERA VIEWS ******/
    private void PerspectiveUpdate (Actor player) {
        this.transform.position = player.ghostCamera.transform.position;
        this.transform.rotation = player.ghostCamera.transform.rotation;
    }

    private void OrthographicUpdate(Actor player, Vector3 cameraOffset)
    {
        transform.position = player.transform.position + cameraOffset;
    }
}