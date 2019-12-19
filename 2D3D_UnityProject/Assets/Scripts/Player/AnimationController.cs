using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationController : MonoBehaviour
{
    private Animator anim;

    /// <summary>
    /// Direction we were facing after last movement input
    /// </summary>
    private Vector2 lastFacing;

    void Start()
    {
        // fuckin sweet way to get components check this shit out
        if (!TryGetComponent(out anim)) {
            Debug.LogWarning(name + " | Animator component not found in AnimationController - please attach an Animator to this GameObject");
        }
    }


    /// <summary>
    /// Update animation parameters
    /// </summary>
    /// <param name="movement">Unit-vector representing movement in form (x, z)</param>
    /// <param name="moveType">0 - Standing, 1 - Walking, 3 - Running</param>
    public void UpdateAnims(Vector2 movement)
    {
        // If player is not moving, use values from last non-zero movement input (so we maintian direction after stopping)
        if(movement == Vector2.zero) {
            movement = lastFacing;
            anim.SetFloat("Speed", 0);
        }
        // Otherwise save movement values to maintain direction when stopped
        else {
            lastFacing = movement;
            anim.SetFloat("Speed", 1);
        }
        // Set animation values
        anim.SetFloat("MoveX", movement.x);
        anim.SetFloat("MoveY", movement.y);
        // anim.SetFloat("Speed", moveType);
    }

}