using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KoiFish : MonoBehaviour
{
    /// <summary>
    /// Order in which this fish should be activated to solve the puzzle
    /// </summary>
    [Range(1, 3)]
    public int fishNumber;

    private Animator animator;

    private Dictionary<int, string> koiFishAnimationMap = new Dictionary<int, string>() {
        { 1, "Koi1_Correct" },
        { 2, "Koi2_Correct" },
        { 3, "Koi3_Correct" },


        { -1, "Koi1_Wrong" },
        { -2, "Koi2_Wrong" },
        { -3, "Koi3_Wrong" },
    };

    private Dictionary<int, KeyCode> koiFishButtonMap = new Dictionary<int, KeyCode>() {
        { 1, KeyCode.Alpha1 },
        { 2, KeyCode.Alpha2 },
        { 3, KeyCode.Alpha3 }
    };

    void Start()
    {
        // Pass fish number to animator (used for selecting proper animations)
        animator = GetComponentInChildren<Animator>();
        animator.SetInteger("FishNumber", fishNumber);
    }

    void Update()
    {
        if(Input.GetKeyDown(koiFishButtonMap[fishNumber])) {
            PlayAnimation(fishNumber);
        }
    }


    void PlayAnimation(int animNumber)
    {
        // Play wrong animation if we're holding shift
        if (Input.GetKey(KeyCode.LeftShift)) {
            animNumber *= -1;
        }


        string animationName = koiFishAnimationMap[animNumber];
        Debug.Log(name + " | Triggering " + animationName);

        // Fire path trigger
        animator.SetInteger("ActivationOrder", animNumber);
        animator.SetTrigger("PathTrigger");
    }
}
