using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectHouseExit : MonoBehaviour
{
    [SerializeField]
    private DoubleSlidingDoors doorsToOpen;
    [SerializeField]
    private GameObject HouseEnterTriggerZone;
    [SerializeField]
    private GameObject Oliver;
    [SerializeField]
    private GameObject Cat;

    private void OnTriggerEnter(Collider other)
    {
        //If the collision is with neither of the actors, then ignore the collision. There's no need to open the doors.
        if(other.gameObject != Oliver && other.gameObject != Cat)
        {
            return;
        }
        //Start the doors Opening
        doorsToOpen.Open();
        //Activate a trigger zone that will detect when the player reenters the house.
        HouseEnterTriggerZone.SetActive(true);
        //Deactivate this trigger zone so we don't try to open the already open doors if the player walks here again. We also want it to be inactive so the doors don't open if the zodiac puzzle hasn't been solved.
        gameObject.SetActive(false);
    }
}
