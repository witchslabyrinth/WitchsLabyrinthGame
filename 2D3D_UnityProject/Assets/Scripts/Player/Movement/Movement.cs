using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Movement : ScriptableObject
{
    // TODO: consider making a Player class and passing it here instead of Transform
    /// <summary>
    /// Returns movement as a unit vector (magnitude 1)
    /// </summary>
    /// <param name="player">Player instance</param>
    /// <returns></returns>
    public abstract Vector3 GetMovement(Actor player);

    /// <summary>
    /// Returns unit vector corresponding to animation direction (which way the player sprite should be facing)
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    public abstract Vector2 GetAnimation(Actor player);
}
