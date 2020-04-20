using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Yarn.Unity;

public class LiarCommands : DialogueCommands
{
    [SerializeField]
    private Collider[] triggerZones;

    [Header("Wwise")]
    /// <summary>
    /// Set Wwise variables
    /// </summary>
    /// <param name="paused">Set Wwise variables for sounds here</param>
    public AK.Wwise.Event puzzleSolved;
    /// <summary>

    [SerializeField]
    private GameObject cutscene5;

    private void Awake()
    {
        dialogueRunner.AddCommandHandler("liar_win", LiarWin);
    }

    private void LiarWin(string[] parameters, System.Action onComplete)
    {
        //stuff for when players solve liars puzzle
        puzzleSolved.Post(gameObject); //Wwise
        cutscene5.SetActive(true);
        foreach(Collider trigger in triggerZones)
        {
            trigger.enabled = false;
        }
        
        onComplete();
    }
}
