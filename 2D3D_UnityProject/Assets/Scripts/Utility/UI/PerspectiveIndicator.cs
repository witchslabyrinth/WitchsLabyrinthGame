using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class PerspectiveIndicator : MonoBehaviour
{
    /// <summary>
    /// Associated camera perspective
    /// </summary>
    public OldCameraController.CameraViews cameraView;

    /// <summary>
    /// Perspective indicator alpha value when not selected
    /// </summary>
    [SerializeField] 
    [Range(0,1)] private float disabledAlpha;


    [SerializeField] private Image image;

    /// <summary>
    /// Sprite to show in image
    /// </summary>
    [SerializeField] private Sprite sprite;

    /// <summary>
    /// Empty rect below image that adds bottom padding when enabled
    /// </summary>
    [SerializeField] private RectTransform bottomOffset;


    // Use this for initialization
    void Start()
    {
        // Set image sprite
        image.sprite = sprite;
    }

    public void SetSelected(bool selected)
    {
        // Set image alpha
        Color color = image.color;
        color.a = selected ? 1f : disabledAlpha;
        image.color = color;

        // Enable/disable bottom offset
        bottomOffset.gameObject.SetActive(selected);
    }
}
