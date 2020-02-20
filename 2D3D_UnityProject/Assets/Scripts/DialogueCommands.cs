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

    [SerializeField]
    private GameObject opCamera;

    [SerializeField]
    private Transform cameraLoc1;

    [SerializeField]
    private Transform cameraLoc3;

    [SerializeField]
    private Transform cameraLoc3Center;

    [SerializeField]
    private Transform cameraLoc4;

    [SerializeField]
    private float panToCatTime = 2f;

    [SerializeField]
    private float panToTempleTime = 2f;

    [SerializeField]
    private float angle = 10f;

    private Coroutine currCoroutine;

    public void Awake()
    {
        dialogueRunner.AddCommandHandler("op_camera_1", OpCamera1);
        dialogueRunner.AddCommandHandler("op_camera_3", OpCamera3);
        dialogueRunner.AddCommandHandler("op_camera_4", OpCamera4);
        dialogueRunner.AddCommandHandler("load_scene", LoadScene);
        dialogueRunner.AddCommandHandler("reset_camera", ResetCamera);
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
        SceneManager.LoadScene("Ukiyo-e Environment");
        onComplete();
    }

    private void OpCamera1(string[] parameters, System.Action onComplete)
    {
        opCamera.transform.position = PlayerController.Instance.GetPlayer().ghostCamera.transform.position;
        opCamera.transform.rotation = PlayerController.Instance.GetPlayer().ghostCamera.transform.rotation;
        opCamera.SetActive(true);

        PlayerController.Instance.GetPlayer().ghostCamera.gameObject.SetActive(false);
        currCoroutine = StartCoroutine(PanToCat(onComplete));
    }

    private void OpCamera3(string[] parameters)
    {
        StopCoroutine(currCoroutine);
        opCamera.transform.position = cameraLoc3.position;
        opCamera.transform.rotation = cameraLoc3.rotation;
        currCoroutine = StartCoroutine(WholeMapPan());
    }

    private void OpCamera4(string[] parameters)
    {
        StopCoroutine(currCoroutine);
        currCoroutine = StartCoroutine(PanToTemple());
    }

    private IEnumerator PanToCat(System.Action onComplete)
    {
        Vector3 startPos = opCamera.transform.position;
        Quaternion startRot = opCamera.transform.rotation;
        for (float time = 0; time < panToCatTime; time += Time.deltaTime)
        {
            float percentage = time / panToCatTime;
            opCamera.transform.position = Vector3.Lerp(startPos, cameraLoc1.position, percentage);
            opCamera.transform.rotation = Quaternion.Lerp(startRot, cameraLoc1.rotation, percentage);
            yield return null;
        }
        opCamera.transform.position = cameraLoc1.position;
        opCamera.transform.rotation = cameraLoc1.rotation;
        onComplete();
    }

    private IEnumerator WholeMapPan()
    {
        while (true)
        {
            opCamera.transform.RotateAround(cameraLoc3Center.position, Vector3.up, angle * Time.deltaTime);
            yield return null;
        }
    }

    private IEnumerator PanToTemple()
    {
        Vector3 startPos = opCamera.transform.position;
        Quaternion startRot = opCamera.transform.rotation;
        for (float time = 0; time < panToTempleTime; time += Time.deltaTime)
        {
            float percentage = time / panToTempleTime;
            opCamera.transform.position = Vector3.Lerp(startPos, cameraLoc4.position, percentage);
            opCamera.transform.rotation = Quaternion.Lerp(startRot, cameraLoc4.rotation, percentage);
            yield return null;
        }

        opCamera.transform.position = cameraLoc4.position;
        opCamera.transform.rotation = cameraLoc4.rotation;
    }
}
