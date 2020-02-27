using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class Scene2Commands : MonoBehaviour
{
    [SerializeField]
    private DialogueRunner dialogueRunner;

    private Coroutine currCoroutine;

    [SerializeField]
    private GameObject scene2Camera;

    [SerializeField]
    private Transform cameraLoc1;

    [SerializeField]
    private GameObject kaitoObject;

    [SerializeField]
    private float panToKaitoTime = 1f;

    private void Awake()
    {
        dialogueRunner.AddCommandHandler("scene2_camera_2", Scene2Camera2);
        dialogueRunner.AddCommandHandler("scene2_camera_8", Scene2Camera8);
    }

    private void Scene2Camera2(string[] parameters)
    {
        // StopCoroutine(currCoroutine);
        currCoroutine = StartCoroutine(PanToKaito());
        kaitoObject.SetActive(true);
    }

    private void Scene2Camera8(string[] parameters)
    {
        kaitoObject.SetActive(false);
        StopCoroutine(currCoroutine);
    }

    private IEnumerator PanToKaito()
    {
        Vector3 startPos = scene2Camera.transform.position;
        Quaternion startRot = scene2Camera.transform.rotation;
        for (float time = 0; time < panToKaitoTime; time += Time.deltaTime)
        {
            float percentage = time / panToKaitoTime;
            scene2Camera.transform.position = Vector3.Lerp(startPos, cameraLoc1.position, percentage);
            scene2Camera.transform.rotation = Quaternion.Lerp(startRot, cameraLoc1.rotation, percentage);
            yield return null;
        }
        scene2Camera.transform.position = cameraLoc1.position;
        scene2Camera.transform.rotation = cameraLoc1.rotation;
    }
}
