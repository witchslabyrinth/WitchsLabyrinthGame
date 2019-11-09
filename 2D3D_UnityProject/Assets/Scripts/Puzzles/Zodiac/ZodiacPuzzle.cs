using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZodiacPuzzle : MonoBehaviour
{
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
    /// Center piece that the disks rotate around: used for distance calculation when generating sprites on disks
    /// </summary>
    [SerializeField]
    public ZodiacCenter center;

    void Start()
    {
        // Initialize each disk
        foreach(ZodiacDisk disk in disks) {
            disk.Init(this);

            // Check solution each time a symbol is selected
            disk.selectedSymbol += CheckSolution;
        }

        // Set control to first (outermost) disk in puzzle
        currentDisk = disks[0];
        currentDisk.PieceOut();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W)) {
            SwitchDisk(false);
        }
        else if (Input.GetKeyDown(KeyCode.S)) {
            SwitchDisk(true);
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

        currentDisk.PieceIn();

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
            currentDisk.PieceOut();
        }
        catch(System.ArgumentOutOfRangeException ex) {
            return;
        }
    }

    /// <summary>
    /// Rotates disk clockwise/coutnerclockwise based on player input
    /// </summary>
    public void RotateDisk()
    {
        if (Input.GetKeyDown(KeyCode.D)) {
            currentDisk.Rotate(Direction.CLOCKWISE);
        }
        else if (Input.GetKeyDown(KeyCode.A)) {
            currentDisk.Rotate(Direction.COUNTER_CLOCKWISE);
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
        currentDisk.PieceIn();
        center.PieceOut();
    }
}
