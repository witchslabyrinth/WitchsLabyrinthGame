using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Movement/Side View Movement")]
public class SideViewMovement : Movement
{
    /// <summary>
    /// Returns horizontal movement along Z-axis
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    public override Vector3 Get(Transform player)
    {
        Vector3 movement = Vector3.zero;

        // Ignore Up/Down - not used in this movement scheme
        // Left/Right = change in Z
        if(Input.GetKey(KeyCode.A)) {
            movement += Vector3.back;
        }
        if(Input.GetKey(KeyCode.D)) {
            movement += Vector3.forward;
        }

        return movement.normalized;
    }
}
