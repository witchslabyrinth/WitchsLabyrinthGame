using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : Singleton<PauseMenu>
{
    [Header("Wwise")]
    /// <summary>
    /// Set Wwise variables
    /// </summary>
    /// <param name="paused">Set Wwise variables for UI here</param>
    //public AK.Wwise.Event OnMenuEnter;
    public AK.Wwise.Event OnMenuExit;
    public AK.Wwise.Event OnMenuHover;
    public AK.Wwise.Event OnMenuSelect;

    public bool paused { get; private set; }

    /// <summary>
    /// Event fired when game is paused/unpaused
    /// </summary>
    /// <param name="paused">True if game was paused, false if game was unpaused</param>
    public delegate void SetPausedEvent(bool paused);

    /// <summary>
    /// Event fired when game is paused/unpaused
    /// </summary>
    public SetPausedEvent onSetGamePaused;

    /// <summary>
    /// UI element containing pause menu contents
    /// </summary>
    [SerializeField] private GameObject pauseMenu;

    [Header("Buttons")]
    /// <summary>
    /// Player presses this to unpause game
    /// </summary>
    [SerializeField] private Button resumeButton;

    /// <summary>
    /// Player presses this to open options menu
    /// </summary>
    [SerializeField] private Button settingsButton;

    /// <summary>
    /// Player presses this to quit game
    /// </summary>
    [SerializeField] private Button quitButton;

    // Start is called before the first frame update
    void Start()
    {
        // Set up button events
        resumeButton.onClick.AddListener(() => SetPaused(false));
        quitButton.onClick.AddListener(() => Application.Quit());

        // Start with the game unpaused
        SetPaused(false);
        AkSoundEngine.SetState("Menu", "OutOfMenu"); //Set state to muffle music
        Debug.Log("Set State to OutOfMenu");
    }

    /// <summary>
    /// Pauses/unpauses game
    /// </summary>
    public void TogglePaused()
    {
        SetPaused(!paused);
        AkSoundEngine.SetState("Menu", "InMenu"); //Set state to play regular music
        Debug.Log("Set State to InMenu");
        OnMenuExit.Post(gameObject);
    }

    /// <summary>
    /// Sets game to paused/unpaused
    /// </summary>
    /// <param name="paused">If true game will be paused, if false game will be unpaused</param>
    public void SetPaused(bool paused)
    {
        // Set paused status
        this.paused = paused;
        Debug.LogFormat("Game " + (paused ? "paused" : "unpaused"));

        // Show/hide pause menu
        pauseMenu.SetActive(paused);

        //return to unmuffled after unpause
        if (!paused)
        {
            AkSoundEngine.SetState("Menu", "OutOfMenu"); //Set state to muffle music
            Debug.Log("Set State to OutOfMenu");
        }

        // Show/hide cursor when pausing/unpausing (respectively)
        if (paused)
            GameManager.SetCursorActive(true);
        else
            GameManager.SetCursorActive(false);

        // Update pause event listeners
        onSetGamePaused?.Invoke(paused);

        // Pause/resume game time based
        Time.timeScale = paused ? 0 : 1;
    }
}
