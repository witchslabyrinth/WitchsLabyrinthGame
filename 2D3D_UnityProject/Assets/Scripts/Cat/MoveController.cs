using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: consider removing this class, or using it in a more generalized Actor class
public abstract class MoveController : MonoBehaviour
{
    /// <summary>
    /// Movement speed
    /// </summary>
    [SerializeField]
    protected float moveSpeed;

    public abstract void MoveTowards(Transform transform);
}
