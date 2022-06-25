using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicSettings : MonoBehaviour
{
    public const string PlayerPrefMusicIsOn = "MusicIsOn";
    
    [SerializeField] private AudioSource _audio;
    
    public static bool MusicEnabled => PlayerPrefs.GetInt(PlayerPrefMusicIsOn, 1) == 1;

    private static float ambientTime;
    private void Start()
    {
        SetMusicEnabled(MusicEnabled);
        _audio.time = ambientTime;
        Debug.Log(ambientTime);
    }

    public void SetMusicEnabled(bool enabled)
    {
        _audio.mute = !enabled;
        PlayerPrefs.SetInt(PlayerPrefMusicIsOn, enabled ? 1 : 0);
    }

    private void Update()
    {
        ambientTime = _audio.time;
    }
}
