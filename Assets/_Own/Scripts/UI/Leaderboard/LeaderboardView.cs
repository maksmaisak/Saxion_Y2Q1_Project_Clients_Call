using TMPro;
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

public class LeaderboardView : MyBehaviour,
    IEventReceiver<OnLeaderboardDisplay>
{
    [SerializeField] List<LeaderboardViewEntry> leaderboardViews = new List<LeaderboardViewEntry>();

    private void Start()
    {
        Assert.IsTrue(leaderboardViews.Count >= 6);
    }

    public void On(OnLeaderboardDisplay message)
    {
        var leaderboard = LeaderboardManager.instance.GetLeaderboardForCurrentMode();
        var leaderboardEntries = leaderboard.GetLeaderboardEntries;

        for(int i = 0; i < leaderboardEntries.Count; i++)
        {
            LeaderboardEntry entry = leaderboardEntries[i];
            leaderboardViews[i].nameText.text = i + ". " + entry.playerName; 
            leaderboardViews[i].scoreText.text = entry.playerScore.ToString("D2"); 
        }
    }
}