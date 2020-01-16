using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    #region components

    /// <summary>
    /// Used for applying player movement
    /// </summary>
    protected Rigidbody rigidbody;

    /// <summary>
    /// Handles animation for this actor
    /// </summary>
    [SerializeField]
    protected AnimationController animationController;

    /// <summary>
    /// Used for handling interactions with other entities (dialogue, puzzles, etc)
    /// </summary>
    private PlayerInteractionController interactionController;

    // TODO: move to a camera-related class
    /// <summary>
    /// 3D perspective camera 
    /// </summary>
    public PerspectiveCameraControl ghostCamera { get; private set;}

    #endregion

    [Header("Movement Settings")]
    /// <summary>
    /// Base movement speed
    /// </summary>
    [SerializeField]
    protected float movementSpeed = 10f;

    /// <summary>
    /// Speed multiplier when sprinting
    /// </summary>
    [SerializeField]
    protected float sprintMultiplier = 1.5f;

    /// <summary>
    /// Used to generate Actor movement - varies depending on current camera perspective, or assigned NPC behavior
    /// </summary>
    [SerializeField] 
    protected Movement movement;


    void Awake()
    {
        // Get components in Awake() before other scripts request them in Start()
        rigidbody = GetComponent<Rigidbody>();
        interactionController = GetComponent<PlayerInteractionController>();
        ghostCamera = GetComponentInChildren<PerspectiveCameraControl>();
    }

    private void Start()
    {
        // TODO: set default movement scheme and camera perspective in the same place
        // Make sure there's a movement type specified
        if (movement == null)
        {
            // Throw error if no movement specified for player actor
            if (PlayerController.Instance.GetActor() == this)
            {
                Debug.LogError("No movement type specified for player-controlled Actor");
            }
            // Otherwise default to NullMovement (idle)
            else
            {
                Debug.LogWarning(name + " | no Movement specified for this Actor, defaulting to NullMovement");
                movement = new NullMovement();
            }
        }
    }

    private void FixedUpdate()
    {
        // Move actor
        Move();

        // Update animations
        Animate();
    }

    /// <summary>
    /// Applies movement, returns vector representing movement
    /// </summary>
    /// <returns>Vector of movement</returns>
    private Vector3 Move()
    {
        // Get move direction (as unit vector) from movement class
        Vector3 direction = movement.GetMovement(this);

        // Apply movement (scaled by movement speed)
        float magnitude = movementSpeed * Time.fixedDeltaTime;

        // Apply speed multiplier if sprinting
        if(Input.GetKey(KeyCode.LeftShift)) {
            magnitude *= sprintMultiplier;
        }

        // Apply movement
        Vector3 moveVector = direction * magnitude;
        rigidbody.MovePosition(transform.position + moveVector);

        // Return movement
        return moveVector;
    }

    private void Animate()
    {
        // Get movement direction as a unit vector (corresponds to player inputs, ignoring invalid movement directions)
        Vector2 direction = movement.GetAnimation(this);

        // Generate proper animations bsed on movement (on x-z plane)
        animationController.UpdateAnims(direction);
    }

    public void SetMovementType(Movement movement)
    {
        this.movement = movement;
    }

    /// <summary>
    /// Used for handling interactions with puzzles/NPCs/etc
    /// </summary>
    public void CheckInteraction()
    {
        interactionController.CheckInteraction();
    }

    /// <summary>
    /// Shows actor and restores control/behavior
    /// </summary>
    public void Enable()
    {
        SetActive(true);
    }

    /// <summary>
    /// Hides actor and revokes control/behavior
    /// </summary>
    public void Disable()
    {
        SetActive(false);
    }

    /// <summary>
    /// Sets actor functionality (camera, movement, visibility, etc)
    /// </summary>
    /// <param name="active"></param>
    private void SetActive(bool active)
    {
        gameObject.SetActive(active);
        interactionController.enabled = active;
        ghostCamera.enabled = active;
    }
}