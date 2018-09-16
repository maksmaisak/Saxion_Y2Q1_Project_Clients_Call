using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class LeaderboardManager : PersistentSingleton<LeaderboardManager>, 
    IEventReceiver<OnResolutionScreen>
{
    [SerializeField] string endlessFileName = "endless.json";
    [SerializeField] string storyFileName = "story.json";

    private readonly List<Leaderboard> leaderboards = new List<Leaderboard>();

    private Leaderboard GetEndlessLeaderboard => leaderboards[(int)LeaderboardIndex.Endless];
    private Leaderboard GetStoryLeaderboard => leaderboards[(int)LeaderboardIndex.Story];

    internal enum LeaderboardIndex
    {
        Endless = 0,
        Story = 1,
        All = 2,
    }

    public void On(OnResolutionScreen message)
    {
        /// Do here all sorts of leaderboard stuff before updating
        /// the LeaderboardView
        new OnLeaderboardDisplay().SetDeliveryType(MessageDeliveryType.Immediate).PostEvent();
    }

    public Leaderboard GetLeaderboardForCurrentMode()
    {
        /// TEMP: until we implement different modes;
        return GetEndlessLeaderboard;
    }

    private void Start()
    {
        DontDestroyOnLoad(this);

        Leaderboard endless = LeaderboardSerializer.GetLeaderboardFromFile(endlessFileName);
        Leaderboard story = LeaderboardSerializer.GetLeaderboardFromFile(storyFileName);

        leaderboards.Add(endless);
        leaderboards.Add(story);

        Assert.IsTrue(leaderboards.Count >= 2);
    }
}
