using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZodiacLight : MonoBehaviour
{
    [SerializeField]
    private List<Light> lights;

    /// <summary>
    /// Enables the light associated with this script
    /// </summary>
    public void TurnOn()
    {
        foreach (Light light in lights)
            light.enabled = true;
    }
}
