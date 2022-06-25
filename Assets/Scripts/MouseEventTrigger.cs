using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MouseEventTrigger : MonoBehaviour
{
    public UnityEvent OnMouseDragEvent;
    public UnityEvent OnMouseEnterEvent;
    public UnityEvent OnMouseExitEvent;
    public UnityEvent OnMouseOverEvent;
    public UnityEvent OnMouseUpAsButtonEvent;
    public UnityEvent OnMouseDownEvent;
    public UnityEvent OnMouseUpEvent;

    private void OnMouseDrag()
    {
        OnMouseDragEvent?.Invoke();
    }

    private void OnMouseEnter()
    {
        OnMouseEnterEvent?.Invoke();
    }

    private void OnMouseExit()
    {
        OnMouseExitEvent?.Invoke();
    }

    private void OnMouseOver()
    {
        OnMouseOverEvent?.Invoke();
    }

    private void OnMouseUpAsButton()
    {
        OnMouseUpAsButtonEvent?.Invoke();
    }

    private void OnMouseDown()
    {
        OnMouseDownEvent?.Invoke();
    }

    private void OnMouseUp()
    {
        OnMouseUpEvent?.Invoke();
    }
}