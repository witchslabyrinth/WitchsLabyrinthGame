using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternTriggerZone : MonoBehaviour
{
    [SerializeField]
    private PatternPuzzle puzzleScript;

    [SerializeField]
    private GameObject camPattern;

    [Header("Wwise")]
    /// <summary>
    /// Set Wwise variables
    /// </summary>
    /// <param name="paused">Set Wwise variables for sounds here</param>
    public AK.Wwise.Event playerReaction; //The reaction sound when nearby this puzzle for Oliver 

    private void OnTriggerEnter(Collider other)
    {
        PlayerInteractionController controller = other.GetComponent<PlayerInteractionController>();
        if (controller != null)
            controller.SetInPatternZone(true, puzzleScript, camPattern);
        Debug.Log("In range of Pattern Puzzle");
        playerReaction.Post(gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerInteractionController controller = other.GetComponent<PlayerInteractionController>();
        if (controller != null)
            controller.SetInPatternZone(false, puzzleScript, camPattern);
        Debug.Log("Out of range of Pattern Puzzle");

    }
}
