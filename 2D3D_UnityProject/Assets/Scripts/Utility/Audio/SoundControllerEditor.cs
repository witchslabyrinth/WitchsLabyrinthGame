using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SoundController))]
public class SoundControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Draws the default inspector for SoundController before adding any custom stuff
        base.DrawDefaultInspector();

        // Plays specified sound effect when clicked
        if (GUILayout.Button("Test Sound Effect")) {
            SoundType soundEffect = SoundController.Instance.soundEffect;
            
            //Debug.LogFormat(name + " | Testing {0} sound effect", soundEffect);
            SoundController.Instance.PlaySoundEffect(soundEffect);
        }
    }
}
