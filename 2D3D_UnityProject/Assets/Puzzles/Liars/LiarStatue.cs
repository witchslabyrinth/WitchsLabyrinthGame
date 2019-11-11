using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiarStatue : MonoBehaviour
{
    [SerializeField]
    private string statement;

    [SerializeField]
    private bool hasOrb;

    private bool inZone;

    // Start is called before the first frame update
    void Start()
    {
        inZone = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            //talk to this statue
        }
        if (Input.GetKeyDown("enter"))
        {
            //try to take orb from statue
        }
    }

    void OnTriggerEnter()
    {
        inZone = true;
    }

    void OnTriggerExit(Collider other)
    {
        inZone = false;
    }
}
