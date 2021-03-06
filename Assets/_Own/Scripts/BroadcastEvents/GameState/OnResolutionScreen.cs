﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnResolutionScreen : BroadcastEvent<OnResolutionScreen>
{
    public OnResolutionScreen(int playerScore, Dictionary<PlayerProfile, int> playerProfiles, Vector3 position)
    {
        score = playerScore;
        profiles = playerProfiles;
        this.position = position;
    }

    public int score { get; }
    public Dictionary<PlayerProfile, int> profiles { get; }
    public Vector3 position { get; }
}
