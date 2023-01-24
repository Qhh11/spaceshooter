using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;

public class TestHighscoreButton : MonoBehaviour
{
    // Start is called before the first frame update
    
    
    [SerializeField]
    private TMP_InputField name;
    [SerializeField]
    private TMP_InputField score;
    [SerializeField]
    private Button btn;
    void Start()
    {
        HighscoreManager.Instance.DisplayHighscores();
    }

    // Update is called once per frame
    public void Add()
    {
        if (name.text.Length != 0 && score.text.Length != 0)
        {
            HighscoreManager.Instance.AddScore(int.Parse(score.text), name.text);
            HighscoreManager.Instance.DisplayHighscores();
        }
    }
}
