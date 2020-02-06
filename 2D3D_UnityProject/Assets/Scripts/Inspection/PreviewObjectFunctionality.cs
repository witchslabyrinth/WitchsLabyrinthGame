using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//allows an interactable object to be rotated in 3 dimensions
public class PreviewObjectFunctionality : MonoBehaviour
{
    /// <summary>
    /// Mouse attributes
    /// </summary>
    private Vector3 mPrevPos;
    private Vector3 mPosDelta;

    [SerializeField]
    private Camera linkedInspectCam;

    [SerializeField]
    private int index;

    [SerializeField]
    private float rotationSpeed = 0.25f;

    /// <summary>
    /// Initial object location and rotation
    /// </summary>
    private Vector3 initPos;

    private Quaternion initRot;

    private void Start()
    {
        mPrevPos = Vector3.zero;
        mPosDelta = Vector3.zero;

        initPos = transform.position;
        initRot = transform.rotation;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && linkedInspectCam.GetComponent<CameraFollow>().GetObjectIndex() == index)
        {

            mPosDelta = Input.mousePosition - mPrevPos;
            Vector3 angle = mPosDelta * rotationSpeed;

            transform.Rotate(Vector3.up, -Vector3.Dot(angle, linkedInspectCam.transform.right), Space.World); //project left and right mouse movement onto object

            transform.Rotate(linkedInspectCam.transform.right, Vector3.Dot(angle, linkedInspectCam.transform.up), Space.World); //project other mouse movement onto object

        }
        mPrevPos = Input.mousePosition;
    }

    public int GetIndex()
    {
        return index;
    }

    public void MoveToCamera(Vector3 endPos, float moveTime)
    {
        StartCoroutine(MoveObjectCoroutine(transform.position, endPos, transform.rotation, transform.rotation, moveTime));
    }

    public void ResetObject(float moveTime)
    {
        StartCoroutine(MoveObjectCoroutine(transform.position, initPos, transform.rotation, initRot, moveTime));
    }

    private IEnumerator MoveObjectCoroutine(Vector3 startPos, Vector3 endPos, Quaternion startRot, Quaternion endRot, float moveTime)
    {
        for (float time = 0; time < moveTime; time += Time.deltaTime)
        {
            float curveEval = time / moveTime;

            Vector3 newPos = Vector3.Lerp(startPos, endPos, curveEval);
            transform.position = newPos;

            Quaternion newRot = Quaternion.Lerp(startRot, endRot, curveEval);
            transform.rotation = newRot;

            yield return null;
        }

        // Unlikely last loop of curve will land on exactly openCloseTime so set final position
        transform.position = endPos;
    }
}
