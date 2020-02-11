using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorObserver : MonoBehaviour
{
    //This code activates the dev cheat to teleport to the final door upon pressing 0.
    void cheat_it_up()
    {
        //Teleport to the given door
        toObserve[doorToExitOnCheat].Exit(player);
        Debug.Log("Dev cheat activated! Warping to exit.");
    }
    //NOTE: This script should be attached to the "DoorList" in the "FullRoom" object of the teledoor puzzle

    //This integer corresponds to a door in the list of doors observed. Since this scans the child's doors in order, it should be the same number as is shown in the hierarchy
    [SerializeField]
    private int doorToExitOnCheat = 0;
    [SerializeField]
    private GameObject player;

    //Observer pattern (close to it) used to aid in rotating room
    private List<TeleDoorScript> toObserve = new List<TeleDoorScript>();
    //This object simply observes all of the doors on its own.

    //Upon loading this object, do the following:
    public void Start()
    {
        //For every child object of this one (all of them should be doors) do the following:
        foreach (Transform child in transform)
        {
            //If it has a TeleDoorScript (it almost definitely does, but let's make sure) do the following:
            if (child.GetComponent<TeleDoorScript>())
            {
                //Add this as one the objects to observe
                toObserve.Add(child.GetComponent<TeleDoorScript>());
            }
        }
        //for every object to observe, do the following:
        for (int i = 0; i < toObserve.Count; i++)
        {
            //add this as one of its observers
            toObserve[i].addDoorObserver(this);
        }
    }

    public void Update()
    {
        //If in editor and 0 is pressed, run the dev cheat
        if (Input.GetKeyDown(KeyCode.Alpha0) && Application.isEditor)
        {
            cheat_it_up();
        }
    }

    //This is called when one of the observed doors exits.
    public void doorObserve(TeleDoorScript.DoorOrientation signalFrom)
    {
        //Set up a new Vector3 for the rotation.
        Vector3 newRotation = new Vector3(0, 0, 0);

        //If the door lets out onto the ceiling, do the following:
        if (signalFrom == TeleDoorScript.DoorOrientation.CEILING)
        {
            //Set the rotation
            newRotation = new Vector3(0, 0, 180);
        }
        //If the door lets out onto the wall in the positive x direction, do the following:
        else if (signalFrom == TeleDoorScript.DoorOrientation.WALL_POS_X)
        {
            //Set the rotation
            newRotation = new Vector3(0, 0, -90);
        }
        //If the door lets out onto the wall in the negative x direction, do the following:
        else if (signalFrom == TeleDoorScript.DoorOrientation.WALL_NEG_X)
        {
            //Set the rotation
            newRotation = new Vector3(0, 0, 90);
        }
        //If the door lets out onto the wall in the positive z direction, do the following:
        else if (signalFrom == TeleDoorScript.DoorOrientation.WALL_POS_Z)
        {
            //Set the rotation
            newRotation = new Vector3(90, 0, 0);
        }
        //If the door lets out onto the wall in the negative z direction, do the following:
        else if (signalFrom == TeleDoorScript.DoorOrientation.WALL_NEG_Z)
        {
            //Set the rotation
            newRotation = new Vector3(-90, 0, 0);
        }
        //else door lets out onto the floor, so newRotation = new Vector3(0, 0, 0);

        //Set the room's rotation to that determined above.
        transform.parent.transform.rotation = Quaternion.Euler(newRotation);
    }
}
