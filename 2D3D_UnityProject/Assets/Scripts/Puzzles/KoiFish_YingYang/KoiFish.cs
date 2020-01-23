﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Author: Drew Graham
public class KoiFish : MonoBehaviour
{
    /// <summary>
    /// Index of path animation to play (see animation controller for mapping)
    /// </summary>
    [Range(1, 3)]
    public int fishAnimationNumber;

    /// <summary>
    /// Amount of time player is given to feed this fish before puzzle resets
    /// </summary>
    public float feedDuration;

    [Header("Particle System Settings")]
    /// <summary>
    /// Color over lifetime used for the particle trail module
    /// </summary>
    [SerializeField]
    private Gradient trailColorOverLifetime;

    /// <summary>
    /// Color over lifetime used for the particles
    /// </summary>
    [SerializeField]
    private Gradient particleColorOverLifetime;

    private Animator animator;
    private ParticleSystem psystem;

    void Start()
    {
        // Pass fish number to animator (used for selecting proper animations)
        animator = GetComponentInChildren<Animator>();
        animator.SetInteger("FishNumber", fishAnimationNumber);

        psystem = GetComponentInChildren<ParticleSystem>();
        try 
        {
            // Initialize particle system with color settings
            ParticleSystem.TrailModule trailModule = psystem.trails;
            trailModule.colorOverLifetime = trailColorOverLifetime;

            ParticleSystem.ColorOverLifetimeModule colorOverLifetimeModule = psystem.colorOverLifetime;
            colorOverLifetimeModule.color = particleColorOverLifetime;

        } catch (System.NullReferenceException ex) 
        {
            Debug.LogWarning(name + " | missing ParticleSystem component in children - please add a ParticleSystem to this fish");
            Debug.LogError(ex.ToString());
        }
    }

    // TODO: call from PlayerInteractionController when player feeds fish
    /// <summary>
    /// Triggers fish path animation and sends message to KoiFishPuzzle.FeedFish().
    /// Called by PlayerInteractionController when player feeds fish.
    /// </summary>
    public void Feed()
    {
        // Start fish path animation
        StartCoroutine(PlayAnimation(fishAnimationNumber));

        // Update puzzle accordingly
        KoiFishPuzzle.Instance.FeedFish(this);
    }


    // TODO: change this to a regular method (not coroutine) after SetTrailActive() hack is resolved
    /// <summary>
    /// Plays the path animation for this fish (coroutine is a hack to disable the trail before the fish warps to starting position)
    /// </summary>
    /// <param name="animNumber"></param>
    /// <returns></returns>
    IEnumerator PlayAnimation(int animNumber)
    {
        // TODO: change this to play wrong animation when fish fed out of order
        // Play wrong animation if we're holding shift
        if (Input.GetKey(KeyCode.LeftShift)) {
            animNumber *= -1;
        }

        // Fire path trigger
        animator.SetInteger("ActivationOrder", animNumber);
        animator.SetTrigger("PathTrigger");

        // TODO: remove this hack when fish path animations are made seamless so fish don't warp
        // Disable trail until fish warps to path starting position
        SetTrailActive(false);
        yield return new WaitForSeconds(.5f);
        SetTrailActive(true);
    }

    /// <summary>
    /// Enables/disables trail emission behind fish
    /// </summary>
    /// <param name="active"></param>
    void SetTrailActive(bool active)
    {
        ParticleSystem.EmissionModule emission = psystem.emission;
        emission.enabled = active;
    }

    public void ToggleTrailActive()
    {
        //ParticleSystem.EmissionModule emission = psystem.emission;
        SetTrailActive(!psystem.emission.enabled);
    }
}
