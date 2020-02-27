using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Yarn.Unity;

public class DialogueCommands : MonoBehaviour
{
    // Drag and drop your Dialogue Runner into this variable.
    [SerializeField]
    protected DialogueRunner dialogueRunner;

    protected Coroutine currCoroutine;

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
        string sceneName = "";
        for (int i = 0; i < parameters.Length; i++)
        {
            sceneName += parameters[i];
            if (i < parameters.Length-1)
            {
                sceneName += " ";
            }
        }
        SceneManager.LoadScene(sceneName);
        onComplete();
    }

    private void SetMouseActive(string[] parameters, System.Action onComplete)
    {
        bool isMouseOn = bool.Parse(parameters[0]);
        GameManager.SetCursorActive(isMouseOn);
        onComplete();
    }

    protected IEnumerator PanCamera(Transform camera, Transform target, float panTime)
    {
        Vector3 startPos = camera.position;
        Quaternion startRot = camera.rotation;
        for (float time = 0; time < panTime; time += Time.deltaTime)
        {
            float percentage = time / panTime;
            camera.position = Vector3.Lerp(startPos, target.position, percentage);
            camera.rotation = Quaternion.Lerp(startRot, target.rotation, percentage);
            yield return null;
        }
        camera.position = target.position;
        camera.rotation = target.rotation;
    }

    protected IEnumerator PanCamera(Transform camera, Transform target, float panTime, System.Action onComplete)
    {
        Vector3 startPos = camera.position;
        Quaternion startRot = camera.rotation;
        for (float time = 0; time < panTime; time += Time.deltaTime)
        {
            float percentage = time / panTime;
            camera.position = Vector3.Lerp(startPos, target.position, percentage);
            camera.rotation = Quaternion.Lerp(startRot, target.rotation, percentage);
            yield return null;
        }
        camera.position = target.position;
        camera.rotation = target.rotation;
        onComplete();
    }
}
