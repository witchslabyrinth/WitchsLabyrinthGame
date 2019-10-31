using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateDisk : MonoBehaviour
{

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

    }

    public void Init()
    {
        // Calculate angle between symbols
        int numSymbols = symbols.Count;
        angleBetweenSymbols = 360f / numSymbols;
        //Debug.LogFormat("Angle between {0} symbols: {1}", numSymbols, angleBetweenSymbols);

        // TODO: Populate disk with symbols radially separated by that angle
        // Get radius from center to ring
    }

    /// <summary>
    /// Rotates disk left/right towards the next symbol along the ring
    /// </summary>
    /// <param name="direction"></param>
    public void Rotate(ZodiacPuzzle.Direction direction)
    {
        if(rotateCoroutineInstance == null) {
            rotateCoroutineInstance = RotateCoroutine(direction);
            StartCoroutine(rotateCoroutineInstance);
        }
    }

    private IEnumerator rotateCoroutineInstance;
    private IEnumerator RotateCoroutine(ZodiacPuzzle.Direction direction)
    {
        // Calculate degrees of rotation (in given distance)
        float currentRotation = transform.rotation.x;
        float change = (angleBetweenSymbols * (int)direction);
        Debug.LogFormat("Rotating {0} degrees to the {1}", change, direction);

        // Create quaternion containing change in rotation (relative to current reference frame)
        Quaternion rotationChange = Quaternion.Euler(new Vector3(change, 0));
        Debug.Log(rotationChange.eulerAngles);

        // Rotate towards that quaternion


        rotateCoroutineInstance = null;
        yield return null;
    }
}
