using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [Serializable]
    class GeometryForm
    {
        public Draggable geometryForm;
        public Vector3 successRotation;
        internal bool completed;
    }
    
    [SerializeField] private AudioSource _winAudio;
    [SerializeField] [CanBeNull] private Collider rootCollider;
    [SerializeField] [CanBeNull] private GameObject winMenu;

    [SerializeField] private bool rootRotationEnabled = false;
    [SerializeField] List<GeometryForm> geometry;
    [SerializeField] float imprecisionRotation = 5;

    private void Start()
    {
        if (winMenu != null) winMenu.SetActive(false);
        foreach (var setting in geometry)
        {
            setting.geometryForm.OnDragEnd += () => OnDragEndHandler(setting);
        }
    }

    private void OnDragEndHandler(GeometryForm settings)
    {
        var geometryTransform = settings.geometryForm.transform;

        var rotationDistance = geometryTransform.localEulerAngles - settings.successRotation;
        var rotationCompleted = CheckRotation(rotationDistance, imprecisionRotation);

        if (rotationCompleted)
            geometryTransform.localEulerAngles = settings.successRotation;
        settings.completed = rotationCompleted;


        if (geometry.All(g => g.completed))
        {
            PlayerPrefs.SetInt(LevelMenuItem.LevelCompletedPrefix + SceneManager.GetActiveScene().name, 1);
            PlayerPrefs.SetInt(LevelMenuItem.LevelNeedPlayAnimationPrefix + SceneManager.GetActiveScene().name, 1);
            PlayerPrefs.Save();
            _winAudio.PlayOneShot(_winAudio.clip);
            if (winMenu != null) winMenu.SetActive(true);
        }
    }

    private static bool CheckDistance(Vector3 distance, float imprecision)
    {
        return Math.Abs(distance.x) < imprecision &&
               Math.Abs(distance.y) < imprecision &&
               Math.Abs(distance.z) < imprecision;
    }

    private static bool CheckRotation(Vector3 rotation, float imprecision)
    {
        return CheckAxisAngle(rotation.x, imprecision) &&
               CheckAxisAngle(rotation.y, imprecision) &&
               CheckAxisAngle(rotation.z, imprecision);
    }

    private static bool CheckAxisAngle(float angle, float imprecision)
    {
        float normalized = Math.Abs(angle % 180);
        return normalized < imprecision || normalized > 180 - imprecision;
    }

    private void FixedUpdate()
    {
        if (rootCollider != null)
            rootCollider.enabled = rootRotationEnabled && Input.GetKey(KeyCode.LeftControl);
    }
}