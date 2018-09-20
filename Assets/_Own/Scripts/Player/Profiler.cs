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
    IEventReceiver<OnPortalEntered>
{
    [SerializeField] int onEnemyKilledBonus = 4;
    [SerializeField] int onScoreChangeBonus = 1;
    [SerializeField] int onPathChangeBonus  = 4;
    [SerializeField] int onCageOpenBonus    = 3;

    private GlobalState globalState;

    void Start()
    {
        globalState = GlobalState.instance;
    }
    
    public void On(OnEnemyKilled message)
    {
        globalState.profiles[PlayerProfile.Killer] += onEnemyKilledBonus;
    }

    public void On(OnScoreChange message)
    {
        globalState.profiles[PlayerProfile.Achiever] += onScoreChangeBonus;
    }

    public void On(OnCageOpen message)
    {
        globalState.profiles[PlayerProfile.Socializer] += onCageOpenBonus;
    }

    public void On(OnPortalEntered portal)
    {
        if (portal.kind == Portal.Kind.BonusLevelEntry)
            globalState.profiles[PlayerProfile.Explorer] += onPathChangeBonus;
    }
}
