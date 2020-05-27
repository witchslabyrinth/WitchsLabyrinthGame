using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleDoorScript : MonoBehaviour
{
    //These handle positioning upon exiting the door.
    private float UnitsInFront = 1.75f;//If Oliver is teleported to the door's position, he will collide with it and be teleported again. This distance in front of the door will avoid him immediately colliding with it and will give the illusion that he exited through it.
    private float UnitsBelow = 1.5f;//Door is Taller than Oliver, so to place him on the floor we need to subtract this from the exit door's y position.

    //Used to determine which door this connects to
    [SerializeField]
    private TeleDoorScript PairedDoor;

    //Observer pattern used to aid in rotating room.
    //Set to be removed in future if DoorObserver is unneeded.
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
        //Set the object's position based on orientation of this door, approx. 1.75 units in front of door's positive z face.
        Vector3 adjustPos = transform.rotation * Vector3.forward * UnitsInFront;
        adjustPos.y -= UnitsBelow;
        thatWasSentThrough.transform.position = new Vector3(transform.position.x + adjustPos.x, transform.position.y + adjustPos.y, transform.position.z + adjustPos.z);

        // Check if player was sent through
        // TODO: verify this still works properly - i changed it to use the Actor componentn instead of PlayerController
        Actor player = thatWasSentThrough.GetComponent<Actor>();
        if (player != null)
        {
            //Set the player's ghost camera's targetCharacterDirection to the value returned by faceAwayFromDoor()
            PerspectiveCameraControl cameraControl = player.ghostCamera;
            if(cameraControl != null)
            {
                //cameraControl.targetCharacterDirection = FaceAwayFromDoor();
                int angleCorrection = 0;
                if (Input.GetKey(KeyCode.S))
                {
                    angleCorrection = 180;
                }
                else if (Input.GetKey(KeyCode.A))
                {
                    angleCorrection = 90;
                }
                else if(Input.GetKey(KeyCode.S))
                {
                    angleCorrection = -90;
                }
                cameraControl.SetCharacterDirection(Quaternion.AngleAxis(angleCorrection,Vector3.up) * FaceAwayFromDoor());
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
    }

    //This function is called when any object collides with the door
    private void OnTriggerEnter(Collider other)
    {
        //Call the "Enter" function upon colliding with the door.
        Enter(other.gameObject);
    }

    // used to make the camera face away from the door the player enters
    private Vector3 FaceAwayFromDoor()
    {
        //This vector faces away from the door.
        return transform.rotation * Vector3.forward;
    }
}
