using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
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

    void Update()
    {
        // Get reference to current actor (can change at runtime)
        Actor playerActor = PlayerController.Instance.GetActor();

        // Change camera perspective (and associated movement scheme)
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            // Update movement type
            playerActor.SetMovementType(new TopDownMovement());

            // Update camera perspective
            CameraController.Instance.SetToTopOrtho();
            playerActor.ghostCamera.enabled = false;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            // Update movement type
            playerActor.SetMovementType(new SideViewMovement());

            // Update camera perspective
            CameraController.Instance.SetToRightOrtho();
            playerActor.ghostCamera.enabled = false;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            // Update movement type
            // TODO: remove GhostCamera paramenter from constructor - can just get reference to it through Actor.ghostCamera
            playerActor.SetMovementType(new PerspectiveMovement(playerActor.ghostCamera.Camera));

            // Update camera perspective
            CameraController.Instance.SetToPerspective();
            playerActor.ghostCamera.enabled = true;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            // Update movement type
            playerActor.SetMovementType(new BackViewMovement());

            // Update camera perspective
            CameraController.Instance.SetToBackOrtho();
            playerActor.ghostCamera.enabled = false;
        }
    }
}