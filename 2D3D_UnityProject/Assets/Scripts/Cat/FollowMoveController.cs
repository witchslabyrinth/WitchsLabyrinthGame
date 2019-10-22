using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMoveController : MoveController
{
    /// <summary>
    /// Target to follow
    /// </summary>
    [SerializeField]
    protected Transform followTarget;

    protected Vector3 lastPosition;

    /// <summary>
    /// Distance to lag behind target path
    /// </summary>
    [SerializeField]
    [Range(0, 5)]
    protected float lagDistance;

    public override void MoveTowards(Vector3 position)
    {
        throw new System.NotImplementedException();
    }

    public override void MoveTowards(Transform transform)
    {
        if(followTargetCoroutine == null) {
            followTargetCoroutine = StartCoroutine(FollowTarget(lastPosition));
        }
    }

    Coroutine followTargetCoroutine;
    IEnumerator FollowTarget(Vector3 position)
    {
        // check how far follow target has moved from last tracked position
        // move same distance towards that follow position
        // if distance between target and last tracked pos >= lagDistance
        // update lastPosition and repeat


        yield return null;
    }
}
