using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

using System.Linq;
using UnityEngine;
using TMPro;

public class SelectorField : MonoBehaviour
{
    private static readonly string[] Alphabet = {"A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "!"};
    private int _index;
    private bool _isActive;

    [SerializeField] private TextMeshProUGUI charText;
    [SerializeField] private GameObject activeIndicator;

    private void Start()
    { 
        Hide();
    }

    private void Update()
    {
        if (_isActive)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                _index++;
                _index %= Alphabet.Length;
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                _index--;
                if (_index < 0) _index = Alphabet.Length - 1;
            }
            UpdateDisplay();
        }
    }

    public void SetActive()
    {
        activeIndicator.SetActive(true);
        _isActive = true;
    }

    public void Hide()
    {
        charText.text = _isActive ? "" : "?";
    }

    public void SetInactive()
    {
        activeIndicator.SetActive(false);
        _isActive = false;
    }

    private void UpdateDisplay()
    {
        charText.text = Alphabet[_index];
    }

    public string GetCharacter()
    {
        return Alphabet[_index];
    }
}