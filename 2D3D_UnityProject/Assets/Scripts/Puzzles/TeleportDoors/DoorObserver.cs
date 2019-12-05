using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorObserver : MonoBehaviour
{
    //NOTE: This script should be attached to the "DoorList" in the "FullRoom" object of the teledoor puzzle
    private List<TeleDoorScript> toObserve = new List<TeleDoorScript>();    //Observer pattern (close to it) used to aid in rotating room
                                                                            //This object simply observes all of the doors on its own.

    public void Start() //Upon loading this object, do the following:
    {
        foreach(Transform child in transform)                               //For every child object of this one (all of them are doors) do the following:
        {
            if (child.GetComponent<TeleDoorScript>())                           //If it has a TeleDoorScript (it almost definitely does) do the following:
            {
                toObserve.Add(child.GetComponent<TeleDoorScript>());                //Add this as one the objects to observe
            }
        }
        for(int i = 0; i < toObserve.Count; i++)                            //for every object to observe, do the following:
        {
            toObserve[i].addDoorObserver(this);                                 //add this as one of its observers
        }
    }

    public void doorObserve(TeleDoorScript.DoorOrientation signalFrom)  //This is called when one of the observed doors exits.
    {
        Vector3 newRotation = new Vector3(0, 0, 0);                         //Set up a new Vector3 for the rotation.
        if (signalFrom == TeleDoorScript.DoorOrientation.CEILING)           //If the door lets out onto the ceiling, do the following:
        {
            newRotation = new Vector3(0, 0, 180);                               //Set the rotation
        }
        else if (signalFrom == TeleDoorScript.DoorOrientation.WALL_POS_X)   //If the door lets out onto the wall in the positive x direction, do the following:
        {
            newRotation = new Vector3(0, 0, -90);                               //Set the rotation
        }
        else if (signalFrom == TeleDoorScript.DoorOrientation.WALL_NEG_X)   //If the door lets out onto the wall in the negative x direction, do the following:
        {
            newRotation = new Vector3(0, 0, 90);                               //Set the rotation
        }
        else if (signalFrom == TeleDoorScript.DoorOrientation.WALL_POS_Z)   //If the door lets out onto the wall in the positive z direction, do the following:
        {
            newRotation = new Vector3(90, 0, 0);                               //Set the rotation
        }
        else if (signalFrom == TeleDoorScript.DoorOrientation.WALL_NEG_Z)   //If the door lets out onto the wall in the negative z direction, do the following:
        {
            newRotation = new Vector3(-90, 0, 0);                               //Set the rotation
        }
        //else door lets out onto the floor, so newRotation = new Vector3(0, 0, 0);

        transform.parent.transform.rotation = Quaternion.Euler(newRotation);//Set the room's rotation to that determined above.
    }
}
