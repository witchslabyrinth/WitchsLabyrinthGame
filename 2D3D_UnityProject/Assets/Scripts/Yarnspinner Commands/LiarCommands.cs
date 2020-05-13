using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Yarn.Unity;

public class LiarCommands : DialogueCommands
{
    //n8-bit 5/13/2020
    /// <summary>
    /// Indicates whether player has solved the puzzle
    /// </summary>
    public bool solved = false;
    //end n8-bit 5/13/2020

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

        PlayerInteractionController controller = PlayerController.Instance.GetPlayer().GetComponent<PlayerInteractionController>();

        if (controller != null)
            controller.SetInDialogueZone(false, null, null);
        
        foreach(Collider trigger in triggerZones)
        {
            trigger.enabled = false;
        }

        //n8-bit 5/13/2020
        solved = true;
        //end n8-bit 5/13/2020

        onComplete();
    }
}
