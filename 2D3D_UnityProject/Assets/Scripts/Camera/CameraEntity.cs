using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Base class for all camera-controlling scripts
/// </summary>
[RequireComponent(typeof(Camera))]
public abstract class CameraEntity : MonoBehaviour
{
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        // Throw error if camera component not attached
        if(!TryGetComponent(out camera)) {
            Debug.LogError(name + " | missing Camera component");
        }
    }

    protected Camera camera;

    /// <summary>
    /// Updates camera position/orientation
    /// </summary>
    public abstract void CameraUpdate();
}