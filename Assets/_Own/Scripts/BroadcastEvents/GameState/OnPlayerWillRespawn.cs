
public class OnPlayerWillRespawn : BroadcastEvent<OnPlayerWillRespawn> 
{
    // TODO This data does not belong here and must be removed.
    public readonly ObjectRepresentation previousPlatform;

    public OnPlayerWillRespawn(ObjectRepresentation previousPlatform)
    {
        this.previousPlatform = previousPlatform;
    }
}
