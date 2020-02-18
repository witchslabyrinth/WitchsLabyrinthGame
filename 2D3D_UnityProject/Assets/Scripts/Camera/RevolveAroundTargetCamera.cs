using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class RevolveAroundTargetCamera : CameraEntity
{
    /// <summary>
    /// Location of target to revolve around
    /// </summary>
    [SerializeField]
    private Transform target;

    /// <summary>
    /// Angular speed (in degrees/second) at which to revolve around target
    /// </summary>
    [SerializeField]
    private float rotationSpeed = 10;

    private Vector3 startPosition;

    void Start()
    {
        // Save starting position
        startPosition = transform.position;
    }

    public override void CameraUpdate()
    {
        camera.transform.RotateAround(target.position, Vector3.up, rotationSpeed * Time.deltaTime);
    }

    public override void SetCameraActive(bool active)
    {
        base.SetCameraActive(active);

        // Move back to starting position
        transform.position = startPosition;
    }
}
