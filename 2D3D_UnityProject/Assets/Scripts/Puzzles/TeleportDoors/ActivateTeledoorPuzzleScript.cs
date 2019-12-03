using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateTeledoorPuzzleScript : MonoBehaviour
{

    //all of these elements will be deactivated when the player collides with this object
    public GameObject[] deactivateOnEnter;

    //all of these elements will be activated when the player collides with this object
    public GameObject[] activateOnEnter;

    //Whenever an object collides with this one, run this function.
    private void OnTriggerEnter(Collider other)
    {
        //If the object is not the player, do the following:
        if (other.GetComponent<PlayerController>() == null)
        {
            //Nothing. We only want to do something if it is the player.
            return;
        }
        //If the above does not happen, run activate()
        activate();
        //Now run deactivate()
        deactivate();
    }

    //used to activate certain elements when the player collides with this object
    private void activate()
    {
        //For all gameobjects in this array, do the following:
        foreach (GameObject i in activateOnEnter)
        {
            //activate them
            i.SetActive(true);
        }
    }

    //used to deactivate certain elements when the player collides with this object
    private void deactivate()
    {
        //For all gameobjects in this array, do the following:
        foreach (GameObject i in deactivateOnEnter)
        {
            //deactivate them
            i.SetActive(false);
        }
    }
}
