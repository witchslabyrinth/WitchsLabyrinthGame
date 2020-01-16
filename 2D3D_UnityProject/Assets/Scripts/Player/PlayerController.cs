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

    private void Start()
    {
        // Make sure we have an actor
        if(actor == null) {
            Debug.LogError("PlayerController: no Actor specified");
        }
    }

    private void Update()
    {
        // Handle interactions with other game entities
        actor.CheckInteraction();
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