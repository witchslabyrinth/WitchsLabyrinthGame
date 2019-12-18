// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class PerspectiveManager : MonoBehaviour {
//     public static PerspectiveManager instance = null;

//     public GameObject mainCamera;
//     public GameObject ghostCamera;
//     public GameObject player;

//     protected CameraController camControl;
//     protected PlayerController playerControl;
//     protected PerspectiveCameraControl perspControl;

//     void Awake () {
//         if (instance == null) {
//             instance = this;
//         } else if (instance != this) {
//             Destroy (gameObject);
//         }
//         DontDestroyOnLoad (gameObject);
//     }

//     void Start () {
//         camControl = mainCamera.GetComponent<CameraController> ();
//         playerControl = player.GetComponent<PlayerController> ();
//         perspControl = ghostCamera.GetComponent<PerspectiveCameraControl> ();
//     }

//     void Update () {
//         if (Input.GetKeyDown ("up")) {
//             camControl.SetToTopOrtho ();
//             playerControl.constrainZ (false);
//             playerControl.constrainX (false);
//             playerControl.constrainY (true);
//             perspControl.enabled = false;
//         } else if (Input.GetKeyDown ("right")) {
//             camControl.SetToRightOrtho ();
//             playerControl.constrainX (true);
//             playerControl.constrainZ (false);
//             playerControl.constrainY (false);
//             perspControl.enabled = false;
//         } else if (Input.GetKeyDown ("left")) {
//             camControl.SetToPerspective ();
//             perspControl.enabled = true;
//             playerControl.constrainZ (false);
//             playerControl.constrainX (false);
//             playerControl.constrainY (false);
//         } else if (Input.GetKeyDown ("down")) {
//             camControl.SetToBackOrtho ();
//             playerControl.constrainZ (true);
//             playerControl.constrainX (false);
//             playerControl.constrainY (false);
//             perspControl.enabled = false;
//         }
//     }

// }