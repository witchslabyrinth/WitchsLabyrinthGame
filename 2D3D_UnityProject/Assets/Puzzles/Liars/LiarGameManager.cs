using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///    ENTIRE SCRIPT MARKED FOR DELETION IN REFACTOR    ///

public class LiarGameManager : MonoBehaviour
{
    /// <summary>
    /// reference to the canvas
    /// </summary>
    public GameObject canvasObject;

    /// <summary>
    /// reference to the DialogueLine script
    /// </summary>
    private DialogueLine dialogueCode;

    /// <summary>
    /// list of all npcs you can talk to in the scene
    /// </summary>
    public GameObject[] npcs;

    /// <summary>
    /// reference to player
    /// </summary>
    public GameObject player;

    /// <summary>
    /// instance of this manager
    /// </summary>
    private static LiarGameManager instance;

    private int currNpc;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
            instance = this;
        dialogueCode = canvasObject.GetComponentInChildren<DialogueLine>();
    }

    /// <summary>
    /// activate the canvas and display the name, subtitle, and line of the person talking
    /// </summary>
    /// <param name="npc">ID of the person currently talking</param>
    public void StartConversation(int npc)
    {
        LiarStatue liar = npcs[npc].GetComponentInChildren<LiarStatue>();
        dialogueCode.SetLine(liar.GetStatement());
        dialogueCode.SetName(liar.GetName());
        dialogueCode.SetSubtitle(liar.GetSubtitle());

        canvasObject.SetActive(true);
        player.GetComponent<PlayerController>().enabled = false;

        currNpc = npc;
    }

    public void CheckOrb(int npc)
    {
        LiarStatue liar = npcs[npc].GetComponentInChildren<LiarStatue>();
        dialogueCode.SetLine(liar.GetWinLoseStatement());
        dialogueCode.SetName(liar.GetName());
        dialogueCode.SetSubtitle(liar.GetSubtitle());

        canvasObject.SetActive(true);
        player.GetComponent<PlayerController>().enabled = false;
    }

    public void CheckOrb()
    {
        LiarStatue liar = npcs[currNpc].GetComponentInChildren<LiarStatue>();
        dialogueCode.SetLine(liar.GetWinLoseStatement());
        dialogueCode.SetName(liar.GetName());
        dialogueCode.SetSubtitle(liar.GetSubtitle());

        canvasObject.SetActive(true);
        player.GetComponent<PlayerController>().enabled = false;
        dialogueCode.yesNoButtons.SetActive(false);
        dialogueCode.printDialogue = dialogueCode.StartCoroutine(dialogueCode.PrintDialogueCoroutine());
        if (currNpc == 0)
            player.GetComponent<PlayerController>().orb.SetActive(true);
    }

    /// <summary>
    /// return the instance of this manager
    /// </summary>
    /// <returns>the instance of this manager</returns>
    public static LiarGameManager Instance()
    {
        return instance;
    }
}
