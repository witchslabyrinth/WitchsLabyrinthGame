using UnityEngine;
using System.Collections;

/// <summary>
/// Used for freezing player movement/animation
/// </summary>
[CreateAssetMenu(menuName = "Movement/Null Movement")]
public class NullMovement : Movement
{

    public override Vector3 GetMovement(Actor actor)
    {
        return Vector2.zero;
    }

    public override Vector2 GetAnimation(Actor actor)
    {
        return Vector2.zero;
    }
}
