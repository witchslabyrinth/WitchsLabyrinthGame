using UnityEngine;
using System.Collections;

/// <summary>
/// Used for freezing player movement/animation
/// </summary>
public class NullMovement : Movement
{

    public override Vector3 GetMovement(Actor player)
    {
        return Vector2.zero;
    }

    public override Vector2 GetAnimation(Actor player)
    {
        return Vector2.zero;
    }
}
