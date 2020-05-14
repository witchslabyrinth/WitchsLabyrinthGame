using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiverBridge : MonoBehaviour
{
    private KoiFishPuzzle koiFishPuzzle => KoiFishPuzzle.Instance;

    /// <summary>
    /// Bridges allowing player to cross river
    /// </summary>
    [SerializeField]
    private GameObject bridges;

    /// <summary>
    /// Colliders blocking player from crossing river before bridges appear
    /// </summary>
    [SerializeField]
    private GameObject blockingColliders;

    private void Start()
    {
        // Unlock bridge when puzzle is solved
        koiFishPuzzle.onSolved += () => SetRiverPassage(true);

        // Block river passage until player solves puzzle
        SetRiverPassage(false);
    }

    private void SetRiverPassage(bool open)
    {
        // Allow/block river passage
        bridges.SetActive(open);
        blockingColliders.SetActive(!open);
    }
}
