using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(SpriteRenderer))]
public class AnimationController : MonoBehaviour
{
    private Animator anim;

    private SpriteRenderer spriteRenderer;

    /// <summary>
    /// Direction we were facing after last movement input
    /// </summary>
    private Vector2 lastFacing;

    private void Start()
    {
        // fuckin sweet way to get components check this shit out
        if (!TryGetComponent(out anim))
        {
            Debug.LogWarning(
                name +
                " | Animator component not found in AnimationController - please attach an Animator to this GameObject");
        }

        if (!TryGetComponent(out spriteRenderer))
        {
            Debug.LogWarning(
                name +
                " | SpriteRenderer component not found in AnimationController - please attach an Animator to this GameObject");
        }
    }


    /// <summary>
    /// Update animation parameters
    /// </summary>
    /// <param name="movement">Unit-vector representing movement in form (x, z)</param>
    public void UpdateAnims(Vector2 movement)
    {
        // If player is not moving, use values from last non-zero movement input (so we maintian direction after stopping)
        if (movement == Vector2.zero)
        {
            movement = lastFacing;
        }
        // Otherwise save movement values to maintain direction when stopped
        else
        {
            lastFacing = movement;
        }

        // Set animation values
        anim.SetFloat("MoveX", movement.x);
        anim.SetFloat("MoveY", movement.y);
        anim.SetFloat("Speed", 0);
    }

    private void LateUpdate()
    {
        // Point sprite towards camera before rendering (ensures no previous rotations affect sprite appearance)
        FaceCamera();
    }

    /// <summary>
    /// Make SpriteRenderer face towards camera
    /// </summary>
    private void FaceCamera()
    {
        transform.forward = Camera.main.transform.forward;
    }
}