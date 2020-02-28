using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class CutsceneTrigger : MonoBehaviour
{
    [SerializeField]
    private YarnProgram scene;

    [SerializeField]
    private CameraEntity sceneCam;

    [SerializeField]
    private string startNode;

    private bool scenePlayed;

    void Start()
    {
        if (scene != null)
        {
            DialogueRunner dialogueRunner = FindObjectOfType<Yarn.Unity.DialogueRunner>();
            dialogueRunner.Add(scene);
        }
        scenePlayed = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!scenePlayed)
        {
            FindObjectOfType<DialogueRunner>().StartDialogue(startNode);
            CameraController.Instance.SetMainCamera(sceneCam);
            scenePlayed = true;
        }
    }
}
