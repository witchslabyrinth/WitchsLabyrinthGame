using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleDoorScript : MonoBehaviour
{
    //Used to rotate Room
    public enum DoorOrientation { FLOOR, CEILING, WALL_NEG_X, WALL_POS_X, WALL_NEG_Z, WALL_POS_Z };

    //Used to determine which door this connects to
    [SerializeField]
    private TeleDoorScript PairedDoor;

    //Used to determine where this door lets out
    [SerializeField]
    private Vector3 TeleportOnExitToCoordinate;

    //By default, all doors assume they're on the floor
    [SerializeField]
    private DoorOrientation myOrientation = DoorOrientation.FLOOR;

    //Observer pattern used to aid in rotating room
    private List<DoorObserver> observers = new List<DoorObserver>();

    //Called upon colliding with the door
    public void Enter(GameObject toSendThrough)
    {
        //This door doesn't need to know the exact location to send the player. This can be handled by the exit door (PairedDoor)
        PairedDoor.Exit(toSendThrough);
    }

    //Called by a different, "Enter"-ed door to teleport the player to this one
    public void Exit(GameObject thatWasSentThrough)
    {
        //Set the object's position to the value of "TeleportOnExitToCoordinate"
        thatWasSentThrough.transform.position = new Vector3(TeleportOnExitToCoordinate.x,TeleportOnExitToCoordinate.y,TeleportOnExitToCoordinate.z);

        //If the object sent through was the player, do the following:
        if (thatWasSentThrough.GetComponent<PlayerController>() != null)
        {
            //Set the player's ghost camera's targetCharacterDirection to the value returned by faceAwayFromDoor()
            thatWasSentThrough.GetComponent<PlayerController>().ghostCamera.GetComponent<PerspectiveCameraControl>().targetCharacterDirection = faceAwayFromDoor();
        }

        //If there are observers, do the following:
        if (observers.Count > 0)
        {
            //For each observer, do the following:
            for (int i = 0; i < observers.Count; i++)
            {
                //Call its "doorObserve" function, passing in myOrientation.
                observers[i].doorObserve(myOrientation);
            }
        }
    }

    //This function is called when any object collides with the door
    private void OnTriggerEnter(Collider other)
    {
        //Call the "Enter" function upon colliding with the door.
        Enter(other.gameObject);
    }

    //Used for the observer pattern
    public void addDoorObserver(DoorObserver toAdd)
    {
        //Add this observer to the list
        observers.Add(toAdd);
    }

    // used to make the camera face away from the door the player enters
    private Vector3 faceAwayFromDoor()
    {
        //Set up our default rotation
        Vector3 defaultVec = new Vector3(0, 0, 0);

        //Change the y-value of the rotation based on where the door is.
        if (transform.localPosition.z == -25)
        {
            defaultVec.y = 180;
        }
        else if (transform.localPosition.x == -25)
        {
            defaultVec.y = -90;
        }
        else if (transform.localPosition.x == 25)
        {
            defaultVec.y = 90;
        }

        //Return the resultant rotation.
        return defaultVec;
    }
}
