using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlaneRespawn : MonoBehaviour
{
    private Vector3 initialPosition;
    // When the scene begins, the object will store its initial position in memory
    void Start()
    {
        initialPosition = transform.position;
    }
    // Should we need to respawn the object, we will set its position to that we stored earlier
    public void respawn()
    {
        transform.position = initialPosition;
    }
}
