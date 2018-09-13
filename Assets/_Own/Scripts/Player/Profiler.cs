using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum PlayerProfile
{
    Killer,
    Socializer,
    Explorer,
    Achiever,
    All,
}

public class Profiler : MyBehaviour, IEventReceiver<OnEnemyKilled>, 
    IEventReceiver<OnScoreChange>, IEventReceiver<OnCageOpen>, IEventReceiver<OnPathChange>
{
    [SerializeField] int onEnemyKilledBonus = 4;
    [SerializeField] int onScoreChangeBonus = 1;
    [SerializeField] int onPathChangeBonus  = 4;
    [SerializeField] int onCageOpenBonus    = 3;

    protected void Start()
    {
        WorldRepresentation.instance.playerProfiles = new Dictionary<PlayerProfile, int>();

        for (PlayerProfile index = PlayerProfile.Killer; index != PlayerProfile.All; index++)
            WorldRepresentation.instance.playerProfiles.Add(index, 0);
    }

    public void On(OnEnemyKilled message)
    {
        WorldRepresentation.instance.playerProfiles[PlayerProfile.Killer] += onEnemyKilledBonus;
    }

    public void On(OnScoreChange message)
    {
        WorldRepresentation.instance.playerProfiles[PlayerProfile.Achiever] += onScoreChangeBonus;
    }

    public void On(OnCageOpen message)
    {
        WorldRepresentation.instance.playerProfiles[PlayerProfile.Socializer] += onCageOpenBonus;
    }

    public void On(OnPathChange message)
    {
        WorldRepresentation.instance.playerProfiles[PlayerProfile.Explorer] += onPathChangeBonus;
    }
}
