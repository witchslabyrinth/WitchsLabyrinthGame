using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Movement/Top Down Movement")]
public class TopDownMovement : Movement
{
    /// <summary>
    /// Returns horizontal/vertical movement along X and Z axes (respectively)
    /// </summary>
    /// <param name="actor"></param>
    /// <returns></returns>
    public override Vector3 GetMovement(Actor actor)
    {
        Vector3 movement = Vector3.zero;

        // Up/Down = change in Z
        if (Input.GetKey(KeyCode.W))
        {
            movement += Vector3.forward;
        }

        if (Input.GetKey(KeyCode.S))
        {
            movement += Vector3.back;
        }

        // Left/Right = change in X
        if (Input.GetKey(KeyCode.A))
        {
            movement += Vector3.left;
        }

        if (Input.GetKey(KeyCode.D))
        {
            movement += Vector3.right;
        }

        return movement.normalized;
    }

    public override Vector2 GetAnimation(Actor actor)
    {
        // TODO: return proper animation values
        return Vector2.zero;
    }
}