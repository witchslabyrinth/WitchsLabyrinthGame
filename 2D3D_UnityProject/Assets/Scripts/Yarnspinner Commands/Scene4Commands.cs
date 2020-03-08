using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class Scene4Commands : DialogueCommands
{
    [SerializeField]
    private GameObject scene4Camera;

    [SerializeField]
    private Transform cameraLoc1;

    [SerializeField]
    private float panToSnakeTime = 1f;

    private void Awake()
    {
        dialogueRunner.AddCommandHandler("scene4_camera_1", Scene4Camera1);
    }

    private void Scene4Camera1(string[] parameters)
    {
        // StopCoroutine(currCoroutine);
        scene4Camera.transform.position = PlayerController.Instance.GetPlayer().actorCamera.gameObject.transform.position;
        scene4Camera.transform.rotation = PlayerController.Instance.GetPlayer().actorCamera.gameObject.transform.rotation;
        currCoroutine = StartCoroutine(PanCamera(scene4Camera.transform, cameraLoc1, panToSnakeTime));
    }
}
