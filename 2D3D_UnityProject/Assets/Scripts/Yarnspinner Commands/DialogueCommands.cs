using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Yarn.Unity;

public class DialogueCommands : MonoBehaviour
{
    // Drag and drop your Dialogue Runner into this variable.
    [SerializeField]
    private DialogueRunner dialogueRunner;

    private Coroutine currCoroutine;

    private void Awake()
    {
        dialogueRunner.AddCommandHandler("load_scene", LoadScene);
        dialogueRunner.AddCommandHandler("reset_camera", ResetCamera);
        dialogueRunner.AddCommandHandler("set_mouse_on", SetMouseActive);
    }

    private void ResetCamera(string[] parameters, System.Action onComplete)
    {
        Actor actor = PlayerController.Instance.GetPlayer();
        actor.Enable();
        CameraController.Instance.SetMainCamera(actor.actorCamera);
        PlayerController.Instance.canSwap = true;

        onComplete();
    }

    private void LoadScene(string[] parameters, System.Action onComplete)
    {
        string scene = parameters[0];
        SceneManager.LoadScene(scene);
        onComplete();
    }

    private void SetMouseActive(string[] parameters, System.Action onComplete)
    {
        bool isMouseOn = bool.Parse(parameters[0]);
        GameManager.SetCursorActive(isMouseOn);
        onComplete();
    }
}
