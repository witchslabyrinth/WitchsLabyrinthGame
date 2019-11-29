using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMovement : Movement
{
    private Transform target;

    /// <summary>
    /// Radius of satisfaction around target - stop moving when we get this far away
    /// </summary>
    [SerializeField]
    protected float stoppingDistance;
    [SerializeField]
    protected float startingDistance;

    public FollowMovement(Transform target, float stoppingDistance, float startingDistance)
    {
        this.target = target;
        this.stoppingDistance = stoppingDistance;
        this.startingDistance = startingDistance;
    }
    public override Vector3 Get(Transform player)
    {
        // Get vector towards target
        Vector3 toTarget = target.position - player.transform.position;
        float distance = toTarget.magnitude;

        // Move towards target if outside stopping radius
        if ((distance >= stoppingDistance)) 
            return toTarget.normalized;
        else 
            return Vector3.zero;
    }
}
