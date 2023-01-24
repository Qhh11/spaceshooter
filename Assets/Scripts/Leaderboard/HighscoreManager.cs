using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class HighscoreManager : MonoBehaviour
{
    public static HighscoreManager Instance { get; private set; }
    
    private XML xml = new();
    private List<HighscoreEntry> scores = new();
    [SerializeField] HighscoreDisplay[] highscoreDisplays;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void DisplayHighscores()
    {
        for (int i = 0; i < highscoreDisplays.Length; i++)
        {
            highscoreDisplays[i].Display(scores[i]);
        }
    }

    public void HideHighscores()
    {
        foreach (var display in highscoreDisplays)
        {
            display.Hide();
        }
    }

    public void AddScore(int score, string name)
    {
        scores.Add(new HighscoreEntry(score, name));
        Sort();
        Save();
    }

    private void Load()
    {
        scores = xml.LoadScoresFromFile();
    }

    private void Save()
    {
        xml.SaveScoresToFile(scores);
    }

    private void Sort()
    {
        scores = scores.OrderByDescending(x => x.Score).ToList();
    }
    
}
public class HighscoreEntry
{
    public int Score;
    public string Name;

    public HighscoreEntry(int score, string name)
    {
        Score = score;
        Name = name;
    }
}

public class HighscoreDisplay : MonoBehaviour
{
    [SerializeField] private Text nameText;
    [SerializeField] private Text scoreText;
    
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

class XML
{
    public Leaderboard leaderboard = new();
    
    // private string highscoresFilePath;
    public XML()
    {
        CreateHighscoresDirectory();
    }

    public void SaveScoresToFile(List<HighscoreEntry> toSave)
    {
        leaderboard.List = toSave;
        XmlSerializer serializer = new XmlSerializer(typeof(Leaderboard));
        using (FileStream stream = new FileStream(GetHighscoresFilePath(), FileMode.Create))
        {
            serializer.Serialize(stream, leaderboard);
        }
    }

    public List<HighscoreEntry> LoadScoresFromFile()
    {
        if (File.Exists(GetHighscoresFilePath()))
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Leaderboard));
            using (FileStream stream = new FileStream(GetHighscoresFilePath(), FileMode.Open))
            {
                leaderboard = serializer.Deserialize(stream) as Leaderboard;
            }
        }
        return leaderboard.List;
    }

    private void CreateHighscoresDirectory()
    {
        string highscoresDirectoryPath = GetHighscoresDirectoryPath();
        if (!Directory.Exists(highscoresDirectoryPath))
        {
            Directory.CreateDirectory(highscoresDirectoryPath);
        }
    }

    private string GetHighscoresDirectoryPath()
    {
        return Application.persistentDataPath + "/Highscores/";
    }

    private string GetHighscoresFilePath()
    {
        return GetHighscoresDirectoryPath() + "highscores.xml";
    }
}
class Leaderboard
{
    public List<HighscoreEntry> List = new List<HighscoreEntry>();
}



/*
class XML
{
    public Leaderboard leaderboard;
    public XML()
    {
        if (!Directory.Exists(Application.persistentDataPath + "/Highscores/"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/Highscores/");
        }
    }

    public void SaveScoresToFile(List<HighscoreEntry> toSave)
    {
        leaderboard.List = toSave;
        XmlSerializer serializer = new XmlSerializer(typeof(Leaderboard));
        FileStream stream = new FileStream(Application.persistentDataPath + "/HighScores/highscores.xml", FileMode.Create);
        serializer.Serialize(stream, leaderboard);
        stream.Close();
    }

    public List<HighscoreEntry> LoadScoresFromFile()
    {
        if (File.Exists(Application.persistentDataPath + "/HighScores/highscores.xml"))
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Leaderboard));
            FileStream stream = new FileStream(Application.persistentDataPath + "/HighScores/highscores.xml", FileMode.Open);
            leaderboard = serializer.Deserialize(stream) as Leaderboard;
        }
        return leaderboard.List;
    }

}
class Leaderboard
{
    public List<HighscoreEntry> List = new();
}

*/