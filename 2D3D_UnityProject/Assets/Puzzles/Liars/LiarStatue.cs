using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///    ENTIRE SCRIPT MARKED FOR DELETION IN REFACTOR    ///

public class LiarStatue : MonoBehaviour
{
    /// <summary>
    /// general statement this statue will say
    /// </summary>
    [SerializeField]
    private string statement;

    /// <summary>
    /// what the statue will say when player looks for orb
    /// </summary>
    [SerializeField]
    private string winLoseStatement;

    /// <summary>
    /// name of the statue
    /// </summary>
    [SerializeField]
    private string statueName;

    /// <summary>
    /// subtitle of the statue
    /// </summary>
    [SerializeField]
    private string subtitle;

    /// <summary>
    /// whether or not this statue has the orb
    /// </summary>
    [SerializeField]
    private bool hasOrb;

    /// <summary>
    /// ID of this statue, used as index in manager list
    /// </summary>
    [SerializeField]
    private int id;

    [SerializeField]
    private GameObject liarCam;

    void OnTriggerEnter(Collider other)
    {
        PlayerInteractionController controller = other.GetComponent<PlayerInteractionController>();
        if (controller != null)
            controller.SetInDialogueZone(true, id);
        Debug.Log("Talking to " + statueName);
    }

    void OnTriggerExit(Collider other)
    {
        PlayerInteractionController controller = other.GetComponent<PlayerInteractionController>();
        if (controller != null)
            controller.SetInDialogueZone(false, id);
        Debug.Log("Leaving conversation with " + statueName);
    }

    public string GetStatement()
    {
        return statement;
    }

    public string GetWinLoseStatement()
    {
        return winLoseStatement;
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

    public GameObject GetCamera()
    {
        return liarCam;
    }
}
