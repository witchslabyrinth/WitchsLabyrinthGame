using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Movement/Follow Movement")]
public class FollowMovement : Movement
{
    private Transform target;

    /// <summary>
    /// Smaller radius of satisfaction around target - stop moving when we are this close
    /// </summary>
    [SerializeField]
    protected float stoppingDistance;
    
    /// <summary>
    /// Larger radius around target - start following the target when we're this far away
    /// </summary>
    [SerializeField]
    protected float startingDistance;

    public FollowMovement(Transform target, float stoppingDistance, float startingDistance)
    {
        this.target = target;
        this.stoppingDistance = stoppingDistance;

        // TODO: make the Get() method use starting distance again
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
