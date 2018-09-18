using System;
using System.Collections.Generic;

/// Common storage of global game state. Persistent between scenes.
public class GlobalState : PersistentSingleton<GlobalState>,
    IEventReceiver<OnScoreChange>,
    IEventReceiver<OnGameplaySessionStarted>
{
    public int playerScore { get; private set; }
    public readonly Dictionary<PlayerProfile, int> profiles = new Dictionary<PlayerProfile, int>();

    protected override void Awake()
    {
        base.Awake();
        ResetGameplaySessionInfo();
    }
       
    public void On(OnScoreChange message) => playerScore += message.scoreDelta;
    public void On(OnGameplaySessionStarted message) => ResetGameplaySessionInfo();

    private void ResetGameplaySessionInfo()
    {
        playerScore = 0;
        ClearProfiles();
    }
    
    private void ClearProfiles()
    {
        profiles.Clear();
        for (var index = PlayerProfile.Killer; index != PlayerProfile.NumProfiles; ++index)
            profiles[index] = 0;
    }
}