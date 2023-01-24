using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HighscoreDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI scoreText;
    
    public void Display(HighscoreEntry entry)
    {
        nameText.text = entry.Name;
        scoreText.text = $"{entry.Score:000000}";
    }
    public void Hide()
    {
        nameText.text = "";
        scoreText.text = "";
    }
}
