using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : Singleton<PlayerController>
{
    // TODO: see if there's a better way (maybe event-driven?) to handle this
    /// <summary>
    /// True when player can swap between actors, false when swapping is disabled
    /// </summary>
    public bool canSwap = true;

    /// <summary>
    /// Reference to Oliver
    /// </summary>
    [SerializeField]
    private Actor oliver;

    /// <summary>
    /// Reference to Cat
    /// </summary>
    [SerializeField]
    private Actor cat;

    /// <summary>
    /// Actor currently controlled by the player
    /// </summary>
    [SerializeField]
    private Actor player;

    /// <summary>
    /// Actor not currently controlled by the player
    /// </summary>
    private Actor friend
    {
        get
        {
            if (player.Equals(oliver))
                return cat;
            else if (player.Equals(cat))
                return oliver;

            // Return null if player actor undefined
            else
            {
                Debug.LogWarningFormat("{0} | Friend actor undefined", name);
                return null;
            }
        }
        set { }
    }

    private void Awake()
    {
        // Make sure we have an actor (default to oliver if not specified)
        if(player == null) {
            player = oliver;
            friend = cat;
        }

        // Throw warnings and disable swapping if oliver/cat not found
        if (!oliver)
        {
            Debug.LogWarning("Warning: Oliver actor not found in PlayerController");
            canSwap = false;
        }
        if (!cat)
        {
            Debug.LogWarning("Warning: Cat actor not found in PlayerController");
            canSwap = false;
        }
        
    }

    private void Update()
    {
        // Handle player input for actor swapping
        ActorSwapUpdate();

        // Handle interactions with other game entities
        player.CheckInteraction();
        
        // Update camera perspective
        PerspectiveController.Instance.UpdatePerspective(player);

        // Update camera to follow player actor
        CameraController.Instance.CameraUpdate(player);
    }

    /// <summary>
    /// Swaps player control between actors based on input (no effect when canSwap is false)
    /// </summary>
    private void ActorSwapUpdate()
    {
        // Skip if swapping is disabled
        if (!canSwap)
            return;
        
        // Toggle currently-controlled actor between oliver/cat
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            Swap();
        }
    }

    /// <summary>
    /// Switches player control to Friend actor
    /// </summary>
    private void Swap()
    {
        // Swap player/friend actors
        Actor temp = player;
        player = friend;
        friend = temp;

        // Copy the friend follow/idle movement over to the new actor
        friend.SetMovement(player.movement);

        // Restore player actor's previous perspective
        PerspectiveController.Instance.SetPerspective(player, player.perspective);
    }

    /// <summary>
    /// Returns reference to currently-controlled Actor (the player)
    /// </summary>
    public Actor GetPlayer()
    {
        return player;
    }

    public Actor GetFriend()
    {
        return friend;
    }
}