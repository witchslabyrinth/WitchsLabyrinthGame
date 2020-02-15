using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Base class for all camera-controlling scripts.
/// ONLY ONE should be enabled at any given time
/// </summary>
[RequireComponent(typeof(Camera))]
public abstract class CameraEntity : MonoBehaviour
{
    public Camera camera { get; protected set; }

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        // Throw error if camera component not attached
        camera = GetComponent<Camera>();
        if(!camera) {
            Debug.LogError(name + " | missing Camera component");
        }
    }

    /// <summary>
    /// Updates camera position/orientation (only called on currently used camera)
    /// </summary>
    public abstract void CameraUpdate();

    /// <summary>
    /// Enables/disables Camera component (call this to turn cameras on/off)
    /// </summary>
    /// <param name="active">True to turn camera on, false to turn it off</param>
    public void SetCameraActive(bool active)
    {
        camera.enabled = active;
    }
}