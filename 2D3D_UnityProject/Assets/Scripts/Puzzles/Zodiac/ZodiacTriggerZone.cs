using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZodiacTriggerZone : MonoBehaviour
{
    [SerializeField]
    private ZodiacPuzzle puzzleScript;

    [SerializeField]
    private GameObject camZodiac;

    private void OnTriggerEnter(Collider other)
    {
        PlayerInteractionController controller = other.GetComponent<PlayerInteractionController>();
        if (controller != null)
            controller.SetInZodiacZone(true, puzzleScript, camZodiac);
        Debug.Log("In range of Zodiac Puzzle");
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerInteractionController controller = other.GetComponent<PlayerInteractionController>();
        if (controller != null)
            controller.SetInZodiacZone(false, puzzleScript, camZodiac);
        Debug.Log("Out of range of Zodiac Puzzle");
    }
}
