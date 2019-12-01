using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationController : MonoBehaviour
{
    private Animator anim;

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
    /// <param name="xdir">looking left or right, from -1 to 1</param>
    /// <param name="ydir">looking down or up, from -1 to 1</param>
    /// <param name="moveType">0 - Standing, 1 - Walking, 3 - Running</param>
    public void UpdateAnims(float xdir, float ydir, float moveType)
    {
        anim.SetFloat("MoveX", xdir);
        anim.SetFloat("MoveY", ydir);
        anim.SetFloat("Speed", moveType);
    }

}