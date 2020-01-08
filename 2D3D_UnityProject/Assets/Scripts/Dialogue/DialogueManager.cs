using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [SerializeField]
    private string[] topics_1;

    private bool toggleBool;

    private string[] uiText;

    // Start is called before the first frame update
    void Start()
    {
        topics_1 = new string[6];

        toggleBool = false;

        topics_1[0] = "null";
        topics_1[1] = "Hi";
        topics_1[2] = "Where am I?";
        topics_1[3] = "How do I get out of the maze?";
        topics_1[4] = "What happened to me?";
        topics_1[5] = "Gotta go";

        uiText = new string[6];
        
        
        LoadTopic();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnGUI()
    {
        // Make a group on the center of the screen 
        GUI.BeginGroup(new Rect(0, Screen.height / 2, 650, 500));
        var y = 35;
        // this will increment the y position for each topic    

        // text panel 
        GUI.Box(new Rect(0, 20, 400, 230), "");

        //Topic 1 
        if (uiText[1] != "")
        {
            if (GUI.Button(new Rect(0, y, 500, 35), ""))
            {
                //ProcessReply(uiReply[1]);   
            }
            toggleBool = GUI.Toggle(new Rect(25, y, 600, 35), false, uiText[1]);
            y += 35;
            // increment the next GUI position by 35 
        }
        //Topic 2 
        if (uiText[2] != "")
        {
            if (GUI.Button(new Rect(0, y, 500, 35), ""))
            {
                //ProcessReply(uiReply[2]);   
            }
            toggleBool = GUI.Toggle(new Rect(25, y, 600, 35), false, uiText[2]);
            y += 35;
            // increment the next GUI position by 35 
        }
        //Topic 3 
        if (uiText[3] != "")
        {
            if (GUI.Button(new Rect(0, y, 500, 35), ""))
            {
                //ProcessReply(uiReply[3]);   
            }
            toggleBool = GUI.Toggle(new Rect(25, y, 600, 35), false, uiText[3]);
            y += 35;
            // increment the next GUI position by 35 
        }
        //Topic 4
        if (uiText[4] != "")
        {
            if (GUI.Button(new Rect(0, y, 500, 35), ""))
            {
                //ProcessReply(uiReply[4]);   
            }
            toggleBool = GUI.Toggle(new Rect(25, y, 600, 35), false, uiText[4]);
            y += 35;
            // increment the next GUI position by 35 
        }
        //Topic 5 
        if (uiText[5] != "")
        {
            if (GUI.Button(new Rect(0, y, 500, 35), ""))
            {
                //ProcessReply(uiReply[5]);   
            }
            toggleBool = GUI.Toggle(new Rect(25, y, 600, 35), false, uiText[5]);
            y += 35;
            // increment the next GUI position by 35 
        }
        //Topic 6 
        // if (uiText[6] != "")
        // {
        //     if (GUI.Button(new Rect(0, y, 500, 35), ""))
        //     {
        //         //ProcessReply(uiReply[6]);   
        //     }
        //     toggleBool = GUI.Toggle(new Rect(25, y, 600, 35), false, uiText[6]);
        //     y += 35;
        //     // increment the next GUI position by 35 
        // }

        GUI.EndGroup();
    }

    private void LoadTopic()
    {
        for (int i = 1; i < 6; i++)
        {
            uiText[i] = " "; // clear the current text in case there is no new text      
            //assign the topic text      
            uiText[i] = topics_1[i];
        }
    }
}
