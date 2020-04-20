using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class WwiseMenuMusic : MonoBehaviour
{
     public void LoadNextLevel(string name)
    {
        StartCoroutine(LevelLoad(name));
    }

    IEnumerator LevelLoad(string name)
    {
        yield return new WaitForSeconds(3f);
        Application.LoadLevel(name);
    }

    public void OnClick()
    {
        AkSoundEngine.SetRTPCValue("Title_Music", 0f, GameObject.Find("Main Camera"), 4000);
    }
}
