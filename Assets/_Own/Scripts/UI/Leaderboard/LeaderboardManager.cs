using UnityEngine;
using UnityEngine.Assertions;
using System.Linq;
using System.Collections.Generic;

public class LeaderboardManager : PersistentSingleton<LeaderboardManager>, 
    IEventReceiver<OnResolutionScreen>, IEventReceiver<OnNameInput>,
    IEventReceiver<OnGameplaySessionStarted>
{
    [SerializeField] string endlessFileName = "endless.json";
    [SerializeField] string storyFileName = "story.json";

    private const int maxPlayersOnLeaderboard = 5;

    private readonly List<Leaderboard> leaderboards = new List<Leaderboard>();

    private Leaderboard endlessLeadboard => leaderboards[(int)LeaderboardIndex.Endless];
    private Leaderboard storyLeadboard => leaderboards[(int)LeaderboardIndex.Story];

    public int currentPlayerPlace { get; private set; }

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
        Leaderboard leaderboard = GetLeaderboardForCurrentMode();

        currentPlayerPlace = leaderboard.GetPlaceOnLeaderboardForPlayer(message.score);
        bool isOnLeaderboard = currentPlayerPlace <= maxPlayersOnLeaderboard;

        string sceneName = GlobalState.instance.playerDeathSceneName;
        int playerScore = GlobalState.instance.playerScore;
        Vector3 playerDeathPosition = GlobalState.instance.playerDeathPosition;

        leaderboard.AddNewEntry(new LeaderboardEntry("You", playerScore, playerDeathPosition, sceneName, true));

        new OnLeaderboardDisplay(leaderboard, new LeaderboardViewInfo(isOnLeaderboard, currentPlayerPlace, playerScore))
            .SetDeliveryType(MessageDeliveryType.Immediate)
            .PostEvent();
    }

    public void On(OnNameInput onNameInput)
    {
        Leaderboard leaderboard = GetLeaderboardForCurrentMode();

        LeaderboardEntry leaderboardEntry = leaderboard.LeaderboardEntries.ElementAt(currentPlayerPlace - 1);
        leaderboardEntry.playerName = onNameInput.newName;
        leaderboardEntry.isTemporary = false;

        // Save leaderboard to JSON file
        LeaderboardSerializer.SaveLeaderboardToFile(leaderboard, endlessFileName);
    }

    public Leaderboard GetLeaderboardForCurrentMode()
    {
        if (GlobalState.instance.currentGameMode == GameMode.Story)
            return storyLeadboard;

        return endlessLeadboard;
    }

    private void Start()
    {
        Leaderboard endless = LeaderboardSerializer.GetLeaderboardFromFile(endlessFileName);
        Leaderboard story = LeaderboardSerializer.GetLeaderboardFromFile(storyFileName);

        leaderboards.Add(endless);
        leaderboards.Add(story);

        Assert.IsTrue(leaderboards.Count >= 2);
    }

    public void On(OnGameplaySessionStarted message)
    {
        foreach (var leaderboard in leaderboards)
            leaderboard.RemoveAllTempEntries();
    }
}
