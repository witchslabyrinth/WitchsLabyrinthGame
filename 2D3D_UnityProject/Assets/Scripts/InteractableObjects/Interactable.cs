using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class Interactable : MonoBehaviour
{
    /// <summary>
    /// Zone within which player can interact with this
    /// </summary>
    private Collider triggerZone;

    private void Start()
    {
        triggerZone = GetComponent<Collider>();
    }

    /// <summary>
    /// Switches control from player-controlled actor to this
    /// </summary>
    public abstract void StartInteracting();

    /// <summary>
    /// Switches control from this back to player-controlled actor
    /// </summary>
    public abstract void StopInteracting();
    

    private void OnTriggerEnter(Collider other)
    {
        // PlayerInteractionController controller = other.GetComponent<PlayerInteractionController>();
        // if (controller != null)
        //     controller.SetInZodiacZone(true, puzzleScript, camZodiac);

        // If player-controlled actor is in range
        if(other.TryGetComponent<Actor>(out Actor actor) && actor == PlayerController.Instance.GetActor()) {
            // tell actor he's near an interactable
            PlayerInteractionController interactionController = actor.interactionController;
            
            Debug.LogFormat("{0} Entered range of {1}", actor.name, name);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // PlayerInteractionController controller = other.GetComponent<PlayerInteractionController>();
        // if (controller != null)
        //     controller.SetInZodiacZone(false, puzzleScript, camZodiac);

        // If player-controlled actor is in range
        if(other.TryGetComponent<Actor>(out Actor actor) && actor == PlayerController.Instance.GetActor()) {
            // tell actor he's near an interactable
            PlayerInteractionController interactionController = actor.interactionController;
            
            Debug.LogFormat("{0} Exited range of {1}", actor.name, name);
        }
    }
}