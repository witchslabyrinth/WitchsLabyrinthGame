using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZodiacPuzzle : MonoBehaviour
{
    public enum Direction {
        LEFT = 1,
        RIGHT = -1,
    }

    /// <summary>
    /// Ordered list of rotating disks, from outermost to innermost
    /// </summary>
    public List<RotateDisk> disks;

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

        currentDisk = disks[0];
    }

    // Update is called once per frame
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

    public void SwitchDisk(bool next)
    {
        // Get disk index
        int diskIndex = disks.IndexOf(currentDisk);

        try {
            if (next) {
                RotateDisk disk = disks[diskIndex + 1];
                currentDisk = disk;
            }
            else {
                RotateDisk disk = disks[diskIndex - 1];
                currentDisk = disk;
            }
        }
        catch(System.ArgumentOutOfRangeException ex) {
            return;
        }
    }

    public void RotateDisk()
    {
        // Turn disk left/right
        if (Input.GetKeyDown(KeyCode.A)) {
            currentDisk.Rotate(Direction.LEFT);
        }
        else if (Input.GetKeyDown(KeyCode.D)) {
            currentDisk.Rotate(Direction.RIGHT);
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

        Debug.LogWarningFormat("{0}: selected correct symbol!", name);
    }
}
