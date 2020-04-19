// currently active on scene start even when player is not using it
// to fix, going to enable/disable this script based on whether the player is directly interacting with it

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Yarn.Unity;

public class ZodiacPuzzle : MonoBehaviour
{

    [Header("Wwise")]
    /// <summary>
    /// Set Wwise variables
    /// </summary>
    /// <param name="paused">Set Wwise variables for sounds here</param>
    public AK.Wwise.Event stoneMove;
    public AK.Wwise.Event doorMove;
    public AK.Wwise.Event puzzleSolved;


    [Header("Camera Settings")]

    /// <summary>
    /// Camera that shows doors opening when puzzle is solved
    /// </summary>
    [SerializeField]
    private CameraEntity doorsOpenCamera;


    /// <summary>
    /// The number of rounds that must be solved in order to complete the puzzle
    /// </summary>
    public const int numberOfRounds = 3;

    /// <summary>
    /// Direction used for disk rotation
    /// </summary>
    public enum Direction
    {
        CLOCKWISE = 1,
        COUNTER_CLOCKWISE = -1,
    }
    [Header("Puzzle Settings")]

    /// <summary>
    /// Ordered list of rotating disks, from outermost to innermost
    /// </summary>
    public List<ZodiacDisk> disks;

    /// <summary>
    /// Disk currently being controlled by the player
    /// </summary>
    private ZodiacDisk currentDisk;

    /// <summary>
    /// The current round the puzzle is in. Dictates what the solution currently is.
    /// </summary>
    public int currentRound { get; private set; }

    /// <summary>
    /// Center piece that the disks rotate around: used for distance calculation when generating sprites on disks
    /// </summary>
    [SerializeField]
    public ZodiacCenter center;

    /// <summary>
    /// List of lights associated with this puzzle to be enabled when the puzzle is solved
    /// </summary>
    [SerializeField]
    private List<ZodiacLight> zodiacLights;

    /// <summary>
    /// Door to be opened when the zodiac puzzle is solved
    /// </summary>
    [SerializeField]
    private DoubleSlidingDoors zodiacDoor;

    /// <summary>
    /// Indicates whether player has solved the puzzle
    /// </summary>
    public bool solved = false;

    public GameObject zodiacCanvas;

    // TODO: find a better place for this
    [SerializeField]
    private YarnProgram scene2;
    [SerializeField]
    private CameraEntity scene2Cam;

    void Start()
    {
        // Initialize each disk
        foreach (ZodiacDisk disk in disks)
        {
            // moving this to ZodiacDisk's Start function
            // disk.Init(this);

            // Check solution each time a symbol is selected
            disk.selectedSymbol += CheckSolution;
        }

        // Set control to first (outermost) disk in puzzle
        currentDisk = disks[0];
        currentDisk.GlowCurrentSymbol(true);
        // currentDisk.PieceInOut(ZodiacPuzzlePiece.ZodiacPuzzlePiecePosition.Out);

        currentRound = 1;


        DialogueRunner dialogueRunner = FindObjectOfType<Yarn.Unity.DialogueRunner>();
        dialogueRunner.Add(scene2);
    }

    void Update()
    {
        //If we're in the editor and press 0, run the dev cheat
        if (Input.GetKeyDown(KeyCode.Alpha0) && Application.isEditor)
        {
            PuzzleSolved();
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            SwitchDisk(false);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            SwitchDisk(true);
        }

        //TODO: Move this to a generalized interaction script
        if (Input.GetKeyDown(KeyCode.E))
        {
            ExitPuzzle();
        }

        RotateDisk();
    }

    /// <summary>
    /// Switch control between disks. The "next" disk is the one closer to the center
    /// </summary>
    /// <param name="next"></param>
    public void SwitchDisk(bool next)
    {
        // Get disk index
        int diskIndex = disks.IndexOf(currentDisk);
        currentDisk.GlowCurrentSymbol(false);

        try
        {
            // Select next disk (moving towards center)
            if (next)
            {
                ZodiacDisk disk = disks[diskIndex + 1];
                currentDisk = disk;
            }
            // Select previous disk (moving away from center)
            else
            {
                ZodiacDisk disk = disks[diskIndex - 1];
                currentDisk = disk;
            }
            // currentDisk.PieceInOut(ZodiacPuzzlePiece.ZodiacPuzzlePiecePosition.Out);
        }
        catch (System.ArgumentOutOfRangeException ex)
        {
            return;
        }

        currentDisk.GlowCurrentSymbol(true);
        disks[diskIndex].PieceInOut(ZodiacPuzzlePiece.ZodiacPuzzlePiecePosition.In);
    }

    /// <summary>
    /// Rotates disk clockwise/coutnerclockwise based on player input
    /// </summary>
    public void RotateDisk()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            currentDisk.Rotate(Direction.CLOCKWISE);
            stoneMove.Post(gameObject); //Wwise
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            currentDisk.Rotate(Direction.COUNTER_CLOCKWISE);
            stoneMove.Post(gameObject); //Wwise

        }
    }

    private void CheckSolution()
    {
        // Make sure each disk has the correct symbol selected
        foreach (ZodiacDisk disk in disks)
        {
            if (!disk.Correct())
            {
                return;
            }
        }

        // TODO: figure out what happens next lol
        Debug.LogWarningFormat("{0}: selected correct symbol!", name);

        // Turn on light according to what round was completed
        zodiacLights[currentRound - 1].TurnOn();
        if (currentRound < numberOfRounds)
        {
            // Switch back to top disk
            while (disks.IndexOf(currentDisk) > 0)
            {
                SwitchDisk(false);
            }
            currentRound++;
        }
        else
        {
            PuzzleSolved();
        }
    }

    /// <summary>
    /// Opens doors and restores control to player
    /// </summary>
    private void PuzzleSolved()
    {
        // Make sure we haven't already solved it before (shouldn't be possible)
        if (solved)
            return;

        // TODO: maybe a smooth camera pan from puzzle view to doors
        // Show camera view of doors opening
        CameraController cameraController = CameraController.Instance;
        cameraController.SetMainCamera(doorsOpenCamera);

        // Switch back to player actor when doors finished opening
        zodiacDoor.onFinishedOpening += () =>
        {
            FindObjectOfType<DialogueRunner>().StartDialogue("SceneAfterZodiac");
            CameraController.Instance.SetMainCamera(scene2Cam);
        };

        // Open doors
        zodiacDoor.Open();
        puzzleSolved.Post(gameObject); //Wwise
        doorMove.Post(gameObject); //Wwise
        solved = true;

        // Disable puzzle
        enabled = false;
    }


    /// <summary>
    /// Exits puzzle and restores control to player
    /// </summary>
    private void ExitPuzzle()
    {
        // Restore control to player actor
        Actor actor = PlayerController.Instance.GetPlayer();
        actor.Enable();
        CameraController.Instance.SetMainCamera(actor.actorCamera);

        // Restore actor swapping
        PlayerController.Instance.canSwap = true;

        // Disable the puzzle
        this.enabled = false;
    }

    void OnEnable()
    {
        zodiacCanvas.SetActive(true);
        //AkSoundEngine.SetState("Interaction", "Interacting"); //Set state to muffle music

    }

    void OnDisable()
    {
        zodiacCanvas.SetActive(false);
        //AkSoundEngine.SetState("Interaction", "NotInteracting"); //Set state to re-enable music

    }
}
