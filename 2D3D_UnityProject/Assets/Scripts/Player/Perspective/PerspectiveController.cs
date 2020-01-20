using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Holds data used for each camera perspective
/// </summary>
[System.Serializable]
public class Perspective
{
    /// <summary>
    /// Unique perspective identifier
    /// </summary>
    public CameraController.CameraViews cameraView;

    /// <summary>
    /// Reference to movement scheme assocaited with perspective
    /// </summary>
    public Movement movement;

    [Header("Orthographic Settings")]
    /// <summary>
    /// True for orthographic camera perspectives, false otherwise
    /// </summary>
    public bool orthographic;
    
    /// <summary>
    /// Camera rotation used for orthographic camera views (ignored for 3D perspective)
    /// </summary>
    public Vector3 orthographicCameraRotation;

    /// <summary>
    /// Position offset between camera and actor (ignored for 3D perspectives)
    /// </summary>
    public Vector3 orthographicCameraOffset;
}


/// <summary>
/// Used for changing changing player camera perspective
/// </summary>
public class PerspectiveController : Singleton<PerspectiveController>
{
    /// <summary>
    /// Default perspective used at game start
    /// </summary>
    [SerializeField]
    private CameraController.CameraViews defaultPerspective;

    /// <summary>
    /// List of camera perspectives
    /// </summary>
    [SerializeField]
    private List<Perspective> cameraPerspectives;

    /// <summary>
    /// Mapping of keycodes to corresponding camera perspectives
    /// </summary>
    /// <typeparam name="KeyCode">Button to press</typeparam>
    /// <typeparam name="CameraController.CameraView">Camera perspective applied when button is pressed</typeparam>
    /// <returns></returns>
    private Dictionary<KeyCode, CameraController.CameraViews> buttonPerspectiveMapping = new Dictionary<KeyCode, CameraController.CameraViews>() 
    {
        // Orthographic perspectives
        {KeyCode.UpArrow, CameraController.CameraViews.TOP},
        {KeyCode.DownArrow, CameraController.CameraViews.BACK},
        {KeyCode.RightArrow, CameraController.CameraViews.RIGHT},

        // 3D perspective
        {KeyCode.LeftArrow, CameraController.CameraViews.THIRD_PERSON},
    };

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        // Set default perspective
        Perspective perspective = cameraPerspectives.Find(i => i.cameraView.Equals(defaultPerspective));
        if(perspective == null) {
            Debug.LogWarning(name + " | No perspective specified, defaulting to 3D perspective");
            perspective = cameraPerspectives.Find(i => i.cameraView.Equals(CameraController.CameraViews.THIRD_PERSON));
        }
        
        Actor player = PlayerController.Instance.GetActor();
        SetPerspective(player, perspective);
    }

    /// <summary>
    /// Detects perspective-change keypress (defined in buttonPerspectiveMapping) and updates perspective accordingly
    /// </summary>
    /// <param name="player">Player actor to update perspective for</param>
    public void UpdatePerspective(Actor player)
    {
        // Check each button in KeyCode -> camera perspective mapping
        foreach(KeyCode key in buttonPerspectiveMapping.Keys) 
        {
            // If button is being pressed
            if (Input.GetKeyDown(key)) 
            {
                // Try and pull associated Perspective from list
                CameraController.CameraViews cameraView = buttonPerspectiveMapping[key];
                Perspective perspective = cameraPerspectives.Find(i => i.cameraView.Equals(cameraView));

                // Continue with error if perspective not found
                if(perspective == null) 
                {
                    Debug.LogErrorFormat("ERROR - no perspective linked to {0} in PerspectiveController.perspectives list", key);
                    continue;
                }
                
                SetPerspective(player, perspective);

                return;
            }
        }
    }

    public void SetPerspective(Actor player, Perspective perspective)
    {
        // Store perspective in actor
        player.perspective = perspective;

        // Apply camera perspective
        CameraController.Instance.SetPerspective(player, perspective);

        // Enable ghost camera for 3D, disable for orthographic
        player.ghostCamera.enabled = !perspective.orthographic;
    }

    /// <summary>
    /// Returns Perspective associated with given CameraView type
    /// </summary>
    /// <param name="type">Type of perspective requested</param>
    /// <returns></returns>
    public Perspective GetPerspectiveByType(CameraController.CameraViews type)
    {
        return cameraPerspectives.Find(i => i.cameraView.Equals(type));
    }
}
