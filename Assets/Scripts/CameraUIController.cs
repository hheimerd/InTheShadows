using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CameraUIController : MonoBehaviour
{
    [SerializeField]
    private Light lightSource;

    private MenuButton _selectedButton;
    private void Update()
    {
        ScanButtons();
        if (!_selectedButton) return;
        
        if (Input.GetMouseButtonUp(0))
            _selectedButton.Click();
        
        _selectedButton.AddTextStyle(FontStyles.Underline);


    }

    private void ScanButtons()
    {
        if (_selectedButton)
            _selectedButton.RemoveTextStyle(FontStyles.Underline);
        
        _selectedButton = null;
        
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out RaycastHit hit)) return;

        if (hit.collider.CompareTag("UIReflector"))
        {
            if (!ReflectRay(hit.point, out hit)) return;
        }

        if (!hit.collider.CompareTag("UI")) return;

        _selectedButton = hit.collider.GetComponent<MenuButton>();
    }

    bool ReflectRay(Vector3 origin, out RaycastHit hit)
    {
        return  Physics.Linecast(origin, lightSource.transform.position, out hit);
    }
}
