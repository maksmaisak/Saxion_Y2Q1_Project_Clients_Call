using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnResolutionScreen : BroadcastEvent<OnResolutionScreen>
{
    public OnResolutionScreen(int playerScore, Dictionary<PlayerProfile, int> playerProfiles)
    {
        score = playerScore;
        profiles = playerProfiles;
    }

    public int score { get; }
    public Dictionary<PlayerProfile, int> profiles { get; }
}
