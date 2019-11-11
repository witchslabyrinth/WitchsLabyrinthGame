using UnityEngine;
using System.Collections;

[System.Serializable]
[CreateAssetMenu(fileName = "SoundTrack")]
public class SoundTrack : ScriptableObject
{   

    public SoundType type;
    public AudioClip clip;

    [Range(0, 1)]
    public float volume = 1;
    public bool loop = false;
}

public enum SoundType
{
    NONE = 0,

}
