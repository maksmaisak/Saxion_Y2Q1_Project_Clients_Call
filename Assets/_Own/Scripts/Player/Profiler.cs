using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum PlayerProfile
{
    Achiever,
    Socializer,
    Killer,
    Explorer,
    All,
}

public class Profiler : MyBehaviour, IEventReceiver<OnEnemyKilled>, 
    IEventReceiver<OnScoreChange>, IEventReceiver<OnCageOpen>, IEventReceiver<OnPathChange>
{
    readonly Dictionary<PlayerProfile, int> profiles = new Dictionary<PlayerProfile, int>();

    [SerializeField] int onEnemyKilledBonus = 4;
    [SerializeField] int onScoreChangeBonus = 1;
    [SerializeField] int onPathChangeBonus  = 4;
    [SerializeField] int onCageReleaseBonus = 3;

    public PlayerProfile getMostDominantProfile => profiles.OrderBy(x => x.Value).First().Key;

    protected void Start()
    {
        for (PlayerProfile index = PlayerProfile.Achiever; index != PlayerProfile.All; index++)
            profiles.Add(index, 0);
    }

    public void On(OnEnemyKilled message)
    {
        profiles[PlayerProfile.Killer] += onEnemyKilledBonus;
    }

    public void On(OnScoreChange message)
    {
        profiles[PlayerProfile.Achiever] += onScoreChangeBonus;
    }

    public void On(OnCageOpen message)
    {
        profiles[PlayerProfile.Socializer] += onCageReleaseBonus;
    }

    public void On(OnPathChange message)
    {
        profiles[PlayerProfile.Explorer] += onPathChangeBonus;
    }
}
