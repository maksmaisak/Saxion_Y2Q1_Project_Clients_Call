using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum PlayerProfile
{
    Killer,
    Socializer,
    Explorer,
    Achiever,
    
    NumProfiles,
}

public class Profiler : PersistentSingleton<Profiler>, 
    IEventReceiver<OnEnemyKilled>,
    IEventReceiver<OnScoreChange>,
    IEventReceiver<OnCageOpen>,
    IEventReceiver<OnPathChange>
{
    [SerializeField] int onEnemyKilledBonus = 4;
    [SerializeField] int onScoreChangeBonus = 1;
    [SerializeField] int onPathChangeBonus  = 4;
    [SerializeField] int onCageOpenBonus    = 3;

    private readonly Dictionary<PlayerProfile, int> profiles = new Dictionary<PlayerProfile, int>();
    
    void Start() => ClearProfiles();
    
    public Dictionary<PlayerProfile, int> GetProfiles() => new Dictionary<PlayerProfile, int>(profiles);
    public void Reset() => ClearProfiles();

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
        profiles[PlayerProfile.Socializer] += onCageOpenBonus;
    }

    public void On(OnPathChange message)
    {
        profiles[PlayerProfile.Explorer] += onPathChangeBonus;
    }

    private void ClearProfiles()
    {
        for (var index = PlayerProfile.Killer; index != PlayerProfile.NumProfiles; ++index)
            profiles[index] = 0;
    }
}
