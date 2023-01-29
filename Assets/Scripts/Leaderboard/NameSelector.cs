using System;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class NameSelector : MonoBehaviour
{
    [SerializeField] private SelectorField[] fields;
    private int currentIndex = 0;
    private string builtName;

    public GameObject SelectorCanvas;
    public GameObject HighScoreCanvas;

    public TextMeshProUGUI YourScore;
    private int score = 0;

    public static void toMenu()
    {
        TransitionManager current = FindObjectOfType<TransitionManager>();
        // current.FromLoseToMenu();
    }

    public static void toGame()
    {
        TransitionManager current = FindObjectOfType<TransitionManager>();
        // current.StartGameFromMenu();
    }

    private void Start()
    {
        try
        {
            score = GameManager.Instance.score;
        }
        catch (NullReferenceException)
        {
            score = 0;
        }

        SelectorCanvas.SetActive(true);
        HighScoreCanvas.SetActive(false);
        SetCurrentFieldActive();
        YourScore.text = "YOUR SCORE: " + score;
    }

    private void Update()
    {
        // TODO: Replace with actual Input system.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SelectNextField();
        }
    }

    private void SelectNextField()
    {
        builtName += fields[currentIndex].GetCharacter();

        if (IsLastField())
        {
            FinishSelection();
        }
        else
        {
            SetCurrentFieldInactive();
            currentIndex++;
            SetCurrentFieldActive();
        }
    }

    // TODO: Implement adding new High Score
    private void FinishSelection()
    {
        SetCurrentFieldInactive();
        Debug.Log("Name is " + builtName);
        HighscoreManager.Instance.AddScore(score, builtName);
        SelectorCanvas.SetActive(false);
        HighScoreCanvas.SetActive(true);
    }

    private bool IsLastField()
    {
        return currentIndex == fields.Length - 1;
    }

    private void SetCurrentFieldActive()
    {
        try
        {
            fields[currentIndex].SetActive();
        }
        catch (IndexOutOfRangeException e)
        {
            Debug.LogException(e);
        }
    }

    private void SetCurrentFieldInactive()
    {
        fields[currentIndex].SetInactive();
    }
}