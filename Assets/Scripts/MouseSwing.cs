using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseSwing : MonoBehaviour
{
    [Range(0, 90)] [SerializeField] float swingIntencity = 1;
    private Vector3 _startRotation;

    private Vector3 _newRotation;

    // Start is called before the first frame update
    void Start()
    {
        _startRotation = transform.eulerAngles;
    }

    float GetRotationAngle(float mousePosition, float maxScreenSize)
    {
        var k = mousePosition / maxScreenSize;
        return Mathf.Lerp(-swingIntencity, swingIntencity, k);
    }

    // Update is called once per frame
    void Update()
    {
        var mousePosition = Input.mousePosition;
        _newRotation = _startRotation;

        _newRotation.y = _startRotation.y + GetRotationAngle(mousePosition.x, Screen.width);
        _newRotation.x = _startRotation.x - GetRotationAngle(mousePosition.y, Screen.height);

        
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(_newRotation), 4 * Time.deltaTime);
    }
}