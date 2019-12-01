using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Movement/Perspective Movement")]
public class PerspectiveMovement : Movement
{
    /// <summary>
    /// Reference to camera following player
    /// </summary>
    private Camera perspCamera;
    public PerspectiveMovement(Camera perspCamera)
    {
        this.perspCamera = perspCamera;
    }

    public override Vector3 Get(Transform player)
    {
        Vector3 movement = Vector3.zero;

        // Up/Down = change in Z
        if(Input.GetKey(KeyCode.W)) {
            movement += Vector3.forward;
        }
        if(Input.GetKey(KeyCode.S)) {
            movement += Vector3.back;
        }

        // Left/Right = change in X
        if(Input.GetKey(KeyCode.A)) {
            movement += Vector3.left;
        }
        if(Input.GetKey(KeyCode.D)) {
            movement += Vector3.right;
        }

        // TODO: consider making it use the camera transform rather than player?
        // Make movement relative to player-facing direction
        movement = player.TransformDirection(movement);
        return movement.normalized;
    }
}
