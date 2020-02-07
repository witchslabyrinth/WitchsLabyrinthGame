using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ID associated with scene file path
/// </summary>
public enum SCENE_ID
{
    MAIN_MENU,
    INTRO,
    UKIYOE,
    TELEDOOR,
}

/// <summary>
/// Non-monobehaviour script used for easy scene loading. Requires scenes to be added as static fields below before loading.
/// (Look I know it's bit of a pain but we should have some uniform way to load scenes)
/// </summary>
public class SceneLoader
{
    // File paths pointing to each scene
    // WARNING: IF THESE ARE INCORRECT SCENE LOADING WILL BREAK
    private const string MainMenuScene = "MainMenu";
    private const string UkiyoeScene = "Ukiyo-e Environment";
    private const string TeledoorScene = "Puzzles/TeledoorPuzzle";
    private const string IntroScene = "Intro";

    /// <summary>
    /// Dictionary associating scene ID with file path (for ease of use when calling LoadScene()). Scene key/value pair must be in the dictionary to allow loading
    /// </summary>
    private static Dictionary<SCENE_ID, string> SceneNameDictionary = new Dictionary<SCENE_ID,string>()
    {
        {SCENE_ID.MAIN_MENU, MainMenuScene},
        {SCENE_ID.INTRO, IntroScene},
        {SCENE_ID.UKIYOE, UkiyoeScene},
        {SCENE_ID.TELEDOOR, TeledoorScene},
    };

    /// <summary>
    /// Loads scene associated with given scene ID
    /// </summary>
    /// <param name="sceneId">ID of scene to load</param>
    public static void LoadScene(SCENE_ID sceneId)
    {
        // Load scene path from dictionary
        Debug.LogFormat("Attempting to load {0} scene...", sceneId);
        if (!SceneNameDictionary.TryGetValue(sceneId, out string scenePath))
        {
            // Throw error and return if dictionary doesn't have scene path (shouldn't happen, but nice to be defensive)
            Debug.LogErrorFormat("SceneLoader | Error loading scene {0}; no associated entry found in dictionary", sceneId);
            return;
        }

        // Load scene
        Debug.LogFormat("Successfully found {0} scene path; loading scene {1}.unity", sceneId, scenePath);
        SceneManager.LoadScene(scenePath);
    }
}
