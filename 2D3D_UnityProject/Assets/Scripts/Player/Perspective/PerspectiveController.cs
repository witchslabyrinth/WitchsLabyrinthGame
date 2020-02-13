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
    public OldCameraController.CameraViews cameraView;

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
    private OldCameraController.CameraViews defaultPerspective;

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
    private Dictionary<KeyCode, OldCameraController.CameraViews> buttonPerspectiveMapping = new Dictionary<KeyCode, OldCameraController.CameraViews>() 
    {
        // Orthographic perspectives
        {KeyCode.Alpha2, OldCameraController.CameraViews.TOP},
        {KeyCode.Alpha3, OldCameraController.CameraViews.BACK},
        {KeyCode.Alpha4, OldCameraController.CameraViews.RIGHT},

        // 3D perspective
        {KeyCode.Alpha1, OldCameraController.CameraViews.THIRD_PERSON},
    };

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        // Set default perspective (fallback to 3D if default not found)
        Perspective perspective = cameraPerspectives.Find(i => i.cameraView.Equals(defaultPerspective));
        if(perspective == null) {
            Debug.LogWarningFormat("{0} | Specified perspective {1} not found, defaulting to 3D perspective", name, defaultPerspective);
            perspective = cameraPerspectives.Find(i => i.cameraView.Equals(OldCameraController.CameraViews.THIRD_PERSON));
        }
        
        // TODO: make sure we only set initial perspective in one place
        Actor player = PlayerController.Instance.GetPlayer();
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
                OldCameraController.CameraViews cameraView = buttonPerspectiveMapping[key];
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

        // Update actor with associated movement scheme
        player.SetMovement(perspective.movement);

        // Apply camera perspective to player actor
        // OldCameraController.Instance.SetPerspective(player, perspective);
        player.actorCamera.SetPerspective(player, perspective);

        // Enable ghost camera for 3D, disable for orthographic
        player.ghostCamera.enabled = !perspective.orthographic;

        // Update UI with selected perspective
        PerspectiveUI.Instance.SelectPerspective(perspective);
    }

    /// <summary>
    /// Returns Perspective associated with given CameraView type
    /// </summary>
    /// <param name="type">Type of perspective requested</param>
    /// <returns></returns>
    public Perspective GetPerspectiveByType(OldCameraController.CameraViews type)
    {
        return cameraPerspectives.Find(i => i.cameraView.Equals(type));
    }
}
