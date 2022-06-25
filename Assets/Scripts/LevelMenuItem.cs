using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "LevelMenuItem",menuName = "Level")]
public class LevelMenuItem : ScriptableObject
{
    public const string LevelCompletedPrefix = "LevelCompleted";
    public const string LevelNeedPlayAnimationPrefix = "LevelAnimationPlayed";
    
    [SerializeField] private Transform prefab;
    public Transform Prefab => prefab;
    
    [SerializeField] private string label;
    public string Label => label;
    
    [SerializeField] private SceneAsset level;
    public SceneAsset Level => level;

    [SerializeField] private Vector3 brokenRotation;
    public Vector3 BrokenRotation => brokenRotation;


    public bool NeedToPlayAnimation
    {
        get => PlayerPrefs.GetInt(LevelNeedPlayAnimationPrefix + Level.name, -1) == 1;
        set => PlayerPrefs.SetInt(LevelNeedPlayAnimationPrefix + Level.name, value ? 1 : 0);
    }

    public bool IsCompleted
    {
        get => PlayerPrefs.GetInt(LevelCompletedPrefix + Level.name, 0) == 1 ;
        set => PlayerPrefs.SetInt(LevelCompletedPrefix + Level.name, value ? 1 : 0);
    }
}
