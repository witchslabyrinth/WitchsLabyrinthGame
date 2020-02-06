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
    /// Reference to nearby KoiFish that can be fed. Null if actor not in a fish-feeding trigger zone
    /// </summary>
    private KoiFish nearbyFish;

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
    // TODO: move this object reference to the LiarGameManager code . instantiate the orb in LiarGameManager and parent it to the Actor, rather than hard-coding it here
    public GameObject orb;

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

    /// <summary>
    /// True if player is in an inspection zone
    /// </summary>
    private bool inInspectZone;

    private GameObject inspectCam;

    void Start()
    {
        if (!TryGetComponent(out actor))
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
            else if (nearbyFish)
            {
                // Feed nearby fish
                nearbyFish.Feed();

                // Hide interact canvas and return without disabling actor
                interactCanvas.SetActive(false);
                return;
            }
            else if (inInspectZone)
            {
                inspectCam.SetActive(true);
                //Cursor.lockState = CursorLockMode.None;
                GameManager.SetCursorActive(true);
            }
            // Ignore interact button press if no nearby interactable
            else
            return;


            // Disable player actor control
            Actor actor = PlayerController.Instance.GetPlayer();
            actor.Disable();

            // Disable actor swapping
            PlayerController.Instance.canSwap = false;

            // Hide interact canvas
            interactCanvas.SetActive(false);
            this.enabled = false;
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

        // TODO: find a way to hide canvas when swapping to actor out of interact zone
        // Show/hide interact canvas
        interactCanvas.SetActive(withinZone);
    }

    public void SetInZodiacZone(bool withinZone, ZodiacPuzzle zodPuz, GameObject zodCam)
    {
        inZodiacZone = withinZone;
        zodiacPuzzle = zodPuz;
        zodiacCam = zodCam;

        // TODO: find a way to hide canvas when swapping to actor out of interact zone
        // Show/hide interact canvas
        interactCanvas.SetActive(withinZone);
    }

    public void SetInKoiFishZone(bool withinZone, KoiFish fish = null)
    {
        // If within fish-feeding zone, set reference to fish
        if (withinZone)
            nearbyFish = fish;
        // Otherwise set fish reference to null
        else
            nearbyFish = null;

        // Show/hide interact canvas
        // TODO: find a way to hide canvas when swapping to actor out of interact zone
        interactCanvas.SetActive(withinZone);
    }

    public void SetInInspectZone(bool withinZone, GameObject camInspect = null)
    {
        inInspectZone = withinZone;
        if (withinZone)
        {
            inspectCam = camInspect;
        }

        interactCanvas.SetActive(withinZone);
    }
}
