using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZodiacPuzzlePiece : MonoBehaviour
{
    /// <summary>
    /// Distance from starting position that the piece will go out during PieceOut()
    /// </summary>
    [SerializeField]
    private float inOutDistance;

    /// <summary>
    /// Time it takes to complete the coroutines PieceOut() and PieceIn()
    /// </summary>
    [SerializeField]
    private float inOutTime = 1.0f;

    /// <summary>
    /// Local position of piece by default and after a PieceIn()
    /// </summary>
    private Vector3 inPosition;
    /// <summary>
    /// Local position of piece after a PieceOut()
    /// </summary>
    private Vector3 outPosition;

    private void Awake()
    {
        //Calculate in and out positions
        inPosition = transform.localPosition;
        outPosition = inPosition;
        outPosition.y += inOutDistance;
    }

    /// <summary>
    /// Animates a piece into the target puzzle piece position
    /// </summary>
    public void PieceInOut(ZodiacPuzzlePiecePosition targetPosition)
    {
        // Stop any running coroutine
        if (pieceInOutRoutineInstance != null)
            StopCoroutine(pieceInOutRoutineInstance);

        // Set coroutine instance based on parameter
        if (targetPosition == ZodiacPuzzlePiecePosition.In)
            pieceInOutRoutineInstance = PieceInOutCoroutine(inPosition);
        else if (targetPosition == ZodiacPuzzlePiecePosition.Out)
            pieceInOutRoutineInstance = PieceInOutCoroutine(outPosition);
        else
            throw new System.Exception("Reached branch that shouldn't be possible.");

        // Run coroutine
        StartCoroutine(pieceInOutRoutineInstance);
    }

    private IEnumerator pieceInOutRoutineInstance;
    private IEnumerator PieceInOutCoroutine(Vector3 targetPosition)
    {
        // Make an animation curve starting from current position and ending at the outPosition y values
        AnimationCurve animationCurve = AnimationCurve.EaseInOut(0, transform.localPosition.y, inOutTime, targetPosition.y);

        for (float timer = 0f; timer < inOutTime; timer += Time.deltaTime)
        {
            // Evaluate the animation curve at the current timer value and set the current local position y to that value
            float animValue = animationCurve.Evaluate(timer);
            Vector3 newPosition = transform.localPosition;
            newPosition.y = animValue;
            transform.localPosition = newPosition;
            yield return null;
        }

        // Jump to targetPosition
        transform.localPosition = targetPosition;
    }

    public enum ZodiacPuzzlePiecePosition
    {
        In = 0,
        Out = 1
    }
}
