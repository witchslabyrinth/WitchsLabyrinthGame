using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZodiacPuzzlePiece : MonoBehaviour
{

    [SerializeField]
    private float inOutDistance;

    private float inOutTime = 1.0f;

    private Vector3 inPosition;
    private Vector3 outPosition;

    private void Awake()
    {
        inPosition = transform.localPosition;
        outPosition = inPosition;
        outPosition.y += inOutDistance;
    }

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
        AnimationCurve animationCurve = AnimationCurve.EaseInOut(0, transform.localPosition.y, inOutTime, outPosition.y);

        for (float timer = 0f; timer < inOutTime; timer += Time.deltaTime)
        {
            float animValue = animationCurve.Evaluate(timer);
            Vector3 newPosition = transform.localPosition;
            newPosition.y = animValue;
            transform.localPosition = newPosition;
            yield return null;
        }
        transform.localPosition = outPosition;
    }

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
        AnimationCurve animationCurve = AnimationCurve.EaseInOut(0, transform.localPosition.y, inOutTime, inPosition.y);

        for (float timer = 0f; timer < inOutTime; timer += Time.deltaTime)
        {
            float animValue = animationCurve.Evaluate(timer);
            Vector3 newPosition = transform.localPosition;
            newPosition.y = animValue;
            transform.localPosition = newPosition;
            yield return null;
        }
        transform.localPosition = inPosition;
    }
}
