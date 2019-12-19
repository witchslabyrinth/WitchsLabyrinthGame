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
    [SerializeField]
    protected float startingDistance;

    private Vector2 dir;
    private float movingType;

    private Movement movement;

    // Start is called before the first frame update
    void Start()
    {
        dir = new Vector2();
        
        // TODO: move this to a generalized Actor class
        movement = new FollowMovement(followTarget.transform, stoppingDistance, startingDistance);
    }

    // TODO: move this to a generalized Actor class
    public override void MoveTowards(Transform targetTransform)
    {   
        // Get vector towards target
        // Vector3 toTarget = targetTransform.position - transform.position;
        // float distance = toTarget.magnitude;

        // // Move towards target if outside stopping radius
        // if ((movingType == 1 && distance >= stoppingDistance) || (movingType == 0 && distance >= startingDistance)) {
        //     float step = moveSpeed * Time.fixedDeltaTime;
        //     Vector3 movement = toTarget.normalized * step;

        //     transform.position += movement;

        //     movingType = 1;
        //     dir.Set(movement.x, movement.y);
        // }
        // else
        // {
        //     movingType = 0;
        // }
        // updateAnims(dir.x, dir.y, movingType);
        float step = moveSpeed * Time.fixedDeltaTime;
        transform.position += movement.GetMovement(transform) * step;
    }

    void Update()
    {
        MoveTowards(followTarget.transform);
    }
}
