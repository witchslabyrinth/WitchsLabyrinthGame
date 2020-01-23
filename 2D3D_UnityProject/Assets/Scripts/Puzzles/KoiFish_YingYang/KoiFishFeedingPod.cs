using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class KoiFishFeedingPod : MonoBehaviour
{
    /// <summary>
    /// Koi Fish associated with this feeding pod
    /// </summary>
    [SerializeField] private KoiFish koiFish;

    // Start is called before the first frame update
    void Start()
    {
        // Check that we have a collider attached
        if (!TryGetComponent(out Collider collider))
            Debug.LogErrorFormat("{0} | Error - missing Collider component. Please attach a Collider to this Feeding Pod", name);

        if(!koiFish)
            Debug.LogErrorFormat("{0} | Error - No KoiFish associated with this Feeding Pod", name);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerInteractionController playerInteractionController))
            playerInteractionController.SetInKoiFishZone(true, koiFish);

        Debug.Log(other.name + " in range of " + koiFish.name);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PlayerInteractionController playerInteractionController))
            playerInteractionController.SetInKoiFishZone(true, koiFish);

        Debug.Log(other.name + " out of range of " + koiFish.name);
    }
}
