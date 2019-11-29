using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Movement
{
    // TODO: consider making a Player class and passing it here instead of Transform
    /// <summary>
    /// Returns movement as a unit vector (magnitude 1)
    /// </summary>
    /// <param name="player">Player instance</param>
    /// <returns></returns>
    public abstract Vector3 Get(Transform player);
}
