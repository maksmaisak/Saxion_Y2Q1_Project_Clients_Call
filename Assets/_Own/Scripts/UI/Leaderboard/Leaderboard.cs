using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LeaderboardEntry
{
    public string playerName;
    public int playerScore;
    public Vector3 playerDeathPosition;
    public bool isTemp;

    public LeaderboardEntry(string name, int score, Vector3 deathPosition, bool isTemp = false)
    {
        playerName = name;
        playerScore = score;
        playerDeathPosition = deathPosition;
        this.isTemp = isTemp;
    }
}

[Serializable]
public class Leaderboard
{
    private List<LeaderboardEntry> entries = new List<LeaderboardEntry>();

    public List<LeaderboardEntry> LeaderboardEntries => entries.OrderByDescending(x => x.playerScore).ToList();

    public void AddNewEntry(LeaderboardEntry newEntry)
    {
        entries.Add(newEntry);
    }

    public bool RemoveEntry(LeaderboardEntry entry)
    {
        return entries.Remove(entry);
    }

    public void RemoveAllTempEntries()
    {
        entries.RemoveAll(x => x.isTemp == true);
    }

    public int GetPlaceOnLeaderboardForPlayer(int playerScore)
    {
        List<LeaderboardEntry> leaderboardEntries = LeaderboardEntries.Where(x => x.playerScore > playerScore).ToList();
        return leaderboardEntries.Count + 1;
    }
}
