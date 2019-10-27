using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MoveController : MonoBehaviour
{
    /// <summary>
    /// Movement speed
    /// </summary>
    [SerializeField]
    protected float moveSpeed;

    public abstract void MoveTowards(Vector3 position);

    public abstract void MoveTowards(Transform transform);
}
