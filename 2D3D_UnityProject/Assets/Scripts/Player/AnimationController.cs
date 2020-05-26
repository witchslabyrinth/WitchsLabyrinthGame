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

    private bool isTop;

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

        // Set ref to main camera (avoids GameObject.Find() calls performed by Camera.main references)
        //mainCamera = Camera.main;

        isTop = false;
    }


    /// <summary>
    /// Update animation parameters
    /// </summary>
    /// <param name="movement">Unit-vector representing movement in form (x, z)</param>
    public void UpdateAnims(Vector2 movement, float currSpeed, bool top)
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
        isTop = top;

        // Set animation values
        anim.SetFloat("MoveX", movement.x);
        anim.SetFloat("MoveY", movement.y);
        anim.SetFloat("Speed", currSpeed);
        anim.SetBool("TopCam", top);

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
        // transform.forward = mainCamera.transform.forward;
        CameraEntity mainCam = CameraController.Instance.GetMainCamera();
        transform.rotation = mainCam.transform.rotation;
        if (isTop)
        {
            if (lastFacing.x != 0)
            {
                // transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, lastFacing.x * 90f, transform.rotation.eulerAngles.z);
                transform.Rotate(transform.right, -lastFacing.x * 90f);
            }
            else if (lastFacing.y == -1)
            {
                // transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, 180f, transform.rotation.eulerAngles.z);
                transform.Rotate(transform.right, 180f);
            }
        }
        
    }
}