using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KoiFishPuzzle : Singleton<KoiFishPuzzle>
{
   
   [Header("Wwise")]
    /// <summary>
    /// Set Wwise variables
    /// </summary>
    /// <param name="paused">Set Wwise variables for sounds here</param>
    public AK.Wwise.Event Fish1;
    public AK.Wwise.Event FishWin;
    public AK.Wwise.Event FishLose;
    /// <summary>

   
   
   
    /// <summary>
    /// True if puzzle has been solved, false otherwise
    /// </summary>
    public bool solved { get; private set; }
    //n8-bit 2/11/2020
    /// <summary>
    /// This code activates the dev cheat to solve the Koi Fish puzzle upon pressing 9
    /// </summary>
    public void cheat_it_up()
    {
        //For each fish in the order list, feed it.
        foreach(KoiFish i in koiFishFeedingOrder)
        {
            Debug.Log("Dev cheat activated! Feeding Fish.");
            FeedFish(i);
        }
    }
    //End n8-bit 2/11/2020


    /// <summary>
    /// Proper order to feed the fish in (may not include all fish in the pond).
    /// </summary>
    [SerializeField] private List<KoiFish> koiFishFeedingOrder;

    /// <summary>
    /// Next fish the player must feed to solve the puzzle.
    /// </summary>
    private KoiFish nextFishToFeed;

    /// <summary>
    /// Number of next fish to be fed
    /// </summary>
    private int currentFishNumber
    {
        get { return koiFishFeedingOrder.IndexOf(nextFishToFeed) + 1; }
    }

    /// <summary>
    /// Amount of time remaining to feed the fish - if this reaches 0 the puzzle is reset
    /// </summary>
    private float timeRemaining = 0f;

    void Start()
    {
        solved = false;

        // Make sure there's a proper feeding order specified
        if (koiFishFeedingOrder == null)
            Debug.LogErrorFormat("{0} | Error: correct fish-feeding order not specified. Please specify this order in {0}.koiFishFeedingOrder", name);

        // Set puzzle to initial state
        nextFishToFeed = koiFishFeedingOrder[0];
    }

    /// <summary>
    /// Sets timer to 0 and clears all puzzle progress
    /// </summary>
    private void ResetPuzzle()
    {
        nextFishToFeed = koiFishFeedingOrder[0];
        timeRemaining = 0f;

        // If puzzle still running, disable trails on all fish
        if(!solved) {
            foreach(KoiFish fish in koiFishFeedingOrder)
                fish.SetTrailActive(false);
            
            // Play failure sound effect
            FishLose.Post(gameObject);
        }
    }

    private void Update()
    {
        // Decrement timer if puzzle is active
        if (PuzzleActive())
        {
            timeRemaining -= Time.deltaTime;

            // Reset puzzle if time runs out
            if (timeRemaining < 0)
            {
                Debug.Log("Time ran out, resetting puzzle");
                ResetPuzzle();
            }
            // Log time remaining to console
            else
            {
                string timeStr = $"{timeRemaining:0.00}";
                //Debug.Log("Time remaining: " + timeStr);
            }
        }

        if(Input.GetKeyDown(KeyCode.I))
            FeedFish(koiFishFeedingOrder[0]);

        if (Input.GetKeyDown(KeyCode.O))
            FeedFish(koiFishFeedingOrder[1]);
        
        if (Input.GetKeyDown(KeyCode.P))
            FeedFish(koiFishFeedingOrder[2]);

        //n8-bit 2/11/2020
        //If we're in the editor and press 9, run the dev cheat.
        if (Input.GetKeyDown(KeyCode.Alpha9) && Application.isEditor)
        {
            cheat_it_up();
        }
        //end n8-bit 2/11/2020
    }

    /// <summary>
    /// Updates puzzle state based on time/order in which this fish was fed
    /// </summary>
    /// <param name="fish">Fish fed by player</param>
    public void FeedFish(KoiFish fish)
    {
        Debug.LogFormat("Feeding fish {0}", fish.name);

        // Feed fish (pass order it was fed in)
        fish.Feed(currentFishNumber);
        Fish1.Post(gameObject); //Wwise

        // Reset puzzle if player fed wrong fish
        if (fish != nextFishToFeed)
        {
            Debug.Log("Fed wrong fish - resetting puzzle");
            
            ResetPuzzle();
            return;
        }

        // Check if player fed last fish in the order
        int index = koiFishFeedingOrder.IndexOf(fish);
        if (index == koiFishFeedingOrder.Count-1)
        {
            // TODO: handle end-of-puzzle logic here
            Debug.LogWarning("You solved the puzzle!");
            solved = true;
            FishWin.Post(gameObject); //Wwise
            ResetPuzzle();
        }

        // Otherwise assign the next fish in order
        else
        {
            nextFishToFeed = koiFishFeedingOrder[index + 1];
            Debug.Log("Next fish: " + nextFishToFeed.name);

            // Set timer for this fish
            timeRemaining = nextFishToFeed.feedDuration;
        }
    }

    /// <summary>
    /// Returns true if player is in the middle of solving this puzzle (i.e. timer counting down to feed next fish), false otherwise
    /// </summary>
    public bool PuzzleActive()
    {
        return timeRemaining > 0 && nextFishToFeed != koiFishFeedingOrder[0];
    }
}