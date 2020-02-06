using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZodiacLight : MonoBehaviour
{
    [SerializeField]
    private List<Light> lights;

    [Header("Wwise")]
    /// <summary>
    /// Set Wwise variables
    /// </summary>
    /// <param name="paused">Set Wwise variables for sounds here</param>
    public AK.Wwise.Event lightOn;

    /// <summary>
    /// Enables the light associated with this script
    /// </summary>
    public void TurnOn()
    {
        foreach (Light light in lights)
            light.enabled = true;
        
        lightOn.Post(gameObject);
        Debug.Log("Played Lantern sound");
    }
}
