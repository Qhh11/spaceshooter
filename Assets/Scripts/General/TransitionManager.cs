using System.Collections;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum PlayState
{
    MainMenu = 0,
    GameScene = 1,
    LoseScene = 2,
    SettingScene = 3,
    HighscoreScene = 4,
    HighscoreViewScene = 5
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
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SwitchState(PlayState state)
    {
        EndState();
        StartCoroutine(LoadScene(state));
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

    public static void StartGameFromMenu()
    {
        Instance.SwitchState(PlayState.GameScene);
    }

    public static void FromGameToLose()
    {
        Instance.SwitchState(PlayState.HighscoreScene);
    }

    public static void FromLoseToMenu()
    {
        
        Instance.SwitchState(PlayState.MainMenu);
    }

    public static void FromMenuToSetting()
    {
        Instance.SwitchState(PlayState.SettingScene);
    }

    public static void FromSettingToMenu()
    {
        Instance.SwitchState(PlayState.MainMenu);
    }

    public static void toHighscoreView()
    {
        Instance.SwitchState(PlayState.HighscoreViewScene);
    }

}

