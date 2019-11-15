using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorObserver : MonoBehaviour
{
    private List<TeleDoorScript> toObserve = new List<TeleDoorScript>();
    

    public void Start()
    {
        foreach(Transform child in transform)
        {
            if (child.GetComponent<TeleDoorScript>())
            {
                toObserve.Add(child.GetComponent<TeleDoorScript>());
            }
        }
        for(int i = 0; i < toObserve.Count; i++)
        {
            toObserve[i].addDoorObserver(this);
        }
    }

    public void doorObserve(TeleDoorScript.DoorOrientation signalFrom)
    {
        /*Vector3 signalFromRotation = signalFrom.localRotation.eulerAngles;
        Vector3 newRotation = new Vector3(0,0,0);
        if(signalFromRotation.x == 90)
        {
            if(signalFromRotation.y == 90)
            {
                newRotation = new Vector3(0, 0, 90);
            }
        }*/
        Vector3 newRotation = new Vector3(0, 0, 0);
        /*if(signalFrom.localPosition.y == 22.5)
        {
            newRotation = new Vector3(0, 0, 180);
        }
        else if (signalFrom.localPosition.x == 22.5)
        {
            newRotation = new Vector3(0, 0, -90);
        }
        else if (signalFrom.localPosition.x == -22.5)
        {
            newRotation = new Vector3(0, 0, 90);
        }
        else if(signalFrom.localPosition.z == 22.5)
        {
            newRotation = new Vector3(90, 0, 0);
        }
        else if (signalFrom.localPosition.z == -22.5)
        {
            newRotation = new Vector3(-90, 0, 0);
        }*/
        if (signalFrom == TeleDoorScript.DoorOrientation.CEILING)
        {
            newRotation = new Vector3(0, 0, 180);
        }
        else if (signalFrom == TeleDoorScript.DoorOrientation.WALL_POS_X)
        {
            newRotation = new Vector3(0, 0, -90);
        }
        else if (signalFrom == TeleDoorScript.DoorOrientation.WALL_NEG_X)
        {
            newRotation = new Vector3(0, 0, 90);
        }
        else if (signalFrom == TeleDoorScript.DoorOrientation.WALL_POS_Z)
        {
            newRotation = new Vector3(90, 0, 0);
        }
        else if (signalFrom == TeleDoorScript.DoorOrientation.WALL_NEG_Z)
        {
            newRotation = new Vector3(-90, 0, 0);
        }
        //else DoorOrientation is FLOOR, so newRotation = new Vector3(0, 0, 0);

        transform.parent.transform.rotation = Quaternion.Euler(newRotation);
    }
}
