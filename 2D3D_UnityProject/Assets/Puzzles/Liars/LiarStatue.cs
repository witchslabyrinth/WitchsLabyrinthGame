using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiarStatue : MonoBehaviour
{
    [SerializeField]
    private string statement;

    [SerializeField]
    private string statueName;

    [SerializeField]
    private string subtitle;

    [SerializeField]
    private bool hasOrb;

    [SerializeField]
    private int id;

    void OnTriggerEnter(Collider other)
    {
        PlayerController controller = other.GetComponent<PlayerController>();
        if (controller != null)
            controller.SetInDialogueZone(true, id);
        Debug.Log("Talking to " + statueName);
    }

    void OnTriggerExit(Collider other)
    {
        PlayerController controller = other.GetComponent<PlayerController>();
        if (controller != null)
            controller.SetInDialogueZone(false, id);
        Debug.Log("Leaving conversation with " + statueName);
    }

    public string GetStatement()
    {
        return statement;
    }

    public string GetName()
    {
        return statueName;
    }

    public string GetSubtitle()
    {
        return subtitle;
    }

    public bool GetOrbState()
    {
        return hasOrb;
    }
}
