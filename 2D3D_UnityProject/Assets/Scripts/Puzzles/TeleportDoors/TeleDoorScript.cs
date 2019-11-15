using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleDoorScript : MonoBehaviour
{
    public enum DoorOrientation { FLOOR, CEILING, WALL_NEG_X, WALL_POS_X, WALL_NEG_Z, WALL_POS_Z };

    [SerializeField]
    private TeleDoorScript PairedDoor;
    [SerializeField]
    private Vector3 TeleportOnExitToCoordinate;

    [SerializeField]
    private DoorOrientation myOrientation = DoorOrientation.FLOOR;

    private List<DoorObserver> observers = new List<DoorObserver>();

    public void Enter(GameObject toSendThrough)
    {
        PairedDoor.Exit(toSendThrough);
    }

    public void Exit(GameObject thatWasSentThrough)
    {
        thatWasSentThrough.transform.position = new Vector3(TeleportOnExitToCoordinate.x,TeleportOnExitToCoordinate.y,TeleportOnExitToCoordinate.z);
        if(observers.Count > 0)
        {
            for(int i = 0; i < observers.Count; i++)
            {
                observers[i].doorObserve(myOrientation);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Enter(other.gameObject);
    }

    public void addDoorObserver(DoorObserver toAdd)
    {
        observers.Add(toAdd);
    }
}
