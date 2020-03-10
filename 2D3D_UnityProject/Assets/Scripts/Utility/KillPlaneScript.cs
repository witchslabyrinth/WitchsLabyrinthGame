using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlaneScript : MonoBehaviour
{
    [SerializeField]
    private Vector3 defaultRespawnPosition = Vector3.zero;

    // The kill plane will detect any object that falls into it.
    // If it has a KillPlaneRespawn script attached, then it will be returned to its initial position.
    // Otherwise, the kill plane will send it to a default position set in the editor (or the room's Origin if that wasn't set).
    private void OnTriggerEnter(Collider other)
    {
        KillPlaneRespawn killPlaneRespawn = other.GetComponent<KillPlaneRespawn>();
        if(killPlaneRespawn == null)
        {
            other.transform.position = defaultRespawnPosition;
        }
        else
        {
            killPlaneRespawn.respawn();
        }
    }
}
