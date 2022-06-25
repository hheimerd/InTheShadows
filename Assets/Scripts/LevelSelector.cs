using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
    [SerializeField] private List<LevelMenuItem> levels;
    [SerializeField] private Transform levelItemWrapper;
    [SerializeField] private float levelSwitchTime = 1;
    [SerializeField] private float unlockAnimationTime = 1;
    [SerializeField] private GameObject rightLock;
    [SerializeField] MenuButton arrowLeft;
    [SerializeField] MenuButton arrowRight;
    [SerializeField] private TMP_Text label;
    public static bool GodMode = false;

    private LevelMenuItem SelectedLevel => levels[_selectedLevelIndex];
    private static int _selectedLevelIndex;
    private Transform _currentLevelInstance;

    public delegate void OnLevenChangeEvent(LevelMenuItem level);

    public event OnLevenChangeEvent OnLevelChange;

    // Start is called before the first frame update
    void Start()
    {
        arrowLeft.OnClick.AddListener(() => { SwitchLevel(MovementDirection.Left); });
        arrowRight.OnClick.AddListener(() => { SwitchLevel(MovementDirection.Right); });
        SetLevelSilent(_selectedLevelIndex);
    }

    #region Animation

    private IEnumerator HideLevel(Transform previousLevel, MovementDirection? direction)
    {
        if (previousLevel == null)
            yield break;
        if (direction != null)
        {
            var target = direction == MovementDirection.Left ? arrowLeft : arrowRight;
            for (float i = 0; i <= 1; i += Time.fixedDeltaTime / levelSwitchTime)
            {
                previousLevel.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, i);
                previousLevel.position = Vector3.Lerp(previousLevel.position, target.transform.position, i);
                yield return new WaitForFixedUpdate();
            }
        }

        Destroy(previousLevel.gameObject);
    }

    private IEnumerator ShowLevel(Vector3 rotationTo, Vector3? rotationFrom, MovementDirection? direction = null)
    {
        _currentLevelInstance = Instantiate(SelectedLevel.Prefab, levelItemWrapper, false);
        var levelTransform = _currentLevelInstance.transform;

        if (rotationFrom != null)
            levelTransform.localEulerAngles = (Vector3) rotationFrom;
        else
            levelTransform.localEulerAngles = rotationTo;

        // animate show level with scale and move to center
        if (direction != null)
        {
            var transformFrom = direction == MovementDirection.Left ? arrowLeft : arrowRight;

            for (float i = 0; i < 1; i += Time.fixedDeltaTime / levelSwitchTime)
            {
                levelTransform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, i);
                levelTransform.position = Vector3.Lerp(transformFrom.transform.position, levelItemWrapper.position, i);
                yield return new WaitForFixedUpdate();
            }
        }

        levelTransform.localScale = Vector3.one;
        levelTransform.position = levelItemWrapper.position;

        if (rotationFrom == null) yield break;

        // animate level rotation
        for (float i = 0; i < 1; i += Time.fixedDeltaTime / unlockAnimationTime)
        {
            levelTransform.localEulerAngles = Vector3.Lerp((Vector3) rotationFrom, rotationTo, i);
            yield return new WaitForFixedUpdate();
        }
    }

    #endregion


    #region Level switching

    private void SetLevelSilent(int newIndex)
    {
        StartCoroutine(SetLevel(newIndex, true));
    }

    private void SwitchLevel(MovementDirection directionTo)
    {
        var newIndex = _selectedLevelIndex + (int) directionTo;
        StartCoroutine(SetLevel(newIndex, true, directionTo, true));
    }

    private IEnumerator SetLevel(int newIndex, bool animateUnlock = false, MovementDirection? directionTo = null,
        bool invoke = false)
    {
        if (!UserCanPlayLevel(newIndex))
            yield break;

        _selectedLevelIndex = newIndex;

        // hide or show UI elements
        HideArrows();
        HideLocks();

        // hide previous level
        var hideDirection = ReverseDirection(directionTo);
        StartCoroutine(HideLevel(_currentLevelInstance, hideDirection));

        // check if the level unlock should be shown
        Vector3? rotationFrom = null;
        bool levelUnlocked = animateUnlock && SelectedLevel.NeedToPlayAnimation;
        if (levelUnlocked)
        {
            rotationFrom = SelectedLevel.BrokenRotation;
            SelectedLevel.NeedToPlayAnimation = false;
        }

        // show the level and it's label
        var sceneCompleted = SelectedLevel.IsCompleted || GodMode;
        
        label.text = sceneCompleted ? SelectedLevel.Label : MakeAnagram(SelectedLevel.Label);

        var rotationTo = sceneCompleted ? Vector3.zero : SelectedLevel.BrokenRotation;
        yield return ShowLevel(rotationTo, rotationFrom, directionTo);

        if (invoke)
            OnLevelChange?.Invoke(SelectedLevel);

        if (levelUnlocked)
            yield return SetLevel(newIndex + 1, animateUnlock, MovementDirection.Right, invoke);
    }

    #endregion


    #region UI

    private void HideLocks()
    {
        var nexExists = _selectedLevelIndex + 1 < levels.Count;
        bool lockIsShowed = nexExists && UserCanPlayLevel(_selectedLevelIndex + 1) == false;
        rightLock.SetActive(lockIsShowed);
    }

    private void HideArrows()
    {
        arrowLeft.gameObject.SetActive(UserCanPlayLevel(_selectedLevelIndex - 1));
        arrowRight.gameObject.SetActive(UserCanPlayLevel(_selectedLevelIndex + 1));
    }

    #endregion


    #region Level loading

    public void LoadLevel()
    {
        SceneManager.LoadScene(SelectedLevel.Level.name);
    }

    public void LoadMainMenu()
    {
        SetLevelSilent(0);
        SceneManager.LoadScene(0);
    }

    #endregion


    #region Helpers

    private MovementDirection? ReverseDirection(MovementDirection? direction)
    {
        if (direction == null) return null;
        return direction == MovementDirection.Left ? MovementDirection.Right : MovementDirection.Left;
    }

    private bool UserCanPlayLevel(int levelIdx)
    {
        if (levelIdx >= levels.Count || levelIdx < 0)
            return false;
        if (levelIdx == 0)
            return true;

        return levels[levelIdx - 1].IsCompleted || GodMode;
    }

    private string MakeAnagram(string original)
    {
        var charArr = original.ToLower().ToCharArray();
        Array.Sort(charArr);
        charArr[0] = char.ToUpper(charArr[0]);
        return new string(charArr);
    }

    #endregion


    private enum MovementDirection
    {
        Left = -1,
        Right = 1
    }
}