using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Actor : MonoBehaviour
{
    // TODO: consider removing CharacterController - only used for movement
    /// <summary>
    /// Used for applying player movement
    /// </summary>
    protected CharacterController controller;

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
    public GameObject ghostCamera;

    [Header("Movement Settings")]

    /// <summary>
    /// Base movement speed
    /// </summary>
    [SerializeField]
    protected float movementSpeed = 15f;

    /// <summary>
    /// Used to generate Actor movement - varies depending on current camera perspective, or assigned NPC behavior
    /// </summary>
    [SerializeField]
    protected Movement movement;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        interactionController = GetComponent<PlayerInteractionController>();

        // TODO: set default movement scheme and camera perspective in the same place
        // Make sure there's a movement type specified
        if(movement == null) {
            // Throw error if no movement specified for player actor
            if(PlayerController.Instance.GetActor() == this) {
                Debug.LogError("No movement type specified for player-controlled Actor");
            }
            // Otherwise default to following player
            // TODO: maybe not a good idea, find a way around this
            else {
                Debug.Log(name + " | no Movement specified for this Actor, defaulting to FollowMovement");
                movement = new FollowMovement(PlayerController.Instance.GetActor().transform);
            }
        }
    }

    void Update()
    {
        // Move actor
        Move();

        // Update animations based on movement
        Animation();

        // @ Victor:
        // TODO: use movement data (value returned from Movement.Get(), or Actor.Move() if you want direction + magnitude) 
        // and send it to the AnimationController to show the proper animations
        //
        // Try and keep your code limited to this class and AnimationController.cs - you shouldn't have to touch PlayerController for any of this
    }

    /// <summary>
    /// Applies movement, returns vector representing movement
    /// </summary>
    /// <returns>Vector of movement</returns>
    private Vector3 Move()
    {
        // Get move direction (as unit vector) from movement class
        Vector3 direction = movement.Get(transform);

        // Apply movement (scaled by movement speed)
        float magnitude = movementSpeed * Time.fixedDeltaTime;
        Vector3 moveVector = direction * magnitude;
        controller.Move(moveVector);

        // Return movement
        return moveVector;
    }

    private void Animation()
    {
        // Get movement as a unit vector
        Vector3 moveVector = movement.Get(transform);

        // TOD: Interpret movement relative to camera perspective??
        // Generate proper animations bsed on movement (on x-z plane)
        animationController.UpdateAnims(new Vector2(moveVector.x, moveVector.z));
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
}