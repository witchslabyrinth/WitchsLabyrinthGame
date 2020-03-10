using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlaneScript : MonoBehaviour
{
    [SerializeField]
    private Vector3 defaultRespawnPosition;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
