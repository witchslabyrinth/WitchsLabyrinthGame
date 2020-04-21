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

    /// <summary>
    /// Event fired after door finished opening/closing
    /// </summary>
    public delegate void OnFinishedOpenClose();

    public OnFinishedOpenClose onFinishedOpening;
    public OnFinishedOpenClose onFinishedClosing;

    public void Open()
    {
        StartCoroutine(OpenCloseCoroutine(-openCloseOffset, onFinishedOpening));
    }

    public void Close()
    {
        StartCoroutine(OpenCloseCoroutine(openCloseOffset, onFinishedClosing));
    }

    private IEnumerator OpenCloseCoroutine(float offset, OnFinishedOpenClose onFinishedEvent)
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

        // Fire event when animation finished
        onFinishedEvent?.Invoke();
    }
}
