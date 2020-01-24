using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Movement/Side View Movement")]
public class SideViewMovement : Movement
{
    /// <summary>
    /// Returns horizontal movement along Z-axis
    /// </summary>
    /// <param name="actor"></param>
    public override Vector3 GetMovement(Actor actor)
    {
        Vector3 movement = Vector3.zero;

        // Ignore Up/Down - not used in this movement scheme
        // Left/Right = change in Z
        if (Input.GetKey(KeyCode.A))
        {
            movement += Vector3.back;
        }

        if (Input.GetKey(KeyCode.D))
        {
            movement += Vector3.forward;
        }

        return movement.normalized;
    }

    public override Vector2 GetAnimation(Actor actor)
    {
        if (Input.GetKey(KeyCode.A))
        {
            return Vector2.left;
        }

        if (Input.GetKey(KeyCode.D))
        {
            return Vector2.right;
        }

        return Vector2.zero;
    }
}