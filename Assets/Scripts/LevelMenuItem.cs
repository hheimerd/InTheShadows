using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelMenuItem",menuName = "Level")]
public class LevelMenuItem : ScriptableObject
{
    public const string LevelCompletedPrefix = "LevelCompleted";
    public const string LevelNeedPlayAnimationPrefix = "LevelAnimationPlayed";
    
    [SerializeField] private Transform prefab;
    public Transform Prefab => prefab;
    
    [SerializeField] private string label;
    public string Label => label;
    
    [SerializeField] private SceneReference  level;
    public SceneReference Level => level;

    [SerializeField] private Vector3 brokenRotation;
    public Vector3 BrokenRotation => brokenRotation;


    public bool NeedToPlayAnimation
    {
        get => PlayerPrefs.GetInt(LevelNeedPlayAnimationPrefix + Level.Name, -1) == 1;
        set => PlayerPrefs.SetInt(LevelNeedPlayAnimationPrefix + Level.Name, value ? 1 : 0);
    }

    public bool IsCompleted
    {
        get => PlayerPrefs.GetInt(LevelCompletedPrefix + Level.Name, 0) == 1 ;
        set => PlayerPrefs.SetInt(LevelCompletedPrefix + Level.Name, value ? 1 : 0);
    }
}
