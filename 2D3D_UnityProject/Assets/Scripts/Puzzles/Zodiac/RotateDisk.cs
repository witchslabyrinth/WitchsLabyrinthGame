﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateDisk : MonoBehaviour
{
    /// <summary>
    /// How fast the disk should rotate
    /// </summary>
    [SerializeField]
    [Range(1,2)]
    private float rotationSpeed = 1.5f;

    /// <summary>
    /// List (ordered containing references to symbols on the disk
    /// </summary>
    public List<Texture2D> symbols;

    // TODO: do this programmatically
    /// <summary>
    /// Hand-placed game object at top of disk, helps us indicate sprite placement (imagine it rotated around the disk)
    /// </summary>
    public GameObject spritePivot;

    /// <summary>
    /// Index of current symbol
    /// </summary>
    private int currentSymbol;

    /// <summary>
    /// Holds angle (in degrees) between each symbol on the disk
    /// </summary>
    private float angleBetweenSymbols;

    /// <summary>
    /// Reference to zodiac puzzle
    /// </summary>
    private ZodiacPuzzle puzzle;

    private void Start()
    {

    }

    public void Init(ZodiacPuzzle puzzle)
    {
        // Break if no symbols found
        if (symbols.Count == 0) {
            Debug.LogErrorFormat("{0}: no symbols found");
            return;
        }

        this.puzzle = puzzle;

        // Calculate angle between symbols
        angleBetweenSymbols = 360f / symbols.Count;

        PopulateSymbols();
    }

    private void PopulateSymbols()
    {
        Debug.LogFormat("{0}: Populating {1} symbols", name, symbols.Count);
        // TODO: Populate disk with symbols radially separated by that angle
        // Get radius from center to sprite pivot
        Vector3 pivotVector = spritePivot.transform.position - puzzle.center.transform.position;
        Debug.LogFormat("Sprite distance: {0}", pivotVector.magnitude);
        Debug.LogFormat("Pivot Vector: {0}", pivotVector);

        foreach(Texture2D texture in symbols) {

        }
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
        Quaternion rotationChange = Quaternion.Euler(new Vector3(0, change));

        // Rotate towards that quaternion
        Quaternion targetRotation = transform.rotation * rotationChange;

        // Track starting rotation to maintain lerp lower-bound
        Quaternion startRotation = transform.rotation; 
        float t = 0;
        while(t < 1) {
            // Update rotation
            Quaternion rotation = Quaternion.Lerp(startRotation, targetRotation, t);
            transform.rotation = rotation;


            // Increment t
            t += rotationSpeed * Time.fixedDeltaTime;
            yield return null;
        }

        // Ensure we land exactly on target rotation
        transform.rotation = targetRotation;

        rotateCoroutineInstance = null;
        yield return null;
    }
}
