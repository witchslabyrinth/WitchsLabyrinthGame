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
    private PerspectiveController perspectiveController => PerspectiveController.Instance;

    void Awake()
    {
        // Make sure we're pointing to a camera
        if(!camera)
            Debug.LogError($"Error: CameraTrigger {name} | missing reference to CameraEntity");

        // Make sure our collider is marked as a trigger
        if(TryGetComponent(out Collider trigger) && !trigger.isTrigger)
            Debug.LogError($"Error: CameraTrigger {name} | collider not marked as Trigger");
    }

    private void OnTriggerEnter(Collider other)
    {
        // Ignore everything but the player actor
        Actor player = PlayerController.Instance.GetPlayer();
        if (other.gameObject != player.gameObject)
            return;

        // Set player movement/animation to match this camera's pespective
        perspectiveController.SetPerspective(player, perspectiveController.GetPerspectiveByType(cameraView));
        //player.SetTopView(cameraView == OldCameraController.CameraViews.TOP);

        // Switch to specified camera
        Debug.LogWarning($"Switching to trigger camera: {camera.name}");
        CameraController.Instance.SetMainCamera(camera);
    }

    private void OnTriggerExit(Collider other)
    {
        // Ignore everything but the player actor
        Actor player = PlayerController.Instance.GetPlayer();
        if (other.gameObject != player.gameObject)
            return;

        // Switch to actor camera
        Debug.LogWarning($"Switching to actor camera ({player.name})");
        CameraController.Instance.SetMainCamera(player.actorCamera);

        // Set actor to 3rd person perspective
        Perspective thirdPersonPerspective = perspectiveController.GetPerspectiveByType(OldCameraController.CameraViews.THIRD_PERSON);
        perspectiveController.SetPerspective(player, thirdPersonPerspective);
    }
}
