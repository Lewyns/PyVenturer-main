using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class ButtonHold : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private Action onHoldStart;
    private Action onHoldEnd;

    public void Setup(Action holdStart, Action holdEnd)
    {
        onHoldStart = holdStart;
        onHoldEnd = holdEnd;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        onHoldStart?.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        onHoldEnd?.Invoke();
    }
}
