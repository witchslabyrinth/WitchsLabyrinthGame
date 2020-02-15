using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : Singleton<CameraController>
{
    /// <summary>
    /// Handles camera currently being displayed on screen
    /// </summary>
    [SerializeField]
    private CameraEntity main;

    /// <summary>
    /// Camera used to render outline effect
    /// </summary>
    private Camera outlineCamera => RenderReplacementShaderToTexture.camera;

    /// <summary>
    /// Reference to previous CameraEntity used before calling SetMainCamera()
    /// </summary>
    private CameraEntity previous;

    // Start is called before the first frame update
    void Awake()
    {
        // Default main camera to player's actor cam
        if(!main) 
        {
            Actor player = PlayerController.Instance.GetPlayer();
            main = player.actorCamera;
        }

        // TODO: disable all other Cameras in scene?
    }

    public CameraEntity GetMainCamera()
    {
        return main;
    }

    public void SetMainCamera(CameraEntity cameraEntity)
    {
        // Set new camera and store reference to previously-used one (can be restored later)
        previous = main;
        main = cameraEntity;

        // Disable previous camera, enable new one
        previous.SetCameraActive(false);
        main.SetCameraActive(true);

        // Make the outline camera a child of the main camera (so it renders the outline on top)
        outlineCamera.transform.SetParent(main.transform, false);
    }
}
