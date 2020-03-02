using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class Scene2Commands : DialogueCommands
{
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
        currCoroutine = StartCoroutine(PanCamera(scene2Camera.transform, cameraLoc1, panToKaitoTime));
        kaitoObject.SetActive(true);
    }

    private void Scene2Camera8(string[] parameters)
    {
        kaitoObject.SetActive(false);
        StopCoroutine(currCoroutine);
    }
}
