using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateTeledoorPuzzleScript : MonoBehaviour
{

    public GameObject[] deactivateOnEnter;//all of these elements will be deactivated when the player collides with this object

    public GameObject[] activateOnEnter;//all of these elements will be activated when the player collides with this object

    private void OnTriggerEnter(Collider other)//Whenever an object collides with this one, run this function.
    {
        if (other.GetComponent<PlayerController>() == null)             //If the object is not the player, do the following:
        {
            return;                                                         //Nothing. We only want to do something if it is the player.
        }
        activate();                                                     //If the above does not happen, run activate()
        deactivate();                                                   //Now run deactivate
    }

    private void activate()//used to activate certain elements when the player collides with this object
    {
        foreach(GameObject i in activateOnEnter)                        //For all gameobjects in this array, do the following:
        {
            i.SetActive(true);                                          //activate them
        }
    }

    private void deactivate()//used to deactivate certain elements when the player collides with this object
    {
        foreach (GameObject i in deactivateOnEnter)                      //For all gameobjects in this array, do the following:
        {
            i.SetActive(false);                                          //deactivate them
        }
    }
}
