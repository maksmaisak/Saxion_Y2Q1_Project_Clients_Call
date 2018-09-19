using System;
using System.Collections.Generic;
using UnityEngine;

public enum GameMode
{
    Story = 0,
    Endless = 1,
    All,
}

/// Common storage of global game state. Persistent between scenes.
public class GlobalState : PersistentSingleton<GlobalState>,
    IEventReceiver<OnScoreChange>,
    IEventReceiver<OnGameplaySessionStarted>,
    IEventReceiver<OnPlayerDeath>
{
    public int playerScore { get; private set; }
    public readonly Dictionary<PlayerProfile, int> profiles = new Dictionary<PlayerProfile, int>();
    public Vector3 playerDeathPosition { get; private set; }
    public string playerDeathSceneName { get; private set; }
    public GameMode currentGameMode { get; private set; }
    public Difficulty currentGameDifficulty { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        currentGameMode = GameMode.Story;
        currentGameDifficulty = Difficulty.Medium;

        ResetGameplaySessionInfo();
    }

    public void On(OnScoreChange message) => playerScore += message.scoreDelta;
    public void On(OnGameplaySessionStarted message) => ResetGameplaySessionInfo();

    public void On(OnGameModeSelect mode)
    {
        currentGameMode = mode.selectedGameMode;
        currentGameDifficulty = mode.selectedDifficulty;
    }

    public void On(OnPlayerDeath message)
    {
        playerDeathPosition = message.deathPosition;
        playerDeathSceneName = message.deathSceneName;
    }

    private void ResetGameplaySessionInfo()
    {
        playerScore = 0;
        playerDeathPosition = Vector3.zero;
        ClearProfiles();
    }
    
    private void ClearProfiles()
    {
        profiles.Clear();
        for (var index = PlayerProfile.Killer; index != PlayerProfile.NumProfiles; ++index)
            profiles[index] = 0;
    }
}