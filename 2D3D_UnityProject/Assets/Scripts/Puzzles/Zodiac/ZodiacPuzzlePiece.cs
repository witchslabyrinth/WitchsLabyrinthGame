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
    /// Animates a piece into the outPosition. Stops and PieceIn() that might be running.
    /// </summary>
    public void PieceOut()
    {
        if (pieceInRoutineInstance != null)
            StopCoroutine(pieceInRoutineInstance);
        pieceOutRoutineInstance = PieceOutRoutine();
        StartCoroutine(pieceOutRoutineInstance);
    }

    private IEnumerator pieceOutRoutineInstance;
    private IEnumerator PieceOutRoutine()
    {
        // Make an animation curve starting from current position and ending at the outPosition y values
        AnimationCurve animationCurve = AnimationCurve.EaseInOut(0, transform.localPosition.y, inOutTime, outPosition.y);

        for (float timer = 0f; timer < inOutTime; timer += Time.deltaTime)
        {
            // Evaluate the animation curve at the current timer value and set the current local position y to that value
            float animValue = animationCurve.Evaluate(timer);
            Vector3 newPosition = transform.localPosition;
            newPosition.y = animValue;
            transform.localPosition = newPosition;
            yield return null;
        }

        // Jump to outPosition
        transform.localPosition = outPosition;
    }

    /// <summary>
    /// Animates a piece into the inPosition. Stops and PieceOut() that might be running.
    /// </summary>
    public void PieceIn()
    {
        if (pieceOutRoutineInstance != null)
            StopCoroutine(pieceOutRoutineInstance);
        pieceInRoutineInstance = PieceInCoroutine();
        StartCoroutine(pieceInRoutineInstance);
    }

    private IEnumerator pieceInRoutineInstance;
    private IEnumerator PieceInCoroutine()
    {
        // Make an animation curve starting from current position and ending at the outPosition y values
        AnimationCurve animationCurve = AnimationCurve.EaseInOut(0, transform.localPosition.y, inOutTime, inPosition.y);

        for (float timer = 0f; timer < inOutTime; timer += Time.deltaTime)
        {
            // Evaluate the animation curve at the current timer value and set the current local position y to that value
            float animValue = animationCurve.Evaluate(timer);
            Vector3 newPosition = transform.localPosition;
            newPosition.y = animValue;
            transform.localPosition = newPosition;
            yield return null;
        }

        // Jump to inPosition
        transform.localPosition = inPosition;
    }
}
