using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MoveController : MonoBehaviour
{
    public abstract void MoveTowards(Vector3 position);

    public abstract void MoveTowards(Transform transform);
}
