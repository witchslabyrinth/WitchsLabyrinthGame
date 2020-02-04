using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PerspectiveIndicator : MonoBehaviour
{
    /// <summary>
    /// Associated camera perspective
    /// </summary>
    [SerializeField] private CameraController.CameraViews perspective;

    /// <summary>
    /// Empty rect below image that adds bottom padding when enabled
    /// </summary>
    [SerializeField] private RectTransform bottomOffset;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
