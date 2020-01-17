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

    public enum CameraViews {
        RIGHT,
        TOP,
        BACK,
        THIRD_PERSON,
    }
    protected CameraViews currentView;
    protected Perspective perspective;

    void Start () 
    {
        // Parent self to Actor > GhostCamera object
        Actor player = PlayerController.Instance.GetActor();
        this.transform.SetParent (player.ghostCamera.transform);

        if(!TryGetComponent(out myCam)) {
            Debug.LogError("ERROR: Camera component not found on CameraController");
        }

        // Default to 3D perspective view
        currentView = CameraViews.THIRD_PERSON;
    }

    /// <summary>
    /// Updates camera position/rotation relative to provided (player-controlled) Actor
    /// </summary>
    /// <param name="player">Actor currently controlled by player</param>
    public void CameraUpdate (Actor player) 
    {
        if(perspective.orthographic)
            OrthographicUpdate(player, perspective.orthographicCameraOffset);
        else 
            PerspectiveUpdate(player);
    }

    public void SetPerspective(Actor player, Perspective perspective)
    {
        // Update actor with associated movement scheme
        player.SetMovement(perspective.movement);
        
        // Save current perspective
        this.perspective = perspective;
        
        myCam.orthographic = perspective.orthographic;

        // Set fixed camera rotation for orthographic
        if(perspective.orthographic) {
            transform.eulerAngles = perspective.orthographicCameraRotation;
        }
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