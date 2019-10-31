using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateDisk : MonoBehaviour
{
    public enum Direction { LEFT, RIGHT }

    /// <summary>
    /// How fast the disk should rotate
    /// </summary>
    [SerializeField]
    private float rotationSpeed;

    /// <summary>
    /// Default rotation value
    /// </summary>
    private Quaternion initialRotation;

    /// <summary>
    /// Index of current symbol
    /// </summary>
    private int currentSymbol;

    /// <summary>
    /// List (ordered containing references to symbols on the disk
    /// </summary>
    public List<Texture2D> symbols;

    /// <summary>
    /// Holds angle (in degrees) between each symbol on the disk
    /// </summary>
    private float angleBetweenSymbols;
    
    private void Start()
    {
        // Calculate angle between symbols
        int numSymbols = symbols.Count;
        angleBetweenSymbols = 360f / numSymbols;
        Debug.LogFormat("Angle between {0} symbols: {1}", numSymbols, angleBetweenSymbols);

        // TODO: Populate disk with symbols radially separated by that angle
    }
}
