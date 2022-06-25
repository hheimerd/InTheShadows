using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draggable : MonoBehaviour
{
    private Vector2 _mouseStartDragPosition;

    public delegate void OnDragEndEvent();
    public event OnDragEndEvent OnDragEnd;

    [SerializeField] private int mouseRotateSpeed = 1;

    [SerializeField] private MovementAxis rotationEnabled = MovementAxis.X | MovementAxis.Y;


    [Flags]
    enum MovementAxis
    {
        X = 0x01,
        Y = 0x10,
    }

    private void OnMouseDown()
    {
        _mouseStartDragPosition = Input.mousePosition;
    }

    private void OnMouseDrag()
    {
        var mouseDiff = _mouseStartDragPosition - (Vector2) Input.mousePosition;
        Rotate(mouseDiff);
    }
    
    private void OnMouseUp()
    {
        OnDragEnd?.Invoke();
    }

    private void Rotate(Vector2 mouseDiff)
    {
        var rotation = transform.eulerAngles;

        if ((rotationEnabled & MovementAxis.X) > 0)
            rotation.y += mouseDiff.x * mouseRotateSpeed * Time.deltaTime;

        if ((rotationEnabled & MovementAxis.Y) > 0)
            rotation.x += -mouseDiff.y * mouseRotateSpeed * Time.deltaTime;

        transform.eulerAngles = rotation;
    }
}