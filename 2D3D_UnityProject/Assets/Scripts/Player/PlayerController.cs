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
    }

    public Actor GetActor()
    {
        return actor;
    }
}