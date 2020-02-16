using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorCamera : CameraEntity
{
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

    /// <summary>
    /// Actor this camera is attached to
    /// </summary>
    [SerializeField]
    private Actor actor;

    void Start () 
    {
        // Enable camera if attached to the player, disable if attached to the friend
        Actor player = PlayerController.Instance.GetPlayer();
        SetCameraActive(actor == player);

        // Default to 3D perspective view
        // TODO: make this correspond to the actor's actual starting perspective
        currentView = CameraViews.THIRD_PERSON;

        // Track actor's starting perspective
        perspective = actor.perspective;
    }

    /// <summary>
    /// Updates camera position/rotation relative to provided (player-controlled) Actor
    /// </summary>
    /// <param name="player">Actor currently controlled by player</param>
    public override void CameraUpdate () 
    {
        if(perspective.orthographic)
            OrthographicUpdate();
        else 
            PerspectiveUpdate();
    }

    /// <summary>
    /// Initializes camera to follow specified Actor with specified Perspective.
    /// </summary>
    /// <param name="actor">Actor to follow</param>
    /// <param name="perspective">New camera perspective</param>
    public void SetPerspective(Perspective perspective)
    {
        // Check if we're changing to a new perspective
        // TODO: see if this is a good conditional to do a smooth perspective transition on
        if(perspective != this.perspective)
            Debug.LogFormat("Setting new perspective ({0}) on actor {1}", perspective.cameraView.ToString(), actor.name);

        // Save current perspective
        this.perspective = perspective;
        
        // Set fixed camera rotation for orthographic
        if(perspective.orthographic) {
            transform.eulerAngles = perspective.orthographicCameraRotation;
        }
        camera.orthographic = perspective.orthographic;
    }

    /// <summary>
    /// Updates camera position/rotation to follow the actor.
    /// Called on Update() when using a 3D Perspective
    /// </summary>
    private void PerspectiveUpdate () 
    {
        // Update ghost camrea orientation
        actor.ghostCamera.CameraUpdate();

        this.transform.position = actor.ghostCamera.transform.position;
        this.transform.rotation = actor.ghostCamera.transform.rotation;
    }

    /// <summary>
    /// Updates camera position to follow the player.
    /// Called on Update() when using an orthographic Perspective
    /// </summary>
    /// <param name="cameraOffset">Position offset between camera and actor</param>
    private void OrthographicUpdate()
    {
        transform.position = actor.transform.position + perspective.orthographicCameraOffset;
    }
}
