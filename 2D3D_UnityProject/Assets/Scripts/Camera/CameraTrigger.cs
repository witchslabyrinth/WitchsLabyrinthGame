using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Activates a camera when player actor enters the trigger zone
/// </summary>
[RequireComponent(typeof(Collider))]
public class CameraTrigger : MonoBehaviour
{
    /// <summary>
    /// Camera to use when player enters trigger 
    /// </summary>
    [SerializeField]
    private CameraEntity camera;

    /// <summary>
    /// Controls which perspective sprites the actor should use
    /// </summary>
    [SerializeField]
    private OldCameraController.CameraViews cameraView;

    /// <summary>
    /// Used to track actors within trigger bounds
    /// </summary>
    private List<Actor> actorsInTrigger = new List<Actor>();

    private PerspectiveController perspectiveController => PerspectiveController.Instance;
    private CameraController cameraController => CameraController.Instance;

    void Awake()
    {
        // Make sure we're pointing to a camera
        if(!camera)
            Debug.LogError($"Error: CameraTrigger {name} | missing reference to CameraEntity");

        // Make sure our collider is marked as a trigger
        if(TryGetComponent(out Collider trigger) && !trigger.isTrigger)
            Debug.LogError($"Error: CameraTrigger {name} | collider not marked as Trigger");
    }

    private void LateUpdate()
    {
        if (actorsInTrigger.Count == 0)
            return;

        // If player is in trigger zone, make sure we're using the referenced camera (handles actor swapping)
        Actor player = PlayerController.Instance.GetPlayer();
        if (actorsInTrigger.Contains(player) && cameraController.GetMainCamera() == player.actorCamera) {
            SwitchToCamera(player);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Ignore interactions with non-actor objects
        if (!other.TryGetComponent(out Actor actor))
            return;

        // Track actor in bounds
        actorsInTrigger.Add(actor);

        // If this is the player, switch to the referenced camera
        if (actor == PlayerController.Instance.GetPlayer())
        {
            SwitchToCamera(actor);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Ignore interactions with non-actor objects
        if (!other.TryGetComponent(out Actor actor))
            return;

        // Stop tracking this actor
        actorsInTrigger.Remove(actor);

        // If this is the player, switch back to actor camera
        if (actor == PlayerController.Instance.GetPlayer())
        {
            cameraController.SetMainCamera(actor.actorCamera);

            // Set actor to 3rd person perspective
            Perspective thirdPersonPerspective = perspectiveController.GetPerspectiveByType(OldCameraController.CameraViews.THIRD_PERSON);
            perspectiveController.SetPerspective(actor, thirdPersonPerspective);
        }
    }

    /// <summary>
    /// Switches to referenced camera, updating actor movement/animation accordingly
    /// </summary>
    /// <param name="actor"></param>
    private void SwitchToCamera(Actor actor)
    {
        // Set player movement/animation to match this camera's pespective
        perspectiveController.SetPerspective(actor, perspectiveController.GetPerspectiveByType(cameraView));

        // Switch to specified camera
        cameraController.SetMainCamera(camera);
    }
}
