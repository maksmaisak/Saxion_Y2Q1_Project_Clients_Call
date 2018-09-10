using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum PlayerProfile
{
    Achiever,
    Socializer,
    Killer,
    Explorer,
    All,
}

public class Profiler : MyBehaviour, IEventReceiver<ProfilerUpdate>
{
    private readonly Dictionary<PlayerProfile, int> profiles = 
        new Dictionary<PlayerProfile, int>();

    protected void Start()
    {
        for (PlayerProfile index = PlayerProfile.Achiever; index != PlayerProfile.All; index++)
            profiles.Add(index, 0);
    }

    public void On(ProfilerUpdate update)
    {
        try
        {
            profiles[update.profile]++;
        }
        catch (KeyNotFoundException)
        {
            Debug.LogError("The requested player profile is not handled.");
        }
    }

    public PlayerProfile getMostDominantProfile => profiles.OrderBy(x => x.Value).First().Key;
}
