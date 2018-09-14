
public class OnPortalEntered : BroadcastEvent<OnPortalEntered>
{
    public readonly Portal.Kind kind;

    public OnPortalEntered(Portal.Kind kind)
    {
        this.kind = kind;
    }
}