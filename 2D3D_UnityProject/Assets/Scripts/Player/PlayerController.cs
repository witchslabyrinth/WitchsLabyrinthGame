using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : Singleton<PlayerController>
{   
    /// <summary>
    /// Actor currently controlled by the player
    /// </summary>
    [SerializeField]
    private Actor actor;

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

    private void Start()
    {
        // Make sure we have an actor (default to oliver if not specified)
        if(actor == null) {
            actor = oliver;
        }

        // Throw warnings if oliver/cat not found
        if(!oliver) {
            Debug.LogWarning("Warning: Oliver actor not found in PlayerController");
        }
        if(!cat) {
            Debug.LogWarning("Warning: Cat actor not found in PlayerController");
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1)) {
            // Switch player control to oliver
            actor = oliver;

            // Set cat to idle
            cat.SetMovement(new NullMovement());
            
            // TODO: Restore actor's previous perspective
            // PerspectiveController.Instance.SetPerspective(actor, actor.perspective);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2)) {
            actor = cat;

            // Set oliver to idle
            oliver.SetMovement(new NullMovement());

            // TODO: Restore actor's previous perspective
            // PerspectiveController.Instance.SetPerspective(actor, actor.perspective);
        }

        // Handle interactions with other game entities
        actor.CheckInteraction();
        
        // Update camera perspective
        PerspectiveController.Instance.UpdatePerspective(actor);

        // Update camera to follow player actor
        CameraController.Instance.CameraUpdate(actor);
    }

    /// <summary>
    /// Returns reference to currently-controlled Actor (the player)
    /// </summary>
    /// <returns></returns>
    public Actor GetActor()
    {
        return actor;
    }
}