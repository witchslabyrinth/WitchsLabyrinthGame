using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternCube : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Amount the cube will travel in the y direction when it is selected")]
    private float selectYOffset;

    [SerializeField]
    [Tooltip("The amount of time it takes to raise and lower the cube.")]
    private float raiseLowerTime;

    [SerializeField]
    [Tooltip("The amount of time it takes to rotate the cube.")]
    private float rotateTime;

    [SerializeField]
    [Tooltip("The amount of time it takes to translate the cube.")]
    private float translateTime;

    private const float RotationAmount = 90f;

    private Vector3 startPosition;

    private void Awake()
    {
        startPosition = transform.localPosition;
    }

    public void Select()
    {
        if (raiseCoroutineInstance != null) StopCoroutine(raiseCoroutineInstance);
        raiseCoroutineInstance = RaiseCoroutine(true);
        StartCoroutine(raiseCoroutineInstance);
    }

    public void Deselect()
    {
        if (raiseCoroutineInstance != null) StopCoroutine(raiseCoroutineInstance);
        raiseCoroutineInstance = RaiseCoroutine(false);
        StartCoroutine(raiseCoroutineInstance);
    }

    private IEnumerator raiseCoroutineInstance;
    /// <summary>
    /// Raises and lowers the cube.
    /// </summary>
    /// <param name="raise">True to raise, false to lower.</param>
    /// <returns></returns>
    private IEnumerator RaiseCoroutine(bool raise)
    {
        // Get the inital position and the final position of the raise/lower based on the parameter
        Vector3 raisePosition = new Vector3(transform.localPosition.x, transform.localPosition.y + selectYOffset, transform.localPosition.z);
        Vector3 lowerPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - selectYOffset, transform.localPosition.z);
        Vector3 initPosition = transform.localPosition;
        Vector3 finalPosition = raise ? raisePosition : lowerPosition;

        // Animate from initPosition to finalPosition
        for (float time = 0f; time < raiseLowerTime; time += Time.deltaTime)
        {
            Vector3 newPosition = Vector3.Lerp(initPosition, finalPosition, time / raiseLowerTime);
            transform.localPosition = newPosition;

            yield return null;
        }

        // Set final location on last frame of coroutine
        transform.localPosition = finalPosition;
    }

    /// <summary>
    /// Rotates the cube in the direction given.
    /// </summary>
    /// <param name="direction"></param>
    public void Rotate(PatternPuzzle.Direction direction)
    {
        if (rotateCoroutineInstance == null)
        {
            rotateCoroutineInstance = (RotateCoroutine(direction));
            StartCoroutine(rotateCoroutineInstance);
        }
    }

    private IEnumerator rotateCoroutineInstance;
    private IEnumerator RotateCoroutine(PatternPuzzle.Direction direction)
    {
        // Get the initial rotation and calculate final rotation based on parameter
        Quaternion initRotation = transform.localRotation;
        Quaternion rotationAmount = DirectionToQuaternion(direction);
        Quaternion finalRotation = initRotation * rotationAmount;

        // Animate from initRotation to finalRotation
        for (float time = 0f; time < rotateTime; time += Time.deltaTime)
        {
            Quaternion newRotation = Quaternion.Lerp(initRotation, finalRotation, time / rotateTime);
            transform.localRotation = newRotation;

            yield return null;
        }

        // Set final rotation on last frame of coroutine
        transform.localRotation = finalRotation;

        // Set coroutine instance to null so it can run again
        rotateCoroutineInstance = null;
    }

    /// <summary>
    /// Translates cube from initPosition to targetPosition.
    /// </summary>
    /// <param name="initPosition"></param>
    /// <param name="targetPosition"></param>
    public void Translate(Vector3 initPosition, Vector3 targetPosition)
    {
        if (translateCoroutineInstance == null)
        {
            translateCoroutineInstance = (translateCoroutine(initPosition, targetPosition));
            StartCoroutine(translateCoroutineInstance);
        }
    }

    private IEnumerator translateCoroutineInstance;
    private IEnumerator translateCoroutine(Vector3 initPosition, Vector3 targetPosition)
    {
        // Animate from initPosition to targetPosition
        for (float time = 0f; time < translateTime; time += Time.deltaTime)
        {
            Vector3 newPosition = Vector3.Lerp(initPosition, targetPosition, time / translateTime);
            transform.localPosition = newPosition;

            yield return null;
        }

        // Set final position on last frame of coroutine
        transform.localPosition = targetPosition;

        // Set coroutine instance to null so it can run again
        translateCoroutineInstance = null;
    }

    /// <summary>
    /// Helper function that takes in a Direction and returns a quaternion that represents a rotation in that direction.
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    private Quaternion DirectionToQuaternion(PatternPuzzle.Direction direction)
    {
        switch(direction)
        {
            case PatternPuzzle.Direction.BACKWARD:
                return Quaternion.AngleAxis(RotationAmount, Vector3.left);
            case PatternPuzzle.Direction.FORWARD:
                return Quaternion.AngleAxis(RotationAmount, Vector3.right);
        }
        throw new System.Exception("Unexpected code path reached.");
    }
}
