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

    [SerializeField]
    [Tooltip("The correct position of this cube for the solved puzzle. 0 for leftmost, 4 for rightmost.")]
    private int correctPosition;

    [SerializeField]
    [Tooltip("The correct rotation of the cube for the solved puzzle. The correct rotation is when the drawn debug ray is pointed upwards (So when the selected local direction matches world up.")]
    private Rotation correctRotation;

    [HideInInspector]
    public bool animating;

    private const float RotationAmount = 90f;

    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        switch (correctRotation)
        {
            case Rotation.Forward:
                Ray forwardRay = new Ray(transform.position, gameObject.transform.forward);
                Gizmos.DrawRay(forwardRay);
                break;
            case Rotation.Back:
                Ray backRay = new Ray(transform.position, gameObject.transform.forward * -1f);
                Gizmos.DrawRay(backRay);
                break;
            case Rotation.Up:
                Ray upRay = new Ray(transform.position, gameObject.transform.up);
                Gizmos.DrawRay(upRay);
                break;
            case Rotation.Down:
                Ray downRay = new Ray(transform.position, gameObject.transform.up * -1f);
                Gizmos.DrawRay(downRay);
                break;
        }
    }
    #endif

    private void Awake()
    {
        animating = false;
    }

    public bool Correct(int position)
    {
        if (position == correctPosition)
        {
            Vector3 direction = Vector3.left;

            switch (correctRotation)
            {
                case Rotation.Forward:
                    direction = gameObject.transform.forward;
                    break;
                case Rotation.Back:
                    direction = gameObject.transform.forward * -1f;
                    break;
                case Rotation.Up:
                    direction = gameObject.transform.up;
                    break;
                case Rotation.Down:
                    direction = gameObject.transform.up * -1f;
                    break;
            }

            if (direction == Vector3.up)
                return true;
            else
                return false;
        }
        else
        {
            return false;
        }
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
        animating = true;

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

        animating = false;
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
        animating = true;

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

        animating = false;
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
        animating = true;

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

        animating = false;
    }

    public void Jump(AnimationCurve animationCurve, float jumpTime)
    {
        StartCoroutine(JumpCoroutine(animationCurve, jumpTime));
    }

    private IEnumerator JumpCoroutine(AnimationCurve animationCurve, float jumpTime)
    {
        Vector3 startPosition = transform.position;

        for (float time = 0f; time < 1f; time += Time.deltaTime)
        {
            float newY = animationCurve.Evaluate(time / jumpTime);
            Vector3 newPosition = new Vector3(startPosition.x, startPosition.y + newY, startPosition.z);
            transform.position = newPosition;

            yield return null;
        }

        transform.position = startPosition;
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
                return Quaternion.AngleAxis(RotationAmount, gameObject.transform.right);
            case PatternPuzzle.Direction.FORWARD:
                return Quaternion.AngleAxis(RotationAmount, gameObject.transform.right * -1f);
        }
        throw new System.Exception("Unexpected code path reached.");
    }

    private enum Rotation
    {
        Forward,
        Back,
        Up,
        Down
    }
}
