using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Movement/Follow Movement")]
public class FollowMovement : Movement
{
    /// <summary>
    /// Target to follow
    /// </summary>
    private Transform target;

    /// <summary>
    /// Sets player actor as target if true
    /// </summary>
    [SerializeField] private bool followPlayer;

    /// <summary>
    /// Smaller radius of satisfaction around target - stop moving when we are this close
    /// </summary>
    [SerializeField] protected float stoppingDistance = 1;

    /// <summary>
    /// Larger radius around target - start following the target when we're this far away
    /// </summary>
    [SerializeField] protected float startingDistance = 3;

    public FollowMovement(Transform target, float stoppingDistance, float startingDistance)
    {
        this.target = target;
        this.stoppingDistance = stoppingDistance;

        // TODO: make the Get() method use starting distance again
        this.startingDistance = startingDistance;
    }

    public FollowMovement(Transform target)
    {
        this.target = target;
    }

    public override Vector3 GetMovement(Actor player)
    {
        if (target == null)
        {
            // Set target to player if specified
            if (followPlayer)
            {
                target = PlayerController.Instance.GetActor().transform;
            }
            // Otherwise return 0
            else
            {
                Debug.LogWarning(player.name + " | FollowMovement.target is null, cannot generate movement");
                return Vector3.zero;
            }
        }

        // Get vector towards target
        Vector3 toTarget = target.position - player.transform.position;
        float distance = toTarget.magnitude;

        // Move towards target if outside stopping radius
        if ((distance >= stoppingDistance))
            return toTarget.normalized;
        else
            return Vector3.zero;
    }

    public override Vector2 GetAnimation(Actor player)
    {
        // TODO: return proper animation values
        return Vector2.zero;
    }
}