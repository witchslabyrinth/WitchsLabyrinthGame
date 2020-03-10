using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlaneRespawn : MonoBehaviour
{
    private Vector3 initialPosition;
    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.position;
    }

    public void respawn()
    {
        transform.position = initialPosition;
    }
}
