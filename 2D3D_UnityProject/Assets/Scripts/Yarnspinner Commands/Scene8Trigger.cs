using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class Scene8Trigger : MonoBehaviour
{
    [SerializeField]
    private YarnProgram scene8;

    [SerializeField]
    private CameraEntity scene8Cam;

    private bool scenePlayed;

    void Start()
    {
        if (scene8 != null)
        {
            DialogueRunner dialogueRunner = FindObjectOfType<Yarn.Unity.DialogueRunner>();
            dialogueRunner.Add(scene8);
        }
        scenePlayed = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!scenePlayed)
        {
            FindObjectOfType<DialogueRunner>().StartDialogue("AfterPortalPuzzle");
            CameraController.Instance.SetMainCamera(scene8Cam);
        }
    }
}
