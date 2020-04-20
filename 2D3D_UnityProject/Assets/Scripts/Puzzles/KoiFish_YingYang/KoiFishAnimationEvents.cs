using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Author: Drew Graham
/// <summary>
/// Contains events called via KoiFish Animator component - don't call any of these methods directly!
/// </summary>

 
[RequireComponent(typeof(Animator))]
public class KoiFishAnimationEvents : MonoBehaviour
{
    /// <summary>
    /// Reference to KoiFish
    /// </summary>
    [SerializeField]
    private KoiFish fish;

    /// <summary>
    /// Turns trail emission off if currently on, or visa versa
    /// </summary>
    void ToggleTrailActive()
    {
        fish.ToggleTrailActive();
    }

    void EnableTrail()
    {
        fish.SetTrailActive(true);
    }

    void DisableTrail()
    {
        fish.SetTrailActive(false);
    }
}
