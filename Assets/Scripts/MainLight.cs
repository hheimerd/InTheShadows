using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainLight : MonoBehaviour
{
    public static MainLight Instance { get; private set; }
    private Light _light;
    private float _initialIntencivity;

    [SerializeField] [Range(0.01f, 3f)] private float blinkTime = 1f;
    
    // Start is called before the first frame update
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        } else {
            Instance = this;
        }
    }

    private void Start()
    {
        _light = GetComponent<Light>();
        _initialIntencivity = _light.intensity;
        _light.intensity = 0;
        StartCoroutine(SetTurnOn(true));
    }

    public delegate void OnFlashLightIsOff();
    public void Blink(OnFlashLightIsOff cb = null)
    {
        StopAllCoroutines();
        StartCoroutine(BlinkCoroutine(cb));
    }

    public IEnumerator SetTurnOn(bool isOn)
    {
        float total = isOn ? 0 : 1;
        while (true)
        {
            var step = Time.deltaTime / (blinkTime / 2);
            if (!isOn)
                step *= -1f;
            
            if (total < 0 || total > 1) break;
            
            total += step;
            _light.intensity = _initialIntencivity * total;
            yield return new WaitForEndOfFrame();
        }
    }

    public IEnumerator BlinkCoroutine(OnFlashLightIsOff cb = null)
    {
        yield return SetTurnOn(false);
        cb?.Invoke();
        yield return SetTurnOn(true);

    }

}
