﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Yarn.Unity;

public class LiarCommands : MonoBehaviour
{
    // Drag and drop your Dialogue Runner into this variable.
    [SerializeField]
    private DialogueRunner dialogueRunner;

    [SerializeField]
    private GameObject cutscene5;

    private void Awake()
    {
        dialogueRunner.AddCommandHandler("liar_win", LiarWin);
    }

    private void LiarWin(string[] parameters)
    {
        //stuff for when players solve liars puzzle
        cutscene5.SetActive(true);
    }
}
