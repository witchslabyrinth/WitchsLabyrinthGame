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

    void Start()
    {
        // Make sure we have an actor
        if(actor == null) {
            Debug.LogError("PlayerController: no Actor specified");
        }
    }

    void Update()
    {
        // Handle interactions with other game entities
        actor.CheckInteraction();

        // @ Victor:
        // TODO: use movement data and send it to the AnimationController to show the proper animations
        // Try and keep your code limited to this class and AnimationController.cs 
        // Don't worry about making the Cat's animations work for now, just focus on getting Oliver working.
        // I'm gonna rework the Cat a bit to make him and the player very similar (I'm thinking both could be instances of an Actor object)
    }

    public Actor GetActor()
    {
        return actor;
    }
}