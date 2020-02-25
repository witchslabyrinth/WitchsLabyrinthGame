using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorCamera : CameraEntity
{
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

    /// <summary>
    /// Speed of camera transitions between perspectives
    /// </summary>
    [SerializeField]
    [Range(0, 1.5f)]
    private float perspectiveTransitionSpeed;

    public Vector3 lastThirdPersonCameraPosition;
    public Vector3 lastThirdPersonCameraRotation;

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
        if (transitionPerspectiveCoroutine != null)
            return;

        if(perspective.orthographic)
            OrthographicUpdate();
        else 
            PerspectiveUpdate();
    }

    /// <summary>
    /// Initializes camera to follow specified Actor with specified Perspective.
    /// </summary>
    /// <param name="perspective">New camera perspective</param>
    public void SetPerspective(Perspective perspective)
    {
        // Check if we're changing to a new perspective
        // TODO: see if this is a good conditional to do a smooth perspective transition on
        if (perspective == this.perspective || transitionPerspectiveCoroutine != null)
            return;
        Debug.LogFormat("Setting new perspective ({0}) on actor {1}", perspective.cameraView.ToString(), actor.name);

        if (this.perspective != null)
            transitionPerspectiveCoroutine = StartCoroutine(TransitionPerspective(this.perspective, perspective));
    }

    private Coroutine transitionPerspectiveCoroutine;
    private IEnumerator TransitionPerspective(Perspective from, Perspective to)
    {
        // Update orthographic setting before moving camera, if we're not currently orthographic
        camera.orthographic = to.orthographic;

        // Disable swapping until transition complete
        PlayerController.Instance.canSwap = false;

        // Lerp from camera's starting transfrom values
        Vector3 startPosition = transform.position;
        Vector3 startRotation = transform.eulerAngles;

        Vector3 endPosition = to.orthographic ? to.orthographicCameraOffset : lastThirdPersonCameraPosition;
        Vector3 endRotation = to.orthographic ? to.orthographicCameraRotation: lastThirdPersonCameraRotation;
        Vector3 endPositionOffset = Vector3.zero;

        float t = 0;
        while(t < 1) {
            // Only apply fixed position offset in orthographic perspectives
            endPositionOffset = to.orthographic ? actor.transform.position : Vector3.zero;

            // Update position/rotation values
            transform.position =  Mathfx.Sinerp(startPosition, endPosition + endPositionOffset, t);
            transform.eulerAngles =  Mathfx.Sinerp(startRotation, endRotation, t);

            float step = 1 / perspectiveTransitionSpeed * Time.deltaTime;
            t += step;
            yield return null;
        }

        // Make sure we hit the target
        transform.position = endPosition + endPositionOffset;
        transform.eulerAngles = endRotation;

        // Update camera perspective
        perspective = to;

        // Restore swapping
        PlayerController.Instance.canSwap = true;
        transitionPerspectiveCoroutine = null;
    }

    /// <summary>
    /// Updates camera position/rotation to follow the actor.
    /// Called on Update() when using a 3D Perspective
    /// </summary>
    private void PerspectiveUpdate () 
    {
        // Update ghost camrea orientation
        actor.ghostCamera.CameraUpdate();

        transform.rotation = actor.ghostCamera.transform.rotation;
        transform.position = actor.ghostCamera.transform.position;

        lastThirdPersonCameraPosition = actor.ghostCamera.transform.position;
        lastThirdPersonCameraRotation = new Vector3(transform.eulerAngles.x, actor.transform.eulerAngles.y, 0);
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
