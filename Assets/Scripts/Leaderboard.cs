using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class HighScoreEntry
{
    public int score;
    public string name;
}

[Serializable]
public class HighScoreData
{
    public List<HighScoreEntry> highScoreEntries = new List<HighScoreEntry>();
}
public class Leaderboard : MonoBehaviour
{
    private const string SaveFileName = "highscores.json";
    private HighScoreData highScoreData;

    void Awake()
    {
        LoadHighScores();
        AddHighScore(400, "Test 1");
        AddHighScore(300, "Test 2");
        AddHighScore(200, "Test 3");
        AddHighScore(100, "Test 4");
    }

    public void AddHighScore(int score, string name)
    {
        HighScoreEntry newEntry = new HighScoreEntry { score = score, name = name };
        highScoreData.highScoreEntries.Add(newEntry);
        highScoreData.highScoreEntries.Sort((entry1, entry2) => entry2.score.CompareTo(entry1.score));
        if (highScoreData.highScoreEntries.Count > 10)
        {
            highScoreData.highScoreEntries.RemoveAt(10);
        }
        SaveHighScores();
    }

    public List<HighScoreEntry> GetHighScores()
    {
        return highScoreData.highScoreEntries;
    }

    private void SaveHighScores()
    {
        string json = JsonUtility.ToJson(highScoreData, true);
        string path = Path.Combine(Application.persistentDataPath, SaveFileName);
        File.WriteAllText(path, json);
    }

    private void LoadHighScores()
    {
        string path = Path.Combine(Application.persistentDataPath, SaveFileName);
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            highScoreData = JsonUtility.FromJson<HighScoreData>(json);
        }
        else
        {
            highScoreData = new HighScoreData();
        }
    }
}
