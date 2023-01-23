using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highscore : MonoBehaviour
{
    private List<HighscoreEntry> highscores;

    public static Highscore highscoreManager;
    // Start is called before the first frame update
    void Start()
    {
        if (highscoreManager == null) highscoreManager = this;
        else Destroy(gameObject);
    }

    void AddScore(int score, string name)
    {
        highscores.Add(new HighscoreEntry(score, name));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

class HighscoreEntry
{
    private int score;
    private string name;

    public HighscoreEntry(int score, string name)
    {
        this.score = score;
        this.name = name;
    }
}