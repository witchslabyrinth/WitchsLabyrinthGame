using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    /// <summary>
    /// Used for handling interactions with other entities (dialogue, puzzles, etc)
    /// </summary>
    private PlayerInteractionController interactionController;

    // TODO: consider removing CharacterController - only used for movement
    /// <summary>
    /// Used for applying player movement
    /// </summary>
    protected CharacterController controller;

    // TODO: move to a camera-related class
    /// <summary>
    /// 3D perspective camera 
    /// </summary>
    public GameObject ghostCamera;

    /// <summary>
    /// Base movement speed
    /// </summary>
    [SerializeField]
    protected float movementSpeed = 15f;

    /// <summary>
    /// Used to generate movement from player input - varies depending on current camera perspective, or assigned NPC behaviors
    /// </summary>
    [SerializeField]
    protected Movement movement;

    /// <summary>
    /// Handles animation for this actor
    /// </summary>
    [SerializeField]
    protected AnimationController animationController;

    void Start()
    {
        controller = this.GetComponent<CharacterController>();
        interactionController = GetComponent<PlayerInteractionController>();

        // TODO: set default movement scheme and camera perspective in the same place
        // Set default movement scheme to 3D perspective if none selected
        if(movement == null) {
            movement = new PerspectiveMovement(ghostCamera.GetComponent<Camera>());
        }
    }

    void Update()
    {
        // Get move direction (as unit vector) from movement class
        Vector3 direction = movement.Get(transform);

        // Apply movement (scaled by movement speed)
        float magnitude = movementSpeed * Time.fixedDeltaTime;
        controller.Move(direction * magnitude);

        // Handle interactions with other game entities
        interactionController.CheckInteraction();

        // @ Victor:
        // TODO: use movement data and send it to the AnimationController to show the proper animations
    }

    public void SetMovementType(Movement movement)
    {
        this.movement = movement;
    }
}