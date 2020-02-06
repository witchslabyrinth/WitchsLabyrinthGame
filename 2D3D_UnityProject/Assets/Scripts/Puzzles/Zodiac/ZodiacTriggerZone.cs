using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZodiacTriggerZone : MonoBehaviour
{
    [SerializeField]
    private ZodiacPuzzle puzzleScript;

    [SerializeField]
    private GameObject camZodiac;

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
            controller.SetInZodiacZone(true, puzzleScript, camZodiac);
        Debug.Log("In range of Zodiac Puzzle");
        playerReaction.Post(gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerInteractionController controller = other.GetComponent<PlayerInteractionController>();
        if (controller != null)
            controller.SetInZodiacZone(false, puzzleScript, camZodiac);
        Debug.Log("Out of range of Zodiac Puzzle");

    }
}
