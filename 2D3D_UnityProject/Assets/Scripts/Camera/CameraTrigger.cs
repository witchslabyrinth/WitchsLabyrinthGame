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

    void Awake()
    {
        if(!camera)
            Debug.LogError($"Error: CameraTrigger {name} missing reference to CameraEntity");
    }

    private void OnTriggerEnter(Collider other)
    {
        // Ignore everything but the player actor
        Actor player = PlayerController.Instance.GetPlayer();
        if (other.gameObject != player.gameObject)
            return;

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

        // Restore actor camera
        Debug.LogWarning($"Switching to actor camera ({player.name})");
        CameraController.Instance.SetMainCamera(player.actorCamera);
    }
}
