using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using System.Linq;
using UnityEngine.SceneManagement;

public class LeaderboardManager : PersistentSingleton<LeaderboardManager>, 
    IEventReceiver<OnResolutionScreen>, IEventReceiver<OnNameInput>
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

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void On(OnResolutionScreen message)
    {
        /// Do here all sorts of leaderboard stuff before updating
        /// the LeaderboardView
        Leaderboard leaderboard = GetLeaderboardForCurrentMode();

        currentPlayerPlace = leaderboard.GetPlaceOnLeaderboardForPlayer(message.score);
        bool isOnLeaderboard = currentPlayerPlace <= maxPlayersOnLeaderboard;

        leaderboard.AddNewEntry(new LeaderboardEntry("You", message.score, message.position, true));

        new OnLeaderboardDisplay(leaderboard, new LeaderboardViewInfo(isOnLeaderboard, currentPlayerPlace, message.score))
            .SetDeliveryType(MessageDeliveryType.Immediate)
            .PostEvent();
    }

    public void On(OnNameInput onNameInput)
    {
        Leaderboard leaderboard = GetLeaderboardForCurrentMode();

        LeaderboardEntry leaderboardEntry = leaderboard.LeaderboardEntries.ElementAt(currentPlayerPlace - 1);
        leaderboardEntry.playerName = onNameInput.newName;
        leaderboardEntry.isTemp = false;

        // Save leaderboard to JSON file
        LeaderboardSerializer.SaveLeaderboardToFile(leaderboard, endlessFileName);
    }

    public Leaderboard GetLeaderboardForCurrentMode()
    {
        /// TEMP: until we implement different modes;
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

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (scene.name == SceneNames.mainLevelName)
            foreach (var leaderboard in leaderboards)
                leaderboard.RemoveAllTempEntries();
    }
}
