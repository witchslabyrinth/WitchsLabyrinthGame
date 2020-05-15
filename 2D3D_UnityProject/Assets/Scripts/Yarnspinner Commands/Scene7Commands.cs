using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene7Commands : DialogueCommands
{
    [SerializeField]
    private CameraEntity scene7Camera;

    private void Awake()
    {
        dialogueRunner.AddCommandHandler("start_cutscene_7", StartCutscene7);
    }

    private void StartCutscene7(string[] parameters)
    {
        StartCutscene(scene7Camera);
    }
}
