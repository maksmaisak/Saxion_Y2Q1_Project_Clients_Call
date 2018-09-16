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

    public LeaderboardEntry(string name, int score, Vector3 deathPosition)
    {
        playerName = name;
        playerScore = score;
        playerDeathPosition = deathPosition;
    }
}

[Serializable]
public class Leaderboard
{
    private List<LeaderboardEntry> entries = new List<LeaderboardEntry>();

    public void AddNewEntry(LeaderboardEntry newEntry)
    {
        /// If there is already an entry with exact name check if the currentScore is higher
        /// than previous one if it just replace it.
        LeaderboardEntry oldEntry = 
            entries.FirstOrDefault<LeaderboardEntry>(x => x.playerName == newEntry.playerName);

        if (oldEntry != null)
        {
            if (newEntry.playerScore > oldEntry.playerScore)
                entries[entries.IndexOf(oldEntry)] = newEntry;
        }
        else
            entries.Add(newEntry);
    }

    public bool RemoveEntry(LeaderboardEntry entry)
    {
        return entries.Remove(entry);
    }

    public List<LeaderboardEntry> GetLeaderboardEntries => entries.OrderByDescending(x => x.playerScore).ToList();
}
