using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragPanel : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private GameObject joystick;

    public event Action<Vector2> OnSetDragDelta;
    public event Action OnEndDragOnPanel;

    public void OnBeginDrag(PointerEventData eventData)
    {
        joystick.transform.position = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        joystick.transform.position = new Vector2(joystick.transform.position.x + eventData.delta.x,
            joystick.transform.position.y + eventData.delta.y);
        OnSetDragDelta?.Invoke(eventData.delta);
        
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        OnEndDragOnPanel?.Invoke();
            
    }
}
