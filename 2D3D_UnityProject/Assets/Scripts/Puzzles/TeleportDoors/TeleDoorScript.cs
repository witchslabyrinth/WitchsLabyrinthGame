using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleDoorScript : MonoBehaviour
{
    //These are used to handle Camera rotation upon exiting a door.
    private int yBehind = 180;
    private int yLeft = -90;
    private int yRight = 90;

    //This corrects the position of where to send the player.
    private int rotationCorrection = 90;

    //These handle positioning upon exiting the door.
    private float UnitsInFront = 1.75f;
    private float UnitsBelow = 1.5f;//Door is Taller than Oliver, so to place him on the floor we need to subtract this from the exit door's y position.


    //Used to rotate Room
    public enum DoorOrientation { FLOOR, CEILING, WALL_NEG_X, WALL_POS_X, WALL_NEG_Z, WALL_POS_Z };

    //Used to determine which door this connects to
    [SerializeField]
    private TeleDoorScript PairedDoor;

    //Used to determine where this door lets out
    /*[SerializeField]
    private Vector3 TeleportOnExitToCoordinate;*/

    //By default, all doors assume they're on the floor
    [SerializeField]
    private DoorOrientation myOrientation = DoorOrientation.FLOOR;

    //Observer pattern used to aid in rotating room
    private List<DoorObserver> observers = new List<DoorObserver>();

    //Used to rotateCamera;
    private float myYDir;

    private void Start()
    {
        myYDir = transform.rotation.eulerAngles.y;
    }

    //Called upon colliding with the door
    public void Enter(GameObject toSendThrough)
    {
        //This door doesn't need to know the exact location to send the player. This can be handled by the exit door (PairedDoor)
        Debug.Log(transform.name + "Enter");
        PairedDoor.Exit(toSendThrough);
    }

    //Called by a different, "Enter"-ed door to teleport the player to this one
    public void Exit(GameObject thatWasSentThrough)
    {
        //Set the object's position based on orientation of this door, approx. 1.75 units in front of door's position
        Vector3 newPos = Quaternion.AngleAxis(transform.rotation.eulerAngles.y, transform.up) * (transform.forward * -UnitsInFront);
        //newPos.y -= UnitsBelow;
        thatWasSentThrough.transform.Translate(newPos);
        Debug.Log(transform.name + "Exit");
        /*if (myYDir == yBehind)//if front is facing negative z (the front is the only side you can enter or exit)
        {
            //thatWasSentThrough.transform.Translate()
            thatWasSentThrough.transform.position = new Vector3(transform.position.x, transform.position.y - UnitsBelow, transform.position.z - UnitsInFront);
        }
        else if(myYDir == yLeft)//otherwise, if front is facing negative x
        {
            thatWasSentThrough.transform.position = new Vector3(transform.position.x + UnitsInFront, transform.position.y - UnitsBelow, transform.position.z);
        }
        else if (myYDir == yRight)//otherwise, if front is facing positive x
        {
            thatWasSentThrough.transform.position = new Vector3(transform.position.x - UnitsInFront, transform.position.y - UnitsBelow, transform.position.z);
        }
        else //otherwise, (front is facing positive z)
        {
            thatWasSentThrough.transform.position = new Vector3(transform.position.x, transform.position.y - UnitsBelow, transform.position.z + UnitsInFront);
        }*/

        // Check if player was sent through
        // TODO: verify this still works properly - i changed it to use the Actor componentn instead of PlayerController
        Actor player = thatWasSentThrough.GetComponent<Actor>();
        if (player != null)
        {
            //Set the player's ghost camera's targetCharacterDirection to the value returned by faceAwayFromDoor()
            PerspectiveCameraControl cameraControl = player.ghostCamera;
            if(cameraControl != null)
            {
                cameraControl.targetCharacterDirection = faceAwayFromDoor();
            }
            else
            {
                Debug.LogError("ERROR! Player's ghostCamera has no PerspectiveCameraControl!");
            }
        }
        else
        {
            Debug.Log("Non-player Collision detected");
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
        Vector3 defaultVec = Vector3.zero;

        //Change the y-value of the rotation based on where the door is.
        if (myYDir == yBehind)
        {
            defaultVec.y = yBehind;
        }
        else if (myYDir == yLeft)
        {
            defaultVec.y = yLeft;
        }
        else if (myYDir == yRight)
        {
            defaultVec.y = yRight;
        }
        /*else
        {
            defaultVec.y = yInFront;
        }*/

        //Return the resultant rotation.
        return defaultVec;
    }
}
