using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using Yarn.Unity.Example;

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
    private NPC dialoguePartner;

    //private GameObject dialogueCam;

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

    private ZodiacPuzzle zodiacPuzzle;

    /// <summary>
    /// Shown when actor is near an interactable
    /// </summary>
    public GameObject interactCanvas;

    /// <summary>
    /// True if player is in an inspection zone
    /// </summary>
    private bool inInspectZone;

    /// <summary>
    /// Inspection behavior component
    /// </summary>
    private CameraFollow inspectCameraFollow;

    /// <summary>
    /// True if player is in the pattern puzzle zone
    /// </summary>
    private bool inPatternZone;

    private PatternPuzzle patternPuzzle;

    /// <summary>
    /// Holds CameraEntity of nearby interactable (if it has one)
    /// </summary>
    private CameraEntity interactionCamera;

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
                FindObjectOfType<DialogueRunner>().StartDialogue(dialoguePartner.talkToNode);
                CameraController.Instance.SetMainCamera(interactionCamera);

                GameManager.SetCursorActive(true);
            }
            else if (inZodiacZone && !zodiacPuzzle.solved)
            {
                // Enable and shift focus to Zodiac puzzle
                zodiacPuzzle.enabled = true;
                CameraController.Instance.SetMainCamera(interactionCamera);

            }
            else if (inPatternZone)
            {
                // Enable and shift focus to Pattern puzzle
                patternPuzzle.enabled = true;
                CameraController.Instance.SetMainCamera(interactionCamera);

            }
            else if (nearbyFish)
            {
                // Feed nearby fish
                KoiFishPuzzle.Instance.FeedFish(nearbyFish);

                // Hide interact canvas and return without disabling actor
                interactCanvas.SetActive(false);
                return;
            }
            else if (inInspectZone)
            {
                // Enable and shift focus to inspection
                CameraController.Instance.SetMainCamera(interactionCamera);
                inspectCameraFollow.enabled = true;

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
    /// <param name="partner">NPC player is currently in zone of</param>
    public void SetInDialogueZone(bool withinZone, NPC partner, CameraEntity diaCam)
    {
        inDialogueZone = withinZone;
        dialoguePartner = partner;

        // Store reference to dialogue camera (or set null if out of zone)
        interactionCamera = withinZone ? diaCam : null;

        interactCanvas.SetActive(withinZone);
    }

    public void SetInZodiacZone(bool withinZone, ZodiacPuzzle zodPuz, CameraEntity zodCam)
    {
        inZodiacZone = withinZone;
        zodiacPuzzle = zodPuz;

        // Store reference to zodiac camera (or set null if out of zone)
        interactionCamera = withinZone ? zodCam : null;

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
        interactCanvas.SetActive(withinZone);
    }

    public void SetInInspectZone(bool withinZone, CameraFollow cameraFollow, CameraEntity camInspect)
    {
        inInspectZone = withinZone;

        // Store reference to camera if entering zone, or set null if exiting
        interactionCamera = withinZone ? camInspect : null;

        inspectCameraFollow = cameraFollow;

        // Show/hide interact canvas
        interactCanvas.SetActive(withinZone);
    }

    public void SetInPatternZone(bool withinZone, PatternPuzzle patPuz, CameraEntity cameraEntity)
    {
        inPatternZone = withinZone;
        patternPuzzle = patPuz;

        // Store reference to camera if entering zone, or set null if exiting
        interactionCamera = withinZone ? cameraEntity : null;

        // Show/hide interact canvas
        interactCanvas.SetActive(withinZone);
    }
}
