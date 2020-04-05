using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;

public class WwiseTitleClick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    public AK.Wwise.Event OnPointerDownSound;
    public AK.Wwise.Event OnPointerUpSound;
    public AK.Wwise.Event OnPointerEnterSound;
    public AK.Wwise.Event OnPointerExitSound;
    public AK.Wwise.Event OnMenuSelect;

    //when you hover, i think
    public void OnPointerDown(PointerEventData eventData)
    {
        OnPointerDownSound.Post(gameObject);

    }

    //when you hover, i think. not sure which
    public void OnPointerEnter(PointerEventData eventData)
    {
        OnPointerEnterSound.Post(gameObject);

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnPointerExitSound.Post(gameObject);

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        OnPointerUpSound.Post(gameObject);

    }

    //when you click an option
    public void OnClick()
    {
        OnMenuSelect.Post(gameObject);
    }

   
}
