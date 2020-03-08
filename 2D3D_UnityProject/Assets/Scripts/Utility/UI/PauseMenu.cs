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
    public AK.Wwise.Event OnMenuHover;
    public AK.Wwise.Event OnMenuSelect;
    public AK.Wwise.Event OnMenuEnter;
    public AK.Wwise.Event OnMenuExit;


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

    [Header("Screens")]
    /// <summary>
    /// UI element containing pause menu contents
    /// </summary>
    [SerializeField] private Graphic pauseMenu;

    /// <summary>
    /// UI element containing controls
    /// </summary>
    [SerializeField] private Graphic controlsMenu;

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
    /// Player presses this to open controls menu
    /// </summary>
    [SerializeField] private Button controlsButton;

    /// <summary>
    /// Player presses this to quit game
    /// </summary>
    [SerializeField] private Button quitButton;

    // Start is called before the first frame update
    void Start()
    {
        // Set up button events
        InitializeButtons();
        
        // Set up game pause/unpause events
        onSetGamePaused += (paused =>
        {
            // Lock/Unlock camera movement when pausing/unpausing
            CameraController.Instance.enabled = !paused;

            // Hide controls menu (whether pausing or unpausing)
            SetControlsMenuActive(false);

            // Muffle/unmuffle music on pause/unpause
          
                
        });

        // Start with the game unpaused
        SetPaused(false);
    }

    /// <summary>
    /// Sets up button events
    /// </summary>
    private void InitializeButtons()
    {
        // Resume button unpauses game
        resumeButton.onClick.AddListener(() => SetPaused(false));

        // Quit button returns to main menu
        quitButton.onClick.AddListener(() =>
        {
            // Restore timescale to 1
            Time.timeScale = 1;


            // Load MainMenu scene
            SceneLoader.LoadScene(SCENE_ID.MAIN_MENU);
        });
        
        // Controls button shows controls menu
        controlsButton.onClick.AddListener(() => SetControlsMenuActive(true));
    }

    /// <summary>
    /// Pauses/unpauses game
    /// </summary>
    public void TogglePaused()
    {
        SetPaused(!paused);

        // TODO: do the OnMenuExit() sound too
        
    }

    /// <summary>
    /// Sets game to paused/unpaused
    /// </summary>
    /// <param name="paused">If true game will be paused, if false game will be unpaused</param>
    public void SetPaused(bool paused)
    {
        // Set paused status
        this.paused = paused;

        // Show/hide pause menu
        pauseMenu.gameObject.SetActive(paused);

        // Show/hide cursor when pausing/unpausing (respectively)
        GameManager.SetCursorActive(paused);

        // Update pause event listeners
        onSetGamePaused?.Invoke(paused);

        // Pause/resume game time
        Time.timeScale = paused ? 0 : 1;
    }

    /// <summary>
    /// Shows/hides controls menu
    /// </summary>
    /// <param name="active">Shows menu if true, hides if false</param>
    private void SetControlsMenuActive(bool active)
    {
        controlsMenu.gameObject.SetActive(active);
    }
}
