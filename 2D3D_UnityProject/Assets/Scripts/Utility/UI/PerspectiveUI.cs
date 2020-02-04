using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PerspectiveUI : MonoBehaviour
{
    /// <summary>
    /// Perspective indicator alpha value when not selected
    /// </summary>
    [SerializeField] 
    [Range(0,1)] private float disabledAlpha;

    private List<PerspectiveIndicator> perspectiveIndicators;

    void Start()
    {
        // Get perspective indicators from children
        perspectiveIndicators = GetComponentsInChildren<PerspectiveIndicator>().ToList();
    }
}
