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
    [SerializeField] protected float stoppingDistance = 2;

    /// <summary>
    /// Larger radius around target - start following the target when we're this far away
    /// </summary>
    [SerializeField] protected float startingDistance = 3.5f;

    /// <summary>
    /// True if actor has arrived at the target (stopped), false otherwise (moving)
    /// </summary>
    private bool arrived = true;

    public FollowMovement(Transform target)
    {
        this.target = target;

        // TODO: ignore collision between target and self
    }

    /// <summary>
    /// Returns movement towards Player actor
    /// </summary>
    /// <param name="actor">Actor requesting movement (should not be player-controlled)</param>
    /// <returns></returns>
    public override Vector3 GetMovement(Actor actor)
    {
        // Make sure actor is following player, not themselves (happens after swapping actors)
        if (target.Equals(actor.transform))
        {
            Actor player = PlayerController.Instance.GetPlayer();
            target = player.transform;
        }

        // Get vector towards target
        Vector3 toTarget = target.position - actor.transform.position;
        float distance = toTarget.magnitude; 

        // If we've already arrived at the target
        if (arrived)
        {
            // Wait until they exceed startingDistance to follow again
            if (distance >= startingDistance)
            {
                arrived = false;
                return toTarget.normalized;
            }
        }
        // If we're still moving towards the target
        else
        {
            // Move towards target while outside stopping radius
            if (distance >= stoppingDistance)
                return toTarget.normalized;

            // Stop moving when we've entered stopping radius
            else
                arrived = true;
        }
        return Vector3.zero;
    }

    public override Vector2 GetAnimation(Actor actor)
    {
        // TODO: return proper animation values
        return Vector2.zero;
    }
}