using System.Linq;
using UnityEngine;

public class NameSelector : MonoBehaviour
{
    [SerializeField] private SelectorField[] fields;
    private int currentIndex = 0;
    private string builtName;

    private void Start()
    {
        SetCurrentFieldActive();
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
        Debug.Log("Name is " + builtName);
    }

    private bool IsLastField()
    {
        return currentIndex == fields.Length - 1;
    }

    private void SetCurrentFieldActive()
    {
        fields[currentIndex].SetActive();
    }

    private void SetCurrentFieldInactive()
    {
        fields[currentIndex].SetInactive();
    }
}