using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class Scene5Commands : DialogueCommands
{
    [SerializeField]
    private GameObject scene5Camera;

    private void Awake()
    {
        dialogueRunner.AddCommandHandler("scene5_camera_1", Scene5Camera1);
    }

    private void Scene5Camera1(string[] parameters)
    {
        // StopCoroutine(currCoroutine);
        scene5Camera.transform.position = PlayerController.Instance.GetPlayer().actorCamera.gameObject.transform.position;
        scene5Camera.transform.rotation = PlayerController.Instance.GetPlayer().actorCamera.gameObject.transform.rotation;
    }
}
