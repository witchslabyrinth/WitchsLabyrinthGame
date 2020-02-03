// currently active on scene start even when player is not using it
// to fix, going to enable/disable this script based on whether the player is directly interacting with it

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZodiacPuzzle : MonoBehaviour
{
    [Header("Wwise")]
    /// <summary>
    /// Set Wwise variables
    /// </summary>
    /// <param name="paused">Set Wwise variables for sounds here</param>
    public AK.Wwise.Event stoneMove;
    public AK.Wwise.Event doorMove;

    /// <summary>
    /// The number of rounds that must be solved in order to complete the puzzle
    /// </summary>
    public const int numberOfRounds = 3;

    /// <summary>
    /// Direction used for disk rotation
    /// </summary>
    public enum Direction {
        CLOCKWISE = 1,
        COUNTER_CLOCKWISE = -1,
    }

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

    [SerializeField]
    private GameObject zodCamera;

    //more probably bad stuff
    //public PlayerController player;

    public GameObject mainCam;

    public GameObject zodiacCanvas;

    void Start()
    {
        // Initialize each disk
        foreach(ZodiacDisk disk in disks) {
            // moving this to ZodiacDisk's Start function
            // disk.Init(this);

            // Check solution each time a symbol is selected
            disk.selectedSymbol += CheckSolution;
        }

        // Set control to first (outermost) disk in puzzle
        currentDisk = disks[0];
        // currentDisk.PieceInOut(ZodiacPuzzlePiece.ZodiacPuzzlePiecePosition.Out);

        currentRound = 1;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W)) 
        {
            SwitchDisk(false);
        }
        else if (Input.GetKeyDown(KeyCode.S)) 
        {
            SwitchDisk(true);
        }

        //TODO: Remove this when it's no longer necessary
        if (Input.GetKeyDown(KeyCode.R))
        {
            // for ukiyoe scene
            // mainCam.SetActive(true);

            // Restore control to player actor
            Actor actor = PlayerController.Instance.GetPlayer();
            actor.Enable();

            // Restore actor swapping
            PlayerController.Instance.canSwap = true;

            zodCamera.SetActive(false);
            this.enabled = false;
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

        try {
            // Select next disk (moving towards center)
            if (next) {
                ZodiacDisk disk = disks[diskIndex + 1];
                currentDisk = disk;
            }
            // Select previous disk (moving away from center)
            else {
                ZodiacDisk disk = disks[diskIndex - 1];
                currentDisk = disk;
            }
            // currentDisk.PieceInOut(ZodiacPuzzlePiece.ZodiacPuzzlePiecePosition.Out);
        }
        catch(System.ArgumentOutOfRangeException ex) {
            return;
        }

        disks[diskIndex].PieceInOut(ZodiacPuzzlePiece.ZodiacPuzzlePiecePosition.In);
    }

    /// <summary>
    /// Rotates disk clockwise/coutnerclockwise based on player input
    /// </summary>
    public void RotateDisk()
    {
        if (Input.GetKeyDown(KeyCode.D)) {
            currentDisk.Rotate(Direction.CLOCKWISE);
            stoneMove.Post(gameObject); //Wwise
        }
        else if (Input.GetKeyDown(KeyCode.A)) {
            currentDisk.Rotate(Direction.COUNTER_CLOCKWISE);
            stoneMove.Post(gameObject); //Wwise

        }
    }

    private void CheckSolution()
    {
        // Make sure each disk has the correct symbol selected
        foreach(ZodiacDisk disk in disks) {
            if(!disk.Correct()) {
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

    private void PuzzleSolved()
    {
        // TODO: Maybe disable to center coming out now that we have lights
        // currentDisk.PieceInOut(ZodiacPuzzlePiece.ZodiacPuzzlePiecePosition.In);
        // center.PieceInOut(ZodiacPuzzlePiece.ZodiacPuzzlePiecePosition.Out);
        zodiacDoor.Open();
        doorMove.Post(gameObject); //Wwise
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
