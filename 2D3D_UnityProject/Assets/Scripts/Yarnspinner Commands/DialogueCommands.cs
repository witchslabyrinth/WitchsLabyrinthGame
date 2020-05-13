using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Yarn.Unity;
using UnityEngine.UI;

public class DialogueCommands : MonoBehaviour
{
    // Drag and drop your Dialogue Runner into this variable.
    [SerializeField]
    protected DialogueRunner dialogueRunner;

    [SerializeField]
    private Image portrait;

    [System.Serializable]
    public struct SpriteInfo
    {
        public string name;
        public Sprite sprite;
    }

    private List<SpriteInfo> talkingSprites;

    protected Coroutine currCoroutine;

    [SerializeField]
    private string talkingSpriteFolder;

    [SerializeField]
    protected GameObject oliverSprite;

    [SerializeField]
    protected GameObject catSprite;

    /// <summary>
    /// add all commands to DialogueRunner
    /// </summary>
    private void Awake()
    {
        dialogueRunner.AddCommandHandler("change_portrait", ChangeTalkingSprite);
        dialogueRunner.AddCommandHandler("load_scene", LoadScene);
        dialogueRunner.AddCommandHandler("reset_camera", ResetCamera);
        dialogueRunner.AddCommandHandler("set_mouse_on", SetMouseActive);
        dialogueRunner.AddCommandHandler("show_sprites", ShowSprites);
        dialogueRunner.AddCommandHandler("enable_control", EnableControl);

        talkingSprites = new List<SpriteInfo>();
        LoadSprites(talkingSpriteFolder);
    }

    private void ChangeTalkingSprite(string[] parameters, System.Action onComplete)
    {
        string spriteName = "";
        for (int i = 0; i < parameters.Length; i++)
        {
            spriteName += parameters[i];
            if (i < parameters.Length - 1)
            {
                spriteName += " ";
            }
        }

        Sprite newPortrait = null;
        foreach (SpriteInfo talkSprite in talkingSprites)
        {
            if (talkSprite.name == spriteName)
            {
                newPortrait = talkSprite.sprite;
                break;
            }
        }
        if (newPortrait == null)
        {
            Debug.LogErrorFormat("Can't find sprite named {0}!", spriteName);
            return;
        }

        portrait.sprite = newPortrait;
        onComplete();
    }

    /// <summary>
    /// reset to player control
    /// </summary>
    /// <param name="parameters"></param>
    /// <param name="onComplete"></param>
    private void ResetCamera(string[] parameters, System.Action onComplete)
    {
        Actor actor = PlayerController.Instance.GetPlayer();
        actor.Enable();
        CameraController.Instance.SetMainCamera(actor.actorCamera);
        PlayerController.Instance.canSwap = true;

        onComplete();
    }

    /// <summary>
    /// load new scene
    /// </summary>
    /// <param name="parameters"></param>
    /// <param name="onComplete"></param>
    private void LoadScene(string[] parameters, System.Action onComplete)
    {
        string sceneName = "";
        for (int i = 0; i < parameters.Length; i++)
        {
            sceneName += parameters[i];
            if (i < parameters.Length - 1)
            {
                sceneName += " ";
            }
        }
        SceneManager.LoadScene(sceneName);
        onComplete();
    }

    /// <summary>
    /// turns mouse on and off
    /// </summary>
    /// <param name="parameters"></param>
    /// <param name="onComplete"></param>
    private void SetMouseActive(string[] parameters, System.Action onComplete)
    {
        bool isMouseOn = bool.Parse(parameters[0]);
        GameManager.SetCursorActive(isMouseOn);
        onComplete();
    }

    /// <summary>
    /// pans the camera from one point to another.
    /// </summary>
    /// <param name="camera">camera to pan</param>
    /// <param name="target">ending position and rotation</param>
    /// <param name="panTime">length of pan</param>
    /// <returns></returns>
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

    /// <summary>
    /// pans the camera from one point to another. blocking version
    /// </summary>
    /// <param name="camera">camera to pan</param>
    /// <param name="target">ending position and rotation</param>
    /// <param name="panTime">length of pan</param>
    /// <param name="onComplete">called at the end to let DialogueRunner know this function is complete</param>
    /// <returns></returns>
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

    protected void StartCutscene(CameraEntity cutsceneCam)
    {
        CameraController.Instance.SetMainCamera(cutsceneCam);
    }

    private void LoadSprites(string pathname)
    {
        Sprite[] allSprites = Resources.LoadAll<Sprite>(pathname);
        foreach (Sprite sprite in allSprites)
        {
            SpriteInfo newSpriteInfo;
            newSpriteInfo.sprite = sprite;
            newSpriteInfo.name = sprite.name;
            talkingSprites.Add(newSpriteInfo);
        }
    }
    protected void ShowSprites(string[] parameters, System.Action onComplete)
    {
        oliverSprite.SetActive(true);
        catSprite.SetActive(true);
        onComplete();
    }

    protected void EnableControl(string[] parameters, System.Action onComplete)
    {
        Actor actor = PlayerController.Instance.GetPlayer();
        actor.SetControlActive(true);
        CameraController.Instance.SetMainCamera(actor.actorCamera);
        onComplete();
    }
}
