using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "SoundDatabase")]
public class SoundDatabase : ScriptableObject
{
    [Header("Music")]
    /// <summary>
    /// Collection of all background music
    /// </summary>
    [SerializeField]
    protected List<SoundTrack> _backgroundMusic;

    // Collection of game sound effects
    [Header("Sound Effects")]
    [SerializeField]
    protected List<SoundTrack> _playerSoundEffects;

    [SerializeField]
    protected List<SoundTrack> _UISoundEffects;

    /// <summary>
    /// Returns list of game's sound effects
    /// </summary>
    /// <returns></returns>
    public List<SoundTrack> GetSoundEffects()
    {
        // Compile sound effect collections into one list
        List<SoundTrack> soundEffects = new List<SoundTrack>();
        soundEffects.AddRange(_playerSoundEffects);
        soundEffects.AddRange(_UISoundEffects);

        return soundEffects;
    }

    public List<SoundTrack> GetMusic()
    {
        return _backgroundMusic;
    }
}
