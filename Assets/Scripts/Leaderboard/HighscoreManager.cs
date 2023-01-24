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

    private static readonly HighscoreEntry DefaultEntry = new HighscoreEntry() { Name = "???", Score = 0};
    
    // private XML xml = new();
    private List<HighscoreEntry> scores = new();
    private XML _xml = new();
    [SerializeField] HighscoreDisplay[] highscoreDisplays;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        _xml.CreateHighscoresDirectory();
        Load();
    }

    public void DisplayHighscores()
    {
        for (int i = 0; i < highscoreDisplays.Length; i++)
        {
            if (i < scores.Count) highscoreDisplays[i].Display(scores[i]);
            else highscoreDisplays[i].Display(DefaultEntry);
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
        scores.Add(new HighscoreEntry { Name = name, Score = score});
        Sort();
        Save();
    }

    private void Load()
    {
        scores = _xml.LoadScoresFromFile();
    }

    private void Save()
    {
        _xml.SaveScoresToFile(scores);
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
}
class XML
{
    public static XML Instance { get; private set; }
    public Leaderboard leaderboard = new();
    
    // private string highscoresFilePath;

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

    public void CreateHighscoresDirectory()
    {
        string highscoresDirectoryPath = GetHighscoresDirectoryPath();
        Debug.Log(highscoresDirectoryPath);
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
public class Leaderboard
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