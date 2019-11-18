using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZodiacLight : MonoBehaviour
{
    private Light light;

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
    }
}
