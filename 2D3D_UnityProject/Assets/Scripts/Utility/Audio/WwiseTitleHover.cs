using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

using UnityEngine;

public class WwiseTitleHover : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    public AK.Wwise.Event mouseDownSound;
    public AK.Wwise.Event mouseUpSound;

    public void OnPointerDown(PointerEventData e)
    {
        mouseDownSound.Post(gameObject);
    }

    public void OnPointerUp(PointerEventData e)
    {
        mouseUpSound.Post(gameObject);
    }
}
