using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Movement/Back View Movement")]
public class BackViewMovement : Movement
{
    /// <summary>
    /// Returns horizontal movement along X-axis
    /// </summary>
    /// <param name="actor"></param>
    public override Vector3 GetMovement(Actor actor)
    {
        Vector3 movement = Vector3.zero;

        // Ignore Up/Down - not used in this movement scheme
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

    // TODO: make this work with non-player actors too (i.e. doesn't return player directional input)
    /// <summary>
    /// Returns direction of actor animation
    /// </summary>
    /// <param name="actor">Reference to actor</param>
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