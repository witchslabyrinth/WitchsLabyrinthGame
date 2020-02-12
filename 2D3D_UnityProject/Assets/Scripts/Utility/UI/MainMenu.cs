using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Buttons")]
    /// <summary>
    /// Player presses this to unpause game
    /// </summary>
    [SerializeField] private Button playButton;

    /// <summary>
    /// Player presses this to open options menu
    /// </summary>
    [SerializeField] private Button settingsButton;

    /// <summary>
    /// Player presses this to quit game
    /// </summary>
    [SerializeField] private Button quitButton;

    [SerializeField] private FadeEffect fadeEffect;

    // Start is called before the first frame update
    void Start()
    {
        // Set up button events
        playButton.onClick.AddListener(() => SceneLoader.LoadScene(SCENE_ID.INTRO));
        quitButton.onClick.AddListener(() => Application.Quit());
        // TODO: implement Settings button functionality

        fadeEffect.FadeIn();
    }
}
