using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundController : Singleton<SoundController>
{
    [Header("Music")]
    /// <summary>
    /// Background music played on scene load
    /// </summary>
    public SoundType music;

    /// <summary>
    /// Background music currently playing
    /// </summary>
    private SoundType currentlyPlayingMusic = SoundType.NONE;

    /// <summary>
    /// Collection of all background music
    /// </summary>
    private List<SoundTrack> backgroundMusic;

    /// <summary>
    /// Collection of all sound effects
    /// </summary>
    private List<SoundTrack> soundEffects = new List<SoundTrack>();

    /// <summary>
    /// Mapping of SoundTracks to their respective audio sources.
    /// NOTE: SoundTracks should only use their own audio source if they must be looped.
    /// </summary>
    private Dictionary<SoundTrack, AudioSource> soundEffectToAudioSourceMap = new Dictionary<SoundTrack, AudioSource>();

    private AudioSource audioSource;


    [Header("Debug Menu")]
    /// <summary>
    /// Used by SoundControllerEditor to play corresponding sound effect when Test Sound Effect button is pressed in Inspector
    /// </summary>
    [Tooltip("Press the button below to play this sound effect (only works while the game is running)")]
    public SoundType soundEffect;

    override
    protected void Awake()
    {
        base.Awake();

        audioSource = GetComponent<AudioSource>();

        // Get sound effects/music from SoundDatabase
        SoundDatabase soundDatabase = Resources.Load<SoundDatabase>("SoundDatabase");
        if (soundDatabase == null) {
            Debug.LogError("Cound not find sound databsae at ScriptableObjects/SoundDatabase");
        }
        else {
            // Combine each collection of sound effects
            soundEffects = soundDatabase.GetSoundEffects();
            backgroundMusic = soundDatabase.GetMusic();

            // Set music to selected track
            SetMusic(music);
        }
    }

    /// <summary>
    /// Plays background music associated with given MusicType
    /// </summary>
    /// <param name="newMusic"></param>
    public void SetMusic(SoundType newMusic)
    {
        // Ignore if passed NONE or if given music already playing
        if (newMusic.Equals(SoundType.NONE) || newMusic.Equals(currentlyPlayingMusic))
        {
            return;
        }

        // Hook up & play music
        SoundTrack musicTrack = backgroundMusic.Find(s => s.type == newMusic);
        audioSource.clip = musicTrack.clip;
        audioSource.volume = musicTrack.volume;
        audioSource.Play();

        currentlyPlayingMusic = newMusic;
    }

    /// <summary>
    /// Pauses/resumes playing music based on value passed
    /// </summary>
    /// <param name="playing">Resumes playing music if true, pauses if false</param>
    public void SetMusicPlaying(bool playing)
    {
        if (playing)
        {
            audioSource.UnPause();
        }
        else
        {
            audioSource.Pause();
        }
    }

    /// <summary>
    /// Plays sound effect associated with SoundType
    /// </summary>
    /// <param name="type">SoundType corresponding to desired sound effect (if multiple sound effects found, will choose one at random)</param>
    public void PlaySoundEffect(SoundType type)
    {
        // Ignore request if passed NONE
        if (type.Equals(SoundType.NONE))
        {
            return;
        }

        // Play sound effect if exists
        SoundTrack track = GetSoundEffect(type);
        if (track != null)
        {
            audioSource.PlayOneShot(track.clip, track.volume);
        }
    }

    public void PlaySoundEffect(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    public void PlaySoundEffectLooping(SoundType type)
    {
        // //Debug.Log("Playing looping sound effect: " + type);
        AudioSource audioSource;
        SoundTrack track = GetSoundEffect(type);

        // Instantiate AudioSource if not found for this track
        if (!soundEffectToAudioSourceMap.TryGetValue(track, out audioSource))
        {
            // //Debug.LogFormat("AudioSource not found for {0} effect; creating one now", type);
            audioSource = InitializeAudioSource(track);

            // Add to mapping
            soundEffectToAudioSourceMap.Add(track, audioSource);
        }

        // Play sound effect
        audioSource.Play();
    }

    private AudioSource InitializeAudioSource(SoundTrack track)
    {
        // Instantiate AudioSource as child
        AudioSource audioSource = Instantiate(new GameObject().AddComponent<AudioSource>(), transform);
        audioSource.name = track.type.ToString() + "_AudioSource";

        // Configure with track's properties
        audioSource.clip = track.clip;
        audioSource.loop = track.loop; // This should always be true, but just in case...
        audioSource.volume = track.volume;

        return audioSource;
    }

    /// <summary>
    /// Returns SoundEffect associated with given SoundType. If multiple found, selects one at random.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    private SoundTrack GetSoundEffect(SoundType type)
    {
        // Get all sound effects of given type
        List<SoundTrack> soundEffects = this.soundEffects.FindAll(i => i.type.Equals(type));

        // Return null if none found
        if (soundEffects.Count == 0)
        {
            //Debug.LogWarningFormat("Warning: SoundTrack not found for {0}", type);
            return null;
        }

        // Return a random effect from the list
        int rand = Random.Range(0, soundEffects.Count - 1);
        return soundEffects[rand];
    }

    public void StopSoundEffectLooping(SoundType type)
    {
        // //Debug.Log("Stopping looping sound effect: " + type);
        AudioSource audioSource;
        SoundTrack track = GetSoundEffect(type);

        if (!soundEffectToAudioSourceMap.TryGetValue(track, out audioSource))
        {
            //Debug.LogWarningFormat("AudioSource not found for {0} effect", type);
            return;
        }
        audioSource.Stop();
    }
}
