using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideViewMovement : Movement
{
    public override Vector3 Get(Transform player)
    {
        Vector3 movement = Vector3.zero;

        // Ignore Up/Down - not used in this movement scheme
        // if(Input.GetKey(KeyCode.W)) {
        //     movement += Vector3.forward;
        // }
        // if(Input.GetKey(KeyCode.S)) {
        //     movement += Vector3.back;
        // }

        // Left/Right = change in X
        if(Input.GetKey(KeyCode.A)) {
            movement += Vector3.back;
        }
        if(Input.GetKey(KeyCode.D)) {
            movement += Vector3.forward;
        }

        return movement.normalized;
    }
}
