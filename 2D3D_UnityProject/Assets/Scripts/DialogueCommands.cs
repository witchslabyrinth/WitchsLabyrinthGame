using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class DialogueCommands : MonoBehaviour
{
    // Drag and drop your Dialogue Runner into this variable.
    [SerializeField]
    private DialogueRunner dialogueRunner;

    [SerializeField]
    private GameObject camera3;

    [SerializeField]
    private float angle = 10f;

    public void Awake() 
    {
        dialogueRunner.AddCommandHandler("op_camera_3", OpCamera3);
        dialogueRunner.AddCommandHandler("reset_input", ResetInput);
    }

    private void ResetInput(string[] parameters, System.Action onComplete)
    {
        Actor actor = PlayerController.Instance.GetPlayer();
        actor.ghostCamera.enabled = true;
        actor.enabled = true;
        GameManager.SetCursorActive(false);
        onComplete();
    }

    /// <summary>
    /// third camera action, map pan
    /// </summary>
    /// <param name="parameters"></param>
    /// <param name="onComplete"></param>
    private void OpCamera3(string[] parameters)
    {
        camera3.SetActive(true);
        StartCoroutine(wholeMapPan(camera3));
    }

    /// <summary>
    /// pans whole map
    /// </summary>
    /// <param name="camera"></param>
    /// <returns></returns>
    private IEnumerator wholeMapPan(GameObject camera)
    {
        while(true)
        {
            camera.transform.RotateAround(transform.position, Vector3.up, angle*Time.deltaTime);
            yield return null;
        }
    }
}
