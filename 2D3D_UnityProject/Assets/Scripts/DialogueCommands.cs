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
    private GameObject opCamera;

    [SerializeField]
    private Transform cameraLoc3;

    [SerializeField]
    private Transform cameraLoc4;

    [SerializeField]
    private float panToTempleTime = 3f;

    [SerializeField]
    private float angle = 10f;

    public void Awake() 
    {
        dialogueRunner.AddCommandHandler("op_camera_3", OpCamera3);
        dialogueRunner.AddCommandHandler("op_camera_4", OpCamera4);
        dialogueRunner.AddCommandHandler("reset_input", ResetInput);
    }

    private void ResetInput(string[] parameters, System.Action onComplete)
    {
        Actor actor = PlayerController.Instance.GetPlayer();
        actor.ghostCamera.gameObject.SetActive(true);
        actor.ghostCamera.enabled = true;
        actor.enabled = true;
        GameManager.SetCursorActive(false);
        ResetCoroutines();
        onComplete();
    }

    private void OpCamera3(string[] parameters)
    {
        opCamera.SetActive(true);
        PlayerController.Instance.GetPlayer().ghostCamera.gameObject.SetActive(false);
        ResetCoroutines();
        StartCoroutine("WholeMapPan");
    }

    private void OpCamera4(string[] parameters)
    {
        ResetCoroutines();
        StartCoroutine("PanToTemple");
    }

    /// <summary>
    /// pans whole map
    /// </summary>
    /// <param name="camera"></param>
    /// <returns></returns>
    private IEnumerator WholeMapPan()
    {
        while(true)
        {
            opCamera.transform.RotateAround(cameraLoc3.position, Vector3.up, angle*Time.deltaTime);
            yield return null;
        }
    }

    private IEnumerator PanToTemple()
    {
        Vector3 startPos = opCamera.transform.position;
        Quaternion startRot = opCamera.transform.rotation;
        for(float time = 0; time < panToTempleTime; time += Time.deltaTime)
        {
            float percentage = time / panToTempleTime;
            opCamera.transform.position = Vector3.Lerp(startPos, cameraLoc4.position, percentage);
            opCamera.transform.rotation = Quaternion.Lerp(startRot, cameraLoc4.rotation, percentage);
            yield return null;
        }

        opCamera.transform.position = cameraLoc4.position;
        opCamera.transform.rotation = cameraLoc4.rotation;
    }

    private void ResetCoroutines()
    {
        StopCoroutine("WholeMapPan");
        StopCoroutine("PanToTemple");
    }
}
