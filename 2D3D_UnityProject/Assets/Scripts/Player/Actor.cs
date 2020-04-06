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

    /// <summary>
    /// Controls actor's camera positioning
    /// </summary>
    public ActorCamera actorCamera;

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
    /// Current status of actor's movement
    /// 0 - stationary
    /// 1 - walking
    /// 2 - sprinting
    /// </summary>
    private int moveStatus;

    /// <summary>
    /// Used to generate Actor movement - varies depending on current camera perspective, or assigned NPC behavior
    /// </summary>
    public Movement movement;

    [Header("Footstep Settings")]

    /// <summary>
    /// Controls how often footstep sound effect plays
    /// </summary>
    [Range(0,1)]
    [SerializeField]
    private float footstepFrequency = .5f;

    /// <summary>
    /// For updating animations, stores whether or not player is using top view
    /// </summary>
    private bool inTopView;

    /// <summary>
    /// Position the actor started at on scene load.
    /// </summary>
    public Vector3 spawnPosition { get; private set; }

    /// Sets actor movement control scheme
    /// </summary>
    /// <param name="movement">Movement control scheme</param>
    public void SetMovement(Movement movement)
    {
        this.movement = movement;
    }

    [Header("Perspective Settings")]

    /// <summary>
    /// Current perspective used for this actor (remains unchanged when not player-controlled)
    /// </summary>
    /// <value></value>
    public Perspective perspective;

    /// <summary>
    /// Determines whether this actor currently allows player to switch perspectives
    /// </summary>
    public bool canSwitchPerspectives = true;

    void Awake()
    {
        // Get components in Awake() before other scripts request them in Start()
        rigidbody = GetComponent<Rigidbody>();
        interactionController = GetComponent<PlayerInteractionController>();
        ghostCamera = GetComponentInChildren<PerspectiveCameraControl>();

        if (!actorCamera)
        {
            Debug.LogWarning(name + " | missing reference to ActorCamera; attempting to find via FindObjectOfType()...");
            actorCamera = FindObjectOfType<ActorCamera>();
        }

        // Save spawn position, so if we fall out of map we can respawn there
        spawnPosition = transform.position;
    }

    private void Start()
    {
        // TODO: make sure we only set initial perspective in one place
        // Default to third-person perspective for both actors
        perspective = PerspectiveController.Instance.GetPerspectiveByType(OldCameraController.CameraViews.THIRD_PERSON);

        // Give player movement control
        if (PlayerController.Instance.GetPlayer() == this) {
            movement = perspective.movement;
        }
        // Give friend Follow movement (if no movement specified)
        else if(movement == null) {
            movement = new FollowMovement(PlayerController.Instance.GetPlayer().transform);
        }

        moveStatus = 0;
        inTopView = false;

        // Start footstep coroutine
        if (playFootstepCoroutine == null)
            playFootstepCoroutine = StartCoroutine(PlayFootstep());
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
        // If player-controlled actor has NullMovement scheme, restore movement from last-used perspective
        if(PlayerController.Instance.GetPlayer().Equals(this) && movement.GetType() == typeof(NullMovement)) {
            movement = perspective.movement;
        }
        
        // Get move direction (as unit vector) from movement class
        Vector3 direction = movement.GetMovement(this);
        if(direction != Vector3.zero)
        {
            moveStatus = 1;
        }
        else
        {
            moveStatus = 0;
        }

        // Apply movement (scaled by movement speed)
        float magnitude = movementSpeed * Time.fixedDeltaTime;

        // Apply speed multiplier if sprinting
        if(Input.GetKey(KeyCode.LeftShift)) 
        {
            magnitude *= sprintMultiplier;
            moveStatus *= 2;
        }

        // Apply movement
        Vector3 moveVector = direction * magnitude;
        rigidbody.MovePosition(transform.position + moveVector);

        // Return movement
        return moveVector;
    }

    private Coroutine playFootstepCoroutine;
    private IEnumerator PlayFootstep()
    {
        while (true)
        {
            // Play footsteps when player is walking/running
            if(moveStatus >= 1 && PlayerController.Instance.GetPlayer() == this) {
                AkSoundEngine.PostEvent("Footsteps", gameObject);
            }
            yield return new WaitForSeconds(footstepFrequency);
        }
    }

    private void Animate()
    {
        // Get movement direction as a unit vector (corresponds to player inputs, ignoring invalid movement directions)
        Vector2 direction = movement.GetAnimation(this);

        // Generate proper animations bsed on movement (on x-z plane)
        animationController.UpdateAnims(direction, moveStatus, inTopView);
    }

    /// <summary>
    /// Used for handling interactions with puzzles/NPCs/etc
    /// </summary>
    public void CheckInteraction()
    {
        if (enabled)
            interactionController.CheckInteraction();
    }

    public void SetTopView(bool isTop)
    {
        inTopView = isTop;
    }

    /// <summary>
    /// Shows actor and restores control/behavior
    /// </summary>
    public void Enable()
    {
        SetActive(true);

        // Turn footsteps back on
        if(playFootstepCoroutine == null)
            playFootstepCoroutine = StartCoroutine(PlayFootstep());
    }

    /// <summary>
    /// Hides actor and revokes control/behavior
    /// </summary>
    public void Disable()
    {
        SetActive(false);

        // Turn off footsteps (should be taken care of by gameObject.SetActive(), but just in case)
        StopCoroutine(playFootstepCoroutine);
        playFootstepCoroutine = null;
    }

    /// <summary>
    /// Sets actor functionality (camera, movement, visibility, etc)
    /// </summary>
    /// <param name="active"></param>
    private void SetActive(bool active)
    {
        gameObject.SetActive(active);
        enabled = active;

        // Re-enable ghost camera (for 3D perspective only)
        if(perspective.cameraView.Equals(OldCameraController.CameraViews.THIRD_PERSON)) 
            ghostCamera.enabled = active;

        // Hide/show perspective UI
        PerspectiveUI.Instance.SetActive(active);
    }
}