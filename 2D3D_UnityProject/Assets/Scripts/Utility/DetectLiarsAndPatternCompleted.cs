using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DoubleSlidingDoors))]
public class DetectLiarsAndPatternCompleted : MonoBehaviour
{
    bool active = true;

    [SerializeField]
    private LiarCommands myLiar;
    [SerializeField]
    private PatternPuzzle myPattern;

    private DoubleSlidingDoors myDoorClose;

    // Start is called before the first frame update
    void Start()
    {
        myDoorClose = GetComponent<DoubleSlidingDoors>();
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            if (myPattern.solved && myLiar.solved)
            {
                myDoorClose.Open();
                active = false;
            }
        }
    }
}
