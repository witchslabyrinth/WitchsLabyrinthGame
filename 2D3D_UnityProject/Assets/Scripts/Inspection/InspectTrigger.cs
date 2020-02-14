using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspectTrigger : MonoBehaviour
{
    [SerializeField]
    private CameraEntity camInspect;

    [SerializeField]
    private CameraFollow cameraFollow;

    private void OnTriggerEnter(Collider other)
    {
        PlayerInteractionController controller = other.GetComponent<PlayerInteractionController>();
        if (controller != null)
            controller.SetInInspectZone(true, cameraFollow, camInspect);
        Debug.Log("In range of Inspection");
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerInteractionController controller = other.GetComponent<PlayerInteractionController>();
        if (controller != null)
            controller.SetInInspectZone(false, cameraFollow, camInspect);
        Debug.Log("Out of range of Inspection");
    }
}
