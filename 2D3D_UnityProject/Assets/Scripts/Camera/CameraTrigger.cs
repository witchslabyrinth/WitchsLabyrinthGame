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
    /// If true, player won't be able to switch perspectives inside the trigger
    /// </summary>
    [SerializeField]
    private bool restrictPerspectiveSwitching = true;

    /// <summary>
    /// Used to track actors within trigger bounds
    /// </summary>
    private List<Actor> actorsInTrigger = new List<Actor>();

    #region singleton_references
    private PerspectiveController perspectiveController => PerspectiveController.Instance;
    private CameraController cameraController => CameraController.Instance;
    private PlayerController playerController => PlayerController.Instance;
    #endregion

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
        Actor player = playerController.GetPlayer();
        if (actorsInTrigger.Contains(player) && cameraController.GetMainCamera() == player.actorCamera) {
            SwitchToReferencedCamera(player);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Ignore interactions with non-actor objects
        if (!other.TryGetComponent(out Actor actor))
            return;

        // Track actor in bounds
        actorsInTrigger.Add(actor);

        // Prevent actor from changing perspectives if specified
        actor.canSwitchPerspectives = !restrictPerspectiveSwitching;

        // If player entered trigger, switch to the referenced camera
        if (actor == playerController.GetPlayer())
            SwitchToReferencedCamera(actor);
    }

    private void OnTriggerExit(Collider other)
    {
        // Ignore interactions with non-actor objects
        if (!other.TryGetComponent(out Actor actor))
            return;

        // Stop tracking this actor, and restore perspective switching
        actorsInTrigger.Remove(actor);
        actor.canSwitchPerspectives = true;

        // If player left trigger, switch back to actor camera
        if (actor == playerController.GetPlayer())
            SwitchToActorCamera(actor);
    }

    /// <summary>
    /// Switches to referenced camera, updating actor movement/animation accordingly
    /// </summary>
    /// <param name="player">Player actor</param>
    private void SwitchToReferencedCamera(Actor player)
    {
        // Set player movement/animation to match this camera's pespective
        perspectiveController.SetPerspective(player, perspectiveController.GetPerspectiveByType(cameraView));

        // Switch to specified camera
        cameraController.SetMainCamera(camera);
    }
    
    /// <summary>
    /// Restores control to actor camera
    /// </summary>
    /// <param name="player">Player actor</param>
    private void SwitchToActorCamera(Actor player)
    {
        cameraController.SetMainCamera(player.actorCamera);

        // Set actor to 3rd person perspective
        perspectiveController.SetPerspective(player, perspectiveController.GetPerspectiveByType(OldCameraController.CameraViews.THIRD_PERSON));
    }
}
