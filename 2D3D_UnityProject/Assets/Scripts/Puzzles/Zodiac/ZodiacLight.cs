using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZodiacLight : MonoBehaviour
{
    private Light light;
    [Header("Wwise")]
    /// <summary>
    /// Set Wwise variables
    /// </summary>
    /// <param name="paused">Set Wwise variables for sounds here</param>
    public AK.Wwise.Event lightOn;

    private void Start()
    {
        light = GetComponent<Light>();
    }

    /// <summary>
    /// Enables the light associated with this script
    /// </summary>
    public void TurnOn()
    {
        light.enabled = true;
        lightOn.Post(gameObject);
        Debug.Log("Played Lantern sound");




    }
}
