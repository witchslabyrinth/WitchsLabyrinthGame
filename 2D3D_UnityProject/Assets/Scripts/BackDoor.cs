using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackDoor : MonoBehaviour
{
    [SerializeField]
    private ZodiacPuzzle zodiacPuzzle;

    [SerializeField]
    private LiarCommands liarPuzzle;

    [SerializeField]
    private DoubleSlidingDoors backDoors;

    public void OpenBackDoor()
    {
        backDoors.Open();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (zodiacPuzzle.solved && liarPuzzle.solved)
        {
            PlayerInteractionController controller = other.GetComponent<PlayerInteractionController>();
            if (controller != null)
                controller.SetInBackDoorZone(true, this);
            Debug.Log("In range of Back Door");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerInteractionController controller = other.GetComponent<PlayerInteractionController>();
        if (controller != null)
            controller.SetInBackDoorZone(false, this);
        Debug.Log("Out of range of Back Door");
    }
}
