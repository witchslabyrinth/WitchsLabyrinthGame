using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZodiacPuzzle : MonoBehaviour
{
    public enum Direction {
        LEFT = 1,
        RIGHT = -1
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
        }

        currentDisk = disks[0];
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A)) {
            currentDisk.Rotate(Direction.LEFT);
        }
        else if (Input.GetKeyDown(KeyCode.D)) {
            currentDisk.Rotate(Direction.RIGHT);
        }
    }
}
