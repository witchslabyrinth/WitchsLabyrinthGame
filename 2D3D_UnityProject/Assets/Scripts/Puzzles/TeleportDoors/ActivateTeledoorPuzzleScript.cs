using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateTeledoorPuzzleScript : MonoBehaviour
{

    public GameObject[] deactivateOnEnter;

    public GameObject[] activateOnEnter;

    private void OnTriggerEnter(Collider other)
    {
        activate();
        deactivate();
    }

    private void activate()
    {
        foreach(GameObject i in activateOnEnter)
        {
            i.SetActive(true);
        }
    }

    private void deactivate()
    {
        foreach (GameObject i in deactivateOnEnter)
        {
            i.SetActive(false);
        }
    }
}
