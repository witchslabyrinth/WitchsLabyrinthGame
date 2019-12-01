using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleDoorScript : MonoBehaviour
{
    public enum DoorOrientation { FLOOR, CEILING, WALL_NEG_X, WALL_POS_X, WALL_NEG_Z, WALL_POS_Z };     //Used to rotate Room

    [SerializeField]
    private TeleDoorScript PairedDoor;                                                                  //Used to determine which door this connects to
    [SerializeField]
    private Vector3 TeleportOnExitToCoordinate;                                                         //Used to determine where this door lets out

    [SerializeField]
    private DoorOrientation myOrientation = DoorOrientation.FLOOR;                                      //By default, all doors assume they're on the floor

    private List<DoorObserver> observers = new List<DoorObserver>();                                    //Observer pattern used to aid in rotating room

    public void Enter(GameObject toSendThrough) //Called upon colliding with the door
    {
        PairedDoor.Exit(toSendThrough);                                                                 //This door doesn't need to know the exact location to send the player. This can be handled by the exit door (PairedDoor)
    }

    public void Exit(GameObject thatWasSentThrough) //Called by a different, "Enter"-ed door
    {
        thatWasSentThrough.transform.position = new Vector3(TeleportOnExitToCoordinate.x,               //Set the object's position to the value of "TeleportOnExitToCoordinate"
            TeleportOnExitToCoordinate.y,TeleportOnExitToCoordinate.z);
        if (thatWasSentThrough.GetComponent<PlayerController>() != null)                                //If the object sent through was the player, do the following:
        {
            thatWasSentThrough.GetComponent<PlayerController>().ghostCamera                             //Set the player's ghost camera's targetCharacterDirection to the value returned by faceAwayFromDoor()
                .GetComponent<PerspectiveCameraControl>()
                .targetCharacterDirection = faceAwayFromDoor();
        }
        if(observers.Count > 0)                                                                         //If there are observers, do the following:
        {
            for(int i = 0; i < observers.Count; i++)                                                        //For each observer, do the following:
            {
                observers[i].doorObserve(myOrientation);                                                        //Call its "doorObserve" function, passing in myOrientation.
            }
        }
    }

    private void OnTriggerEnter(Collider other) //Called upon colliding with the door
    {
        Enter(other.gameObject);                                                                        //Call the "Enter" function upon colliding with the door.
    }

    public void addDoorObserver(DoorObserver toAdd) //Used for the observer pattern
    {
        observers.Add(toAdd);                                                                           //Add this observer to the list
    }

    private Vector3 faceAwayFromDoor()// used to make the camera face away from the door the player enters
    {
        Vector3 defaultVec = new Vector3(0, 0, 0);                                                      //Set up our default rotation
        if(transform.localPosition.z == -25)                                                            //Change the y-value of the rotation based on where the door is.
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
        return defaultVec;                                                                              //Return the resultant rotation.
    }
}
