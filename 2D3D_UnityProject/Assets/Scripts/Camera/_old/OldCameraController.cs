using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Manipulates the position/rotation of the camera based on the currently-selected camera perspective
/// </summary>
[RequireComponent(typeof(Camera))]
public class OldCameraController : Singleton<OldCameraController>
{
    /// <summary>
    /// Reference to main camera
    /// </summary>
    protected Camera mainCamera;

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
        mainCamera = GetComponent<Camera>();

        // Parent self to Actor > GhostCamera object
        Actor player = PlayerController.Instance.GetPlayer();
        this.transform.SetParent (player.ghostCamera.transform);

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

    /// <summary>
    /// Initializes camera to follow specified Actor with specified Perspective.
    /// </summary>
    /// <param name="actor">Actor to follow</param>
    /// <param name="perspective">New camera perspective</param>
    public void SetPerspective(Actor actor, Perspective perspective)
    {
        // Make sure we have ref to Camera component
        if (!mainCamera)
            mainCamera = GetComponent<Camera>();

        // Save current perspective
        this.perspective = perspective;
        
        // Set fixed camera rotation for orthographic
        if(perspective.orthographic) {
            transform.eulerAngles = perspective.orthographicCameraRotation;
        }
        mainCamera.orthographic = perspective.orthographic;

        // Parent CameraController to actor's ghostCamera
        transform.SetParent(actor.ghostCamera.transform);
    }

    /// <summary>
    /// Updates camera position/rotation to follow the player.
    /// Called on Update() when using a 3D Perspective
    /// </summary>
    /// <param name="player">Player actor</param>
    private void PerspectiveUpdate (Actor player) 
    {
        this.transform.position = player.ghostCamera.transform.position;
        this.transform.rotation = player.ghostCamera.transform.rotation;
    }

    /// <summary>
    /// Updates camera position to follow the player.
    /// Called on Update() when using an orthographic Perspective
    /// </summary>
    /// <param name="player">Player actor</param>
    /// <param name="cameraOffset">Position offset between camera and player</param>
    private void OrthographicUpdate(Actor player, Vector3 cameraOffset)
    {
        transform.position = player.transform.position + cameraOffset;
    }
}