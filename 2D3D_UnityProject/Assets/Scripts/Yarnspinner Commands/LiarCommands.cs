using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Yarn.Unity;

public class LiarCommands : MonoBehaviour
{

    [Header("Wwise")]
    /// <summary>
    /// Set Wwise variables
    /// </summary>
    /// <param name="paused">Set Wwise variables for sounds here</param>
    public AK.Wwise.Event puzzleSolved;
    /// <summary>
 

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
        puzzleSolved.Post(gameObject); //Wwise
        cutscene5.SetActive(true);
    }
}
