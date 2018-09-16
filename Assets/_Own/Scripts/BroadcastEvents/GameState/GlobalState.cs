using System;
using System.Collections.Generic;

/// Commonly accessible storage of global game state.
public class GlobalState : PersistentSingleton<GlobalState>,
    IEventReceiver<OnScoreChange>
{
    public int playerScore { get; private set; }
    public readonly Dictionary<PlayerProfile, int> profiles = new Dictionary<PlayerProfile, int>();

    protected override void Awake()
    {
        base.Awake();
        Reset();
    }
       
    public void On(OnScoreChange message)
    {
        playerScore += message.scoreDelta;
    }

    public void Reset()
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