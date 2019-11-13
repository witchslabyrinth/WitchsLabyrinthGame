using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiarGameManager : MonoBehaviour
{
    public GameObject canvasObject;

    private DialogueLine dialogueCode;

    public GameObject[] npcs;

    private static LiarGameManager instance;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
            instance = this;
        dialogueCode = canvasObject.GetComponentInChildren<DialogueLine>();
    }

    public void StartConversation(int npc)
    {
        LiarStatue liar = npcs[npc].GetComponentInChildren<LiarStatue>();
        dialogueCode.SetLine(liar.GetStatement());
        dialogueCode.SetName(liar.GetName());
        dialogueCode.SetSubtitle(liar.GetSubtitle());

        canvasObject.SetActive(true);
    }

    public static LiarGameManager Instance()
    {
        return instance;
    }
}
