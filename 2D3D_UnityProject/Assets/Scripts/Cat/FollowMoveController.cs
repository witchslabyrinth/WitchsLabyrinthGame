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

    private Animator anim;
    private Vector2 dir;
    private float movingType;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        dir = new Vector2();
    }

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
        if ((movingType == 1 && distance >= stoppingDistance) || (movingType == 0 && distance >= startingDistance)) {
            float step = moveSpeed * Time.fixedDeltaTime;
            Vector3 movement = toTarget.normalized * step;

            transform.position += movement;

            movingType = 1;
            dir.Set(movement.x, movement.y);
        }
        else
        {
            movingType = 0;
        }
        updateAnims(dir.x, dir.y, movingType);
    }

    // TODO: consider moving this call to an abstract behavior component (although that's most likely overkill)
    void Update()
    {
        MoveTowards(followTarget.transform);
    }

    /// <summary>
    /// Update animation parameters
    /// </summary>
    /// <param name="xdir">looking left or right, from -1 to 1</param>
    /// <param name="ydir">looking down or up, from -1 to 1</param>
    /// <param name="moveType">0 - Standing, 0.2 - Walking, 1 - Running</param>
    private void updateAnims(float xdir, float ydir, float moveType)
    {
        anim.SetFloat("MoveX", xdir);
        anim.SetFloat("MoveY", ydir);
        anim.SetFloat("Speed", moveType);
    }
}
