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

    /// <summary>
    /// Mapping of fish number to button used to trigger animation
    /// </summary>
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

    void Update()
    {
        // Play fish animation if proper key is pressed
        if(Input.GetKeyDown(koiFishButtonMap[fishNumber])) {
            StartCoroutine(PlayAnimation(fishNumber));
        }
    }


    // TODO: change this to a regular method (not coroutine) after SetTrailActive() hack is resolved
    /// <summary>
    /// Plays the path animation for this fish (coroutine is a hack to disable the trail before the fish warps to starting position)
    /// </summary>
    /// <param name="animNumber"></param>
    /// <returns></returns>
    IEnumerator PlayAnimation(int animNumber)
    {
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
}
