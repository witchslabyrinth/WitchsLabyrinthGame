using System;
using UnityEngine;
using System.Collections;
using System.Security.Cryptography;
using UnityEngine.UI;

public class FadeEffect : MonoBehaviour
{
    [SerializeField] private Canvas canvas;

    [SerializeField] private Graphic fadePrefab;

    private Graphic fadeInstance;

    public static float duration = 1f;

    void Start()
    {
        FadeIn();
    }

    /// <summary>
    /// Fades from black screen to transparent
    /// </summary>
    public void FadeIn()
    {
        StartCoroutine(Fade(1, 0));
    }

    /// <summary>
    /// Fades from transparent to black screen
    /// </summary>
    public void FadeOut()
    {
        StartCoroutine(Fade(0, 1));
    }

    /// <summary>
    /// Fades a black screen's alpha value from start to target
    /// </summary>
    /// <param name="start">Starting value</param>
    /// <param name="target">Target value</param>
    /// <returns></returns>
    private IEnumerator Fade(float start, float target)
    {
        // Instantiate if haven't already
        if (!fadeInstance)
            fadeInstance = Instantiate(fadePrefab, canvas.transform);


        // Fade color from start to target
        SetAlpha(fadeInstance, start);
        float t = 0;
        while (t < 1)
        {
            float alpha = Mathf.Lerp(start, target, t);
            SetAlpha(fadeInstance, alpha);

            t += Time.deltaTime / duration;
            yield return null;
        }

        // Ensure we hit target
        SetAlpha(fadeInstance, target);
        Destroy(fadeInstance);
    }

    private void SetAlpha(Graphic graphic, float alpha)
    {
        Color color = graphic.color;
        color.a = alpha;
        graphic.color = color;
    }
}
