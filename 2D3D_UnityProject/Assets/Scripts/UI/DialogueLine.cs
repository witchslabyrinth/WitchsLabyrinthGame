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
    /// Dialogue line
    /// </summary>
    [SerializeField]
    private string line;

    [Header("Text Printing Options")]
    /// <summary>
    /// Time interval between each character printed to the dialogue box
    /// </summary>
    [SerializeField]
    // [Range(.01f, .1f)]
    private float printSpeed;

    /// <summary>
    /// Sound effect to play while printing dialogue text
    /// </summary>
    [SerializeField]
    private AudioClip soundBlip;

    /// <summary>
    /// Time interval between each blip sound played
    /// </summary>
    [SerializeField]
    [Range(0, .25f)]
    private float soundBlipInterval = .1f;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(printDialogue == null) {
            printDialogue = StartCoroutine(PrintDialogueCoroutine());
        }
    }

    Coroutine printDialogue;

    IEnumerator PrintDialogueCoroutine()
    {
        // Clear previous text before printing
        dialogueText.text = "";

        int charIndex = 0;
        // Continue until full text is printed
        while(!FinishedPrinting()) 
        {
            int numChars = 2;
            PrintChars(charIndex, numChars);
            charIndex += numChars;

            yield return new WaitForSeconds((1/printSpeed) * Time.deltaTime);
        }

        // Set coroutine to null
        printDialogue = null;
    }

    /// <summary>
    /// Prints series of characters of length numChars, starting from currentIndex 
    /// </summary>
    /// <param name="currentIndex">First character to print</param>
    /// <param name="numChars">Number of characters to print</param>
    void PrintChars(int currentIndex, int numChars)
    {
        try {
            // Print next character in string
            char[] array = line.ToCharArray();
            for(int i = 0; i < numChars; i++) {
                dialogueText.text += array[currentIndex];
                currentIndex++;
            }
        }
        // Catch and ignore out-of-bounds exception if we exceed end of string
        catch(IndexOutOfRangeException ex) {
            return;
        } 
    }

    bool FinishedPrinting()
    {
        return dialogueText.text == line;
    }

    void PrintAdditionalLetter()
    {

    }
}
