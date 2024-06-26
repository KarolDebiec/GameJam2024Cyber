using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardDisplay : MonoBehaviour
{
    public Leaderboard leaderboard;
    public Text leaderboardText;
    void Start()
    {
        DisplayHighScores();
    }

    public void DisplayHighScores()
    {
        List<HighScoreEntry> highScores = leaderboard.GetHighScores();
        leaderboardText.text = "";
        for (int i = 0; i < highScores.Count; i++)
        {
            leaderboardText.text += $"{highScores[i].name}: {highScores[i].score}\n";
        }
    }
}
