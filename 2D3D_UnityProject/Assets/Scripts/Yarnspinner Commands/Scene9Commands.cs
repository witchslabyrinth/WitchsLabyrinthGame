using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene9Commands : DialogueCommands
{
    [SerializeField]
    private GameObject scene9Camera;

    private void Awake()
    {
        dialogueRunner.AddCommandHandler("start_cutscene_9", StartCutscene9);
    }

    private void StartCutscene9(string[] parameters)
    {
        StartCutscene(scene9Camera.GetComponent<CameraEntity>());
    }
}
