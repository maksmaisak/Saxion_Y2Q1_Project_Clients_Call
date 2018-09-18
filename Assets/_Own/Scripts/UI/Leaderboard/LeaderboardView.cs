using TMPro;
using System;
using UnityEngine;
using UnityEngine.Assertions;
using System.Linq;
using System.Collections.Generic;

[System.Serializable]
public struct LeaderboardViewEntry
{
    public TMP_Text nameText;
    public TMP_Text scoreText;
}

public struct LeaderboardViewInfo
{
    public bool isOnLeaderboard;
    public int playerPlace;
    public int playerScore;

    public LeaderboardViewInfo(bool isOnLeaderboard, int place, int score)
    {
        this.isOnLeaderboard = isOnLeaderboard;
        playerPlace = place;
        playerScore = score;
    }
}

public class LeaderboardView : MyBehaviour,
    IEventReceiver<OnLeaderboardDisplay>, IEventReceiver<OnNameInput>
{
    [SerializeField] GameObject inputPrefab;
    [SerializeField] List<LeaderboardViewEntry> leaderboardViews = new List<LeaderboardViewEntry>();

    private const int nonLeaderboardTextIndex = 5;

    private void Start()
    {
        Assert.IsNotNull(inputPrefab);
        Assert.IsTrue(leaderboardViews.Count >= 6);
    }

    public void On(OnLeaderboardDisplay message)
    {
        var leaderboard = message.leaderboard;
        var leaderboardEntries = leaderboard.LeaderboardEntries;

        int entriesCount = Math.Min(5, leaderboardEntries.Count);

        for (int i = 0; i < entriesCount; i++)
        {
            LeaderboardEntry entry = leaderboardEntries[i];
            leaderboardViews[i].nameText.text = (i + 1).ToString() + ". " + entry.playerName;
            leaderboardViews[i].scoreText.text = entry.playerScore.ToString("D2");
        }

        int playerPlace = message.viewInfo.playerPlace;
        SetupInput(message.viewInfo.isOnLeaderboard ? 
            playerPlace - 1 : nonLeaderboardTextIndex, message.viewInfo.playerScore);
    }

    private void SetupInput(int textObjectIndex, int playerScore)
    {
        int playerPlace = LeaderboardManager.instance.currentPlayerPlace;

        string nameText = playerPlace.ToString() + ".  ";
        leaderboardViews[textObjectIndex].nameText.text = nameText;

        if (textObjectIndex >= nonLeaderboardTextIndex)
            leaderboardViews[textObjectIndex].scoreText.text = playerScore.ToString("D2");

        GameObject nameTextObject = leaderboardViews[textObjectIndex].nameText.gameObject;
        Instantiate(inputPrefab, nameTextObject.transform);
    }

    public void On(OnNameInput message)
    {
        int playerPlace = LeaderboardManager.instance.currentPlayerPlace;
        int textObjectIndex = playerPlace > nonLeaderboardTextIndex ? nonLeaderboardTextIndex : playerPlace - 1;
        leaderboardViews[textObjectIndex].nameText.text = playerPlace + ". " + message.newName;
    }
}