using UnityEngine;

public class OnPlayerRespawn : BroadcastEvent<OnPlayerRespawn>
{
    public ObjectRepresentation previousPlatform { get; }

    public OnPlayerRespawn(ObjectRepresentation previousPlatform)
    {
        this.previousPlatform = previousPlatform;
    }
}
