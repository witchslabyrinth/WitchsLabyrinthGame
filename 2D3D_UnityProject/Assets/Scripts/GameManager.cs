using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private CameraController camControl;

    [SerializeField]
    private PerspectiveCameraControl perspControl;

    /// <summary>
    /// Used for controlling player actor
    /// </summary>
    private PlayerController playerControl;

    public static GameManager instance = null;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        playerControl = PlayerController.Instance;
    }

    void Update()
    {
        // Get reference to current actor (can change at runtime)
        Actor playerActor = playerControl.GetActor();

        // Change camera perspective (and associated movement scheme)
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            // Update movement type
            playerActor.SetMovementType(new TopDownMovement());

            // Update camera
            camControl.SetToTopOrtho();
            perspControl.enabled = false;

            // Update Top View
            playerActor.SetTopView(true);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            // Update movement type
            playerActor.SetMovementType(new SideViewMovement());

            // Update camera
            camControl.SetToRightOrtho();
            perspControl.enabled = false;

            // Update Top View
            playerActor.SetTopView(false);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            // Update movement type
            playerActor.SetMovementType(new PerspectiveMovement(perspControl.Camera));

            // Update camera
            camControl.SetToPerspective();
            perspControl.enabled = true;

            // Update Top View
            playerActor.SetTopView(false);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            // Update movement type
            playerActor.SetMovementType(new BackViewMovement());

            // Update camera
            camControl.SetToBackOrtho();
            perspControl.enabled = false;

            // Update Top View
            playerActor.SetTopView(false);
        }
    }
}