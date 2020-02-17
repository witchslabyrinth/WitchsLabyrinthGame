using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;


public class PlayerController : Singleton<PlayerController>
{
    [Header("Wwise")]
    /// <summary>
    /// Set Wwise variables
    /// </summary>
    /// <param name="paused">Set Wwise variables for sounds here</param>
    public AK.Wwise.Event oliverSwitchSound;
    public AK.Wwise.Event catSwitchSound;
    public AK.Wwise.Event oliverStaySound;
    public AK.Wwise.Event catStaySound;

    // TODO: see if there's a better way (maybe event-driven?) to handle this
    /// <summary>
    /// True when player can swap between actors, false when swapping is disabled
    /// </summary>
    public bool canSwap = true;

    /// <summary>
    /// Reference to Oliver
    /// </summary>
    [SerializeField]
    private Actor oliver;

    /// <summary>
    /// Reference to Cat
    /// </summary>
    [SerializeField]
    private Actor cat;

    /// <summary>
    /// Actor currently controlled by the player
    /// </summary>
    [SerializeField]
    private Actor player;

    /// <summary>
    /// Actor not currently controlled by the player
    /// </summary>
    private Actor friend
    {
        get
        {
            if (player.Equals(oliver))
                return cat;
            else if (player.Equals(cat))
                return oliver;

            // Return null if player actor undefined
            else
            {
                Debug.LogWarningFormat("{0} | Friend actor undefined", name);
                return null;
            }
        }
        set { }
    }

    private void Awake()
    {
        // Make sure we have an actor (default to oliver if not specified)
        if(player == null) {
            player = oliver;
            friend = cat;
        }

        // Throw warnings and disable swapping if oliver/cat not found
        if (!oliver)
        {
            Debug.LogWarning("Warning: Oliver actor not found in PlayerController");
            canSwap = false;
        }
        if (!cat)
        {
            Debug.LogWarning("Warning: Cat actor not found in PlayerController");
            canSwap = false;
        }

        if (oliver && cat)
        {
            // Ignore collision between oliver and cat
            Collider oliverCollider, catCollider;
            if (oliver.TryGetComponent(out oliverCollider) && cat.TryGetComponent(out catCollider))
            {
                Physics.IgnoreCollision(oliverCollider, catCollider);
            }
        }
        // making sure everything's loaded before starting opening cutscene
        // can probably be deleted later
        StartCoroutine(DialInput());
    }

    private void Update()
    {
        // Toggle paused/unpaused on ESC
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseMenu.Instance.TogglePaused();
        }
        
        // Only allow actor control if game is unpaused
        if (!PauseMenu.Instance.paused)
        {
            // Toggle currently-controlled actor between oliver/cat
            if(canSwap && Input.GetKeyDown(KeyCode.Tab))
            {
                Swap();
                if (player.Equals(oliver))
                    oliverSwitchSound.Post(gameObject); //Play Oliver switch sound
                else if (player.Equals(cat))
                    catSwitchSound.Post(gameObject); //Play Cat switch sound
            }

            // Input for telling Friend actor to follow/stay
            if (Input.GetKeyDown(KeyCode.F))
            {
                FriendCommand();
                if (player.Equals(oliver))
                    catStaySound.Post(gameObject); //Play Cat stay sound
                else if (player.Equals(cat))
                    oliverStaySound.Post(gameObject); //Play Oliver stay sound
            }

            // Handle interactions with other game entities
            player.CheckInteraction();
    
            // Update camera perspective
            PerspectiveController.Instance.UpdatePerspective(player);

            // Update camera to follow player actor
            CameraController.Instance.CameraUpdate(player);
        }
    }

    /// <summary>
    /// Switches player control to Friend actor
    /// </summary>
    private void Swap()
    {
        // Swap player/friend actors
        Actor temp = player;
        player = friend;
        friend = temp;

        // Copy the friend follow/idle movement over to the new actor
        friend.SetMovement(player.movement);

        // Restore player actor's previous perspective
        PerspectiveController.Instance.SetPerspective(player, player.perspective);
    }

    /// <summary>
    /// Toggles friend actor's movement between following player and staying idle
    /// </summary>
    private void FriendCommand()
    {
        // Switch to idle
        if (friend.movement.GetType() == typeof(FollowMovement))
        {
            Debug.LogFormat("Switching {0} movement from Follow to Idle", friend.name);
            friend.SetMovement(new NullMovement());
        }
        // Switch to follow
        else
        {
            Debug.LogFormat("Switching {0} movement from Idle to Follow", friend.name);
            friend.SetMovement(new FollowMovement(player.transform));
        }
    }

    /// <summary>
    /// Returns reference to currently-controlled Actor (the player)
    /// </summary>
    public Actor GetPlayer()
    {
        return player;
    }

    public Actor GetFriend()
    {
        return friend;
    }

    private IEnumerator DialInput()
    {
        for(float i = 0; i < 1; i += Time.deltaTime)
        {
            yield return null;
        }
        Actor actor = PlayerController.Instance.GetPlayer();
        actor.ghostCamera.enabled = false;
        actor.enabled = false;
        GameManager.SetCursorActive(true);
        FindObjectOfType<DialogueRunner>().StartDialogue("OpeningScene");
    }
}