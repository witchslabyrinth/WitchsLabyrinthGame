    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleSlidingDoors : MonoBehaviour
{
    [SerializeField]
    private Transform leftDoor;
    [SerializeField]
    private Transform rightDoor;
    [SerializeField]
    private AnimationCurve openCloseCurve;
    [SerializeField]
    private float openCloseTime;

    private float openCloseOffset = 3f;

    public delegate void OnFinishedOpenClose();

    /// <summary>
    /// Event fired after door finished opening/closing
    /// </summary>
    public OnFinishedOpenClose onFinished;

    public void Open()
    {
        StartCoroutine(OpenCloseCoroutine(-openCloseOffset));
    }

    public void Close()
    {
        StartCoroutine(OpenCloseCoroutine(openCloseOffset));
    }

    private IEnumerator OpenCloseCoroutine(float offset)
    {
        // Create start and end positions for both doors
        Vector3 leftDoorStartPos = leftDoor.localPosition;
        Vector3 leftDoorEndPos = new Vector3(leftDoorStartPos.x, leftDoorStartPos.y, leftDoorStartPos.z + offset);

        Vector3 rightDoorStartPos = rightDoor.localPosition;
        Vector3 rightDoorEndPos = new Vector3(rightDoorStartPos.x, rightDoorStartPos.y, rightDoorStartPos.z - offset);

        for (float time = 0; time < openCloseTime; time += Time.deltaTime)
        {
            // Evaluate animation curve at current percentage of the way through openCloseTime
            float animCurveEval = openCloseCurve.Evaluate(time / openCloseTime);

            // Lerp left door position with the animation curve evalutation value
            Vector3 leftDoorNewPos = Vector3.Lerp(leftDoorStartPos, leftDoorEndPos, animCurveEval);
            leftDoor.localPosition = leftDoorNewPos;

            // Repeat with right door
            Vector3 rightDoorNewPos = Vector3.Lerp(rightDoorStartPos, rightDoorEndPos, animCurveEval);
            rightDoor.localPosition = rightDoorNewPos;

            yield return null;
        }

        // Unlikely last loop of curve will land on exactly openCloseTime so set final position
        leftDoor.localPosition = leftDoorEndPos;
        rightDoor.localPosition = rightDoorEndPos;

        // Fire finished opening/closing event
        if (offset < 0)
        {
            onFinished?.Invoke();
        }
    }
}
