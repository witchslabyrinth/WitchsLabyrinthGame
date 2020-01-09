using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: refactor this to be more generalized - each interaction should be processed more-or-less the same way

/// <summary>
/// Used for handling player interactions with game entities (NPCs, Puzzles, etc)
/// </summary>
[RequireComponent((typeof(Actor)))]
public class PlayerInteractionController : MonoBehaviour
{
    /// <summary>
    /// Reference to associated actor (either Oliver or Cat)
    /// </summary>
    private Actor actor;

    ///    CAN PROBABLY DISCARD NEXT SECTION IN REFACTOR    ///

    /// <summary>
    /// is the player within talking distance of an npc
    /// </summary>
    private bool inDialogueZone;

    /// <summary>
    /// the id of the npc in range of the player
    /// </summary>
    private int dialoguePartner;

    /// <summary>
    /// reference to the orb the player is trying to find
    /// </summary>
    public GameObject orb;

    /// <summary>
    /// Reference to an interactable within range (if present)
    /// </summary>
    private Interactable interactable;

    /// <summary>
    /// True if player is near the Zodiac puzzle
    /// </summary>
    private bool inZodiacZone;

    ///    CAN PROBABLY DISCARD NEXT SECTION IN REFACTOR - END    ///

    // this is probably all bad, but should work for tomorrow's demo
    private ZodiacPuzzle zodiacPuzzle;

    private GameObject zodiacCam;
    // end of bad stuff

    public GameObject interactCanvas;

    void Start()
    {
        if(!TryGetComponent(out actor)) 
        {
            Debug.LogWarning(name + " | PlayerInteractionController failed to get Actor component from this game object");
        }
    }

    /// <summary>
    /// Called via PlayerController to handle interactions
    /// </summary>
    public void CheckInteraction()
    {
        // Checks if player is near interactable and pressing interact button
        if (Input.GetKeyDown(KeyCode.E))
        {
            // @Victor - Delete this code if no longer needed
            // if (inDialogueZone)
            // {
            //     if (dialoguePartner == 0)
            //         orb.SetActive(true);
            //     LiarGameManager.Instance().CheckOrb(dialoguePartner);
            // }

            if (inDialogueZone)
            {
                // Show dialogue conversation if interacting with NPC
                LiarGameManager.Instance().StartConversation(dialoguePartner);
            }
            else if (inZodiacZone)
            {
                // Enable and shift focus to Zodiac puzzle
                zodiacPuzzle.enabled = true;
                zodiacCam.SetActive(true);
            }

            // Disable player actor control
            Actor actor = PlayerController.Instance.GetActor();
            actor.Disable();

            // Hide interact canvas
            interactCanvas.SetActive(false);
            this.enabled = false;
        }
        else if(Input.GetKeyDown(KeyCode.R)) 
        {

        }
    }

    /// <summary>
    /// called by OnTriggerEnter and OnTriggerExit of npc zones. Determines whether the player can talk or not
    /// </summary>
    /// <param name="withinZone">true on enter, false on exit</param>
    /// <param name="partner">ID of npc</param>
    public void SetInDialogueZone(bool withinZone, int partner)
    {
        inDialogueZone = withinZone;
        dialoguePartner = partner;
    }

    public void SetInZodiacZone(bool withinZone, ZodiacPuzzle zodPuz, GameObject zodCam)
    {
        inZodiacZone = withinZone;
        zodiacPuzzle = zodPuz;
        zodiacCam = zodCam;
    }

    /// <summary>
    /// Called when this actor enters/exits the trigger zone of an Interactable
    /// </summary>
    /// <param name="nearInteractable">True if actor entered trigger zone, false if actor exited trigger zone</param>
    /// <param name="interactable">Nearby interactable</param>
    public void SetNearbyInteractable(bool nearInteractable, Interactable interactable) 
    {
        if(nearInteractable) 
            this.interactable = interactable;
        else 
            this.interactable = null;
    }

    void Update()
    {
        // Don't try to interact if there's no nearby interactable
        if(interactable == null) {
            return;
        }

        if (inZodiacZone || inDialogueZone)
        {
            interactCanvas.SetActive(true);
        }
        else
        {
            interactCanvas.SetActive(false);
        }
    }
}
