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
    /// Actor currently controlled by the player
    /// </summary>
    [SerializeField]
    private Actor player;

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

    private void Awake()
    {
        // Make sure we have an actor (default to oliver if not specified)
        if(player == null) {
            player = oliver;
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
            if (player.Equals(cat))
                Swap(cat, oliver);
            else
                Swap(oliver, cat);
        }
    }

    /// <summary>
    /// Swaps player control from currentActor to friendActor
    /// </summary>
    /// <param name="currentActor">Actor currently controlled by player</param>
    /// <param name="friendActor">Friend actor (not currently controlled by player)</param>
    private void Swap(Actor currentActor, Actor friendActor)
    {
        // Switch player control to friend
        player = friendActor;

        // Set previous actor to idle
        currentActor.SetMovement(new NullMovement());

        // Restore player actor's previous perspective
        PerspectiveController.Instance.SetPerspective(player, player.perspective);
    }

    /// <summary>
    /// Returns reference to currently-controlled Actor (the player)
    /// </summary>
    /// <returns></returns>
    public Actor GetActor()
    {
        return player;
    }
}