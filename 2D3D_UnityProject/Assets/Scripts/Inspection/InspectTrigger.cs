using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspectTrigger : MonoBehaviour
{
    [SerializeField]
    private GameObject camInspect;

    private void OnTriggerEnter(Collider other)
    {
        PlayerInteractionController controller = other.GetComponent<PlayerInteractionController>();
        if (controller != null)
            controller.SetInInspectZone(true, camInspect);
        Debug.Log("In range of Inspection");
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerInteractionController controller = other.GetComponent<PlayerInteractionController>();
        if (controller != null)
            controller.SetInInspectZone(false, camInspect);
        Debug.Log("Out of range of Inspection");
    }
}
