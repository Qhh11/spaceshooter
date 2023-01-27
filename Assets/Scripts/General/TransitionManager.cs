using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public enum PlayState
{
    MainMenu = 0,
    GameScene = 1,
    LoseScene = 2,
    WinScene = 3
}

public class TransitionManager : MonoBehaviour
{
    public static TransitionManager Instance = null;

    public Canvas canvas;

    public Animator animator;

    private bool _inTransition = false;

    public float WaitTime = 2f;

    [SerializeField] private PlayState currentState = PlayState.MainMenu;
    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else Destroy(gameObject);
    }

    public void SwitchState(PlayState state)
    {
        switch (state)
        {
            case PlayState.MainMenu:
                EndState();
                StartCoroutine(LoadScene(state));
                break;
            case PlayState.GameScene:
                EndState();
                StartCoroutine(LoadScene(state));
                break;
            case PlayState.WinScene:
                EndState();
                StartCoroutine(LoadScene(state));
                break;
            case PlayState.LoseScene:
                EndState();
                StartCoroutine(LoadScene(state));
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }
    }

    IEnumerator LoadScene(PlayState state)
    {
        if (_inTransition) yield return null;
        _inTransition = true;
        canvas.sortingOrder = 69;
        animator.SetTrigger("transition");
        yield return new WaitForSeconds(WaitTime);
        SceneManager.LoadScene((int) state);
        animator.SetTrigger("transitionopen");
        _inTransition = false;
        yield return new WaitForSeconds(WaitTime);
        canvas.sortingOrder = 0;
    }

    public void EndState()
    {
        // May do something.
    }

    // Update is called once per frame

    public void StartGameFromMenu()
    {
        Instance.SwitchState(PlayState.GameScene);
    }

    public void FromGameToLose()
    {
        Instance.SwitchState(PlayState.LoseScene);
    }

    public void FromLoseToMenu()
    {
        Instance.SwitchState(PlayState.MainMenu);
    }
}

