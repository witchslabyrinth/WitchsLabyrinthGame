using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostWwiseEvent : MonoBehaviour
{
    public AK.Wwise.Event MyEvent;

    private void PlayFootStepAudio()
    {
        AkSoundEngine.PostEvent("Footsteps", gameObject);
    }
}
