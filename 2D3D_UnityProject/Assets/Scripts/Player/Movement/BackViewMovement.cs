using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Movement/Back View Movement")]
public class BackViewMovement : Movement
{
    /// <summary>
    /// Returns horizontal movement along X-axis
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    public override Vector3 GetMovement(Transform player)
    {
        Vector3 movement = Vector3.zero;

        // Ignore Up/Down - not used in this movement scheme
        // Left/Right = change in X
        if(Input.GetKey(KeyCode.A)) {
            movement += Vector3.left;
        }
        if(Input.GetKey(KeyCode.D)) {
            movement += Vector3.right;
        }

        return movement.normalized;
    }

    public override Vector2 GetAnimation(Actor player)
    {
        if(Input.GetKey(KeyCode.A)) {
            return Vector2.left;
        }
        if(Input.GetKey(KeyCode.D)) {
            return Vector2.right;
        }
        return Vector2.zero;
    }
}
