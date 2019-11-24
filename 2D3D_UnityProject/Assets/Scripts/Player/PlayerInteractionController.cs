using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: refactor this to be more generalized - each interaction should be processed more-or-less the same way

/// <summary>
/// Used for handling player interactions with game entities (NPCs, Puzzles, etc)
/// </summary>
public class PlayerInteractionController : MonoBehaviour
{
    /// <summary>
    /// Reference to player (note: could switch between Oliver and Cat)
    /// </summary>
    [SerializeField]
    private PlayerController player;

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

    private bool inZodiacZone;

    ///    CAN PROBABLY DISCARD NEXT SECTION IN REFACTOR - END    ///

    // this is probably all bad, but should work for tomorrow's demo
    private ZodiacPuzzle zodiacPuzzle;

    private GameObject zodiacCam;

    public GameObject mainCam;
    // end of bad stuff

    public void CheckInteraction()
    {
        // Checks if player is near interactable and pressing interact button
        if (Input.GetKeyDown(KeyCode.E))
        {
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
                player.ghostCamera.GetComponent<PerspectiveCameraControl>().enabled = false;
                this.enabled = false;
            }
            else if (inZodiacZone)
            {
                // Enable and shift focus to Zodiac puzzle
                zodiacPuzzle.enabled = true;
                zodiacCam.SetActive(true);
                mainCam.SetActive(false);
                this.enabled = false;
            }
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
}
