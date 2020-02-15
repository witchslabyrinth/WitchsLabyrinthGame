using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity.Example;

public class LiarTriggerZone : MonoBehaviour
{
    private NPC npcScript;

    [SerializeField]
    private CameraEntity dialogueCam;

    private void Start()
    {
        npcScript = GetComponent<NPC>();
    }
    private void OnTriggerEnter(Collider other)
    {
        PlayerInteractionController controller = other.GetComponent<PlayerInteractionController>();
        if (controller != null)
            controller.SetInDialogueZone(true, npcScript, dialogueCam);
        Debug.Log("Talking to " + npcScript.characterName);
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerInteractionController controller = other.GetComponent<PlayerInteractionController>();
        if (controller != null)
            controller.SetInDialogueZone(false, npcScript, dialogueCam);
        Debug.Log("Leaving conversation with " + npcScript.characterName);
    }
}
