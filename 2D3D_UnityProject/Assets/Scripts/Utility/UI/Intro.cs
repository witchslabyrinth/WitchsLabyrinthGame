using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Intro : MonoBehaviour
{
    [SerializeField] private Graphic inputPrompt;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FlashPrompt());
    }

    // Update is called once per frame
    void Update()
    {
        // Load game scene after player input
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
            SceneLoader.LoadScene(SCENE_ID.UKIYOE);
    }

    private IEnumerator FlashPrompt()
    {
        // Ping pong the prompt alpha value with a Sinerp function
        float t = 0;
        Color color;
        while (true)
        {
            // Plug ping-pong into sinerp
            float pingpong = Mathf.PingPong(t, 1);
            float alpha = Mathfx.Sinerp(0, 1, pingpong);

            // Update alpha value
            color = inputPrompt.color;
            color.a = alpha;
            inputPrompt.color = color;

            t += Time.deltaTime;
            yield return null;
        }
    }
}
