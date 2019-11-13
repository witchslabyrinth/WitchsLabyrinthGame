using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DialogueLine : MonoBehaviour
{
    [Header("Dialogue Content")]
    /// <summary>
    /// Reference to dialogue box UI text element - where we're printing the text
    /// </summary>
    [SerializeField]
    private Text dialogueText;

    /// <summary>
    /// reference to name box UI element
    /// </summary>
    [SerializeField]
    private Text nameText;

    /// <summary>
    /// reference to subtitle box UI element
    /// </summary>
    [SerializeField]
    private Text subtitleText;

    /// <summary>
    /// Dialogue line
    /// </summary>
    [SerializeField]
    private string line;

    [Header("Text Printing Options")]
    /// <summary>
    /// Rate at which text is printed
    /// </summary>
    [SerializeField]
    // [Range(.01f, .1f)]
    private float printSpeed = 100;

    /// <summary>
    /// Number of character to print to screen per UI refresh
    /// </summary>
    private int printChunkSize = 2;

    /// <summary>
    /// Sound effect to play while printing dialogue text
    /// </summary>
    [SerializeField]
    private SoundTrack soundBlip;

    /// <summary>
    /// Time interval between each blip sound played (scales relative to printSpeed)
    /// </summary>
    [SerializeField]
    [Range(0, 1)]
    private float soundBlipSpeed = .5f;

    /// <summary>
    /// reference to parent canvas object
    /// </summary>
    public GameObject canvasObj;

    // Update is called once per frame
    void Update()
    {
        if (printDialogue == null)
        {
            printDialogue = StartCoroutine(PrintDialogueCoroutine());
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (FinishedPrinting())
            {
                canvasObj.SetActive(false);
            }
            else
            {
                StopCoroutine(printDialogue);
                dialogueText.text = line;
                printDialogue = null;
            }
        }
    }

    Coroutine printDialogue;

    IEnumerator PrintDialogueCoroutine()
    {
        // Clear previous text before printing
        dialogueText.text = "";

        int charIndex = 0;
        // Continue until full text is printed
        while (!FinishedPrinting())
        {
            // Print next chunk of characters
            int numChars = 2;
            PrintChars(charIndex, numChars);
            charIndex += numChars;

            PlaySoundBlips();

            // Wait and repeat
            float printInterval = (1 / printSpeed) * Time.deltaTime;
            yield return new WaitForSeconds(printInterval);
        }

        while (!Input.GetKeyDown(KeyCode.Return))
        {
            yield return null;
        }

        // Set coroutine to null so we know it finished
        printDialogue = null;
    }

    #region Sound_Blips
    void PlaySoundBlips()
    {
        if (!FinishedPrinting() && playSoundBlips == null)
        {
            playSoundBlips = StartCoroutine(PlaySoundBlipsCoroutine());
        }
    }

    Coroutine playSoundBlips;

    IEnumerator PlaySoundBlipsCoroutine()
    {
        // Play sound effect and wait interval
        SoundController.Instance.PlaySoundEffect(soundBlip);

        float soundBlipInterval = ((1 / printSpeed) * Time.deltaTime) * (1 / soundBlipSpeed);
        yield return new WaitForSeconds(soundBlipInterval);

        // TODO: Stop sound effect if still playing
        playSoundBlips = null;
    }
    #endregion

    /// <summary>
    /// Prints series of characters of length numChars, starting from currentIndex 
    /// </summary>
    /// <param name="currentIndex">First character to print</param>
    /// <param name="numChars">Number of characters to print</param>
    void PrintChars(int currentIndex, int numChars)
    {
        try
        {
            // Print next character in string
            char[] array = line.ToCharArray();
            for (int i = 0; i < numChars; i++)
            {
                dialogueText.text += array[currentIndex];
                currentIndex++;
            }
        }
        // Catch and ignore out-of-bounds exception if we exceed end of string
        catch (IndexOutOfRangeException ex)
        {
            return;
        }
    }

    bool FinishedPrinting()
    {
        return dialogueText.text == line;
    }

    /// <summary>
    /// change the line being said
    /// </summary>
    /// <param name="newLine">new line to use</param>
    public void SetLine(string newLine)
    {
        line = newLine;
    }

    /// <summary>
    /// change the name of npc that's talking
    /// </summary>
    /// <param name="newName">name of npc</param>
    public void SetName(string newName)
    {
        nameText.text = newName;
    }

    /// <summary>
    /// change the subtitle of npc that's talking
    /// </summary>
    /// <param name="newName">name of npc</param>
    public void SetSubtitle(string newSubtitle)
    {
        subtitleText.text = newSubtitle;
    }
}
