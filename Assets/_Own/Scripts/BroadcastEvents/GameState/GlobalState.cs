using System;
using System.Collections.Generic;
using UnityEngine;

/// Commonly accessible storage of global game state.
public class GlobalState : PersistentSingleton<GlobalState>,
    IEventReceiver<OnScoreChange>, IEventReceiver<OnPlayerDeath>
{
    public int playerScore { get; private set; }
    public readonly Dictionary<PlayerProfile, int> profiles = new Dictionary<PlayerProfile, int>();
    public Vector3 playerPosition { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        Reset();
    }
       
    public void On(OnScoreChange message)
    {
        playerScore += message.scoreDelta;
    }

    public void On(OnPlayerDeath message)
    {
        playerPosition = message.position;
    }

    public void Reset()
    {
        playerScore = 0;
        playerPosition = Vector3.zero;
        ClearProfiles();
    }
    
    private void ClearProfiles()
    {
        profiles.Clear();
        for (var index = PlayerProfile.Killer; index != PlayerProfile.NumProfiles; ++index)
            profiles[index] = 0;
    }
}