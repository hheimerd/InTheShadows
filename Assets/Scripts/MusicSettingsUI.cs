using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;

public class MusicSettingsUI : MonoBehaviour
{
    [SerializeField] private GameObject musicOnIco;
    [SerializeField] private GameObject musicOffIco;

    private void Start()
    {
        musicOnIco.SetActive(MusicSettings.MusicEnabled);
        musicOffIco.SetActive(!MusicSettings.MusicEnabled);
    }
}
