using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class Scene6Commands : DialogueCommands
{
    [SerializeField]
    private GameObject scene6Camera;

    [SerializeField]
    private Transform cameraLoc1;

    [SerializeField]
    private float panToMirrorTime = 1.5f;

    private void Awake()
    {
        dialogueRunner.AddCommandHandler("scene6_camera_1", Scene6Camera1);
    }

    private void Scene6Camera1(string[] parameters)
    {
        // StopCoroutine(currCoroutine);
        scene6Camera.transform.position = PlayerController.Instance.GetPlayer().actorCamera.gameObject.transform.position;
        scene6Camera.transform.rotation = PlayerController.Instance.GetPlayer().actorCamera.gameObject.transform.rotation;
        currCoroutine = StartCoroutine(PanCamera(scene6Camera.transform, cameraLoc1, panToMirrorTime));
    }
}
