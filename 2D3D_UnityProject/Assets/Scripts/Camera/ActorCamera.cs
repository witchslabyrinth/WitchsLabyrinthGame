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
        // Parent self to Actor > GhostCamera object
        //this.transform.SetParent (player.ghostCamera.transform);

        // Enable camera if attached to the player, disable if attached to the friend
        Actor player = PlayerController.Instance.GetPlayer();
        SetCameraActive(actor == player);

        // Default to 3D perspective view
        // TODO: make this correspond to the actor's actual starting perspective
        currentView = CameraViews.THIRD_PERSON;
    }

    /// <summary>
    /// Updates camera position/rotation relative to provided (player-controlled) Actor
    /// </summary>
    /// <param name="player">Actor currently controlled by player</param>
    public override void CameraUpdate () 
    {
        //Actor actor = PlayerController.Instance.GetPlayer();

        if(perspective.orthographic)
            OrthographicUpdate(actor, perspective.orthographicCameraOffset);
        else 
            PerspectiveUpdate(actor);
    }

    /// <summary>
    /// Initializes camera to follow specified Actor with specified Perspective.
    /// </summary>
    /// <param name="actor">Actor to follow</param>
    /// <param name="perspective">New camera perspective</param>
    public void SetPerspective(Actor actor, Perspective perspective)
    {
        // Save current perspective
        this.perspective = perspective;
        
        // Set fixed camera rotation for orthographic
        if(perspective.orthographic) {
            transform.eulerAngles = perspective.orthographicCameraRotation;
        }
        camera.orthographic = perspective.orthographic;

        // Parent CameraController to actor's ghostCamera
        // transform.SetParent(actor.ghostCamera.transform);
    }

    /// <summary>
    /// Updates camera position/rotation to follow the player.
    /// Called on Update() when using a 3D Perspective
    /// </summary>
    /// <param name="player">Player actor</param>
    private void PerspectiveUpdate (Actor player) 
    {
        // Update ghost camrea orientation
        player.ghostCamera.CameraUpdate();

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
