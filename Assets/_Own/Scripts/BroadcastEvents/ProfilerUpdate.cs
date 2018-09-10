using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ProfilerUpdate : BroadcastEvent<ProfilerUpdate>
{
    public ProfilerUpdate(PlayerProfile profileType) { profile = profileType; }
    public PlayerProfile profile { get; }
}
