using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMoveController : MoveController
{
    [Header("Follow Settings")]
    /// <summary>
    /// Target to follow
    /// </summary>
    [SerializeField]
    protected GameObject followTarget;

    /// <summary>
    /// Radius of satisfaction around target - stop moving when we get this far away
    /// </summary>
    [SerializeField]
    protected float stoppingDistance;

    public override void MoveTowards(Vector3 position)
    {
        throw new System.NotImplementedException();
    }

    public override void MoveTowards(Transform targetTransform)
    {
        // Get vector towards target
        Vector3 toTarget = targetTransform.position - transform.position;
        float distance = toTarget.magnitude;

        // Move towards target if outside stopping radius
        if (distance >= stoppingDistance) {
            float step = moveSpeed * Time.fixedDeltaTime;
            Vector3 movement = toTarget.normalized * step;

            transform.position += movement;
        }
    }

    // TODO: consider moving this call to an abstract behavior component (although that's most likely overkill)
    void Update()
    {
        MoveTowards(followTarget.transform);
    }
}
