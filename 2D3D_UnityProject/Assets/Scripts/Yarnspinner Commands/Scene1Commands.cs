using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Yarn.Unity;

public class Scene1Commands : DialogueCommands
{
    [SerializeField]
    private GameObject opCamera;

    [SerializeField]
    private Transform cameraLoc1;

    [SerializeField]
    private Transform cameraLoc3;

    [SerializeField]
    private Transform cameraLoc3Center;

    [SerializeField]
    private Transform cameraLoc4;

    [SerializeField]
    private Transform cameraLoc5;

    [SerializeField]
    private float panToCatTime = 2f;

    [SerializeField]
    private float panToTempleTime = 2f;

    [SerializeField]
    private float angle = 10f;

    [SerializeField]
    private float panToZodiacTime = 2f;

    private void Awake()
    {
        dialogueRunner.AddCommandHandler("start_cutscene_1", StartCutscene1);
        dialogueRunner.AddCommandHandler("op_camera_1", OpCamera1);
        dialogueRunner.AddCommandHandler("op_camera_3", OpCamera3);
        dialogueRunner.AddCommandHandler("op_camera_4", OpCamera4);
        dialogueRunner.AddCommandHandler("op_camera_5", OpCamera5);
    }

    private void StartCutscene1(string[] parameters)
    {
        StartCutscene(opCamera.GetComponent<CameraEntity>());
    }

    private void OpCamera1(string[] parameters, System.Action onComplete)
    {
        currCoroutine = StartCoroutine(PanCamera(opCamera.transform, cameraLoc1, panToCatTime, onComplete));
    }

    private void OpCamera3(string[] parameters)
    {
        StopCoroutine(currCoroutine);
        opCamera.transform.position = cameraLoc3.position;
        opCamera.transform.rotation = cameraLoc3.rotation;
        currCoroutine = StartCoroutine(WholeMapPan());
    }

    private void OpCamera4(string[] parameters)
    {
        StopCoroutine(currCoroutine);
        currCoroutine = StartCoroutine(PanCamera(opCamera.transform, cameraLoc4, panToTempleTime));
    }

    private void OpCamera5(string[] parameters)
    {
        StopCoroutine(currCoroutine);
        currCoroutine = StartCoroutine(PanCamera(opCamera.transform, cameraLoc5, panToZodiacTime));
    }

    private IEnumerator WholeMapPan()
    {
        while (true)
        {
            opCamera.transform.RotateAround(cameraLoc3Center.position, Vector3.up, angle * Time.deltaTime);
            yield return null;
        }
    }
}
