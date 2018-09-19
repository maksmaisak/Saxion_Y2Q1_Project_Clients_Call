using System;
using System.Collections.Generic;
using UnityEngine;

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

    protected override void Awake()
    {
        base.Awake();
        ResetGameplaySessionInfo();
    }
       
    public void On(OnScoreChange message) => playerScore += message.scoreDelta;
    public void On(OnGameplaySessionStarted message) => ResetGameplaySessionInfo();
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