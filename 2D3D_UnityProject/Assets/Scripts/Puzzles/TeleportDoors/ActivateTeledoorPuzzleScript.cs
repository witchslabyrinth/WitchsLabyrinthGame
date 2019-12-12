using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateTeledoorPuzzleScript : MonoBehaviour
{

    //all of these elements will be deactivated when the player collides with this object
    public GameObject[] deactivateOnEnter;

    //all of these elements will be activated when the player collides with this object
    public GameObject[] activateOnEnter;

    //This function runs whenever an object collides with this one
    private void OnTriggerEnter(Collider other)
    {
        //Exit if collided with non-player object
        if (other.GetComponent<PlayerController>() == null)
        {
            return;
        }
        //otherwise continue
        activate();
        deactivate();
    }

    //Activates all objects in activateOnEnter
    private void activate()
    {
        foreach (GameObject i in activateOnEnter)
        {
            i.SetActive(true);
        }
    }

    //Deactivates all objects in deactivateOnEnter
    private void deactivate()
    {
        foreach (GameObject i in deactivateOnEnter)
        {
            i.SetActive(false);
        }
    }
}
