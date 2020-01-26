using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Movement control scheme
/// </summary>
[System.Serializable]
public abstract class Movement : ScriptableObject
{
    // TODO: consider making a Player class and passing it here instead of Transform
    /// <summary>
    /// Returns movement as a unit vector (magnitude 1)
    /// </summary>
    /// <param name="actor">Player instance</param>
    public abstract Vector3 GetMovement(Actor actor);

    /// <summary>
    /// Returns unit vector corresponding to animation direction (which way the player sprite should be facing)
    /// </summary>
    /// <param name="actor"></param>
    public abstract Vector2 GetAnimation(Actor actor);
}
