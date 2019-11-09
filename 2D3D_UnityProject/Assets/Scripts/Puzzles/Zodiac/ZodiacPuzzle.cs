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
    public List<RotateDisk> disks;

    /// <summary>
    /// Disk currently being controlled by the player
    /// </summary>
    private RotateDisk currentDisk;

    /// <summary>
    /// Center piece that the disks rotate around: used for distance calculation when generating sprites on disks
    /// </summary>
    [SerializeField]
    public GameObject center;

    void Start()
    {
        // Initialize each disk
        foreach(RotateDisk disk in disks) {
            disk.Init(this);

            // Check solution each time a symbol is selected
            disk.selectedSymbol += CheckSolution;
        }

        // Set control to first (outermost) disk in puzzle
        currentDisk = disks[0];
        currentDisk.DiskOut();
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

        currentDisk.DiskIn();

        try {
            // Select next disk (moving towards center)
            if (next) {
                RotateDisk disk = disks[diskIndex + 1];
                currentDisk = disk;
            }
            // Select previous disk (moving away from center)
            else {
                RotateDisk disk = disks[diskIndex - 1];
                currentDisk = disk;
            }
            currentDisk.DiskOut();
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
        foreach(RotateDisk disk in disks) {
            if(!disk.Correct()) {
                return;
            }
        }

        // TODO: figure out what happens next lol
        Debug.LogWarningFormat("{0}: selected correct symbol!", name);
    }
}
