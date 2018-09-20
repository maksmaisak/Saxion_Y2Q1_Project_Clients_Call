
public class OnPortalEntered : BroadcastEvent<OnPortalEntered>
{
    public readonly Portal.Kind kind;
    public readonly string storyModeNextLevelScene;

    public OnPortalEntered(Portal.Kind kind, string storyModeNextLevelScene)
    {
        this.kind = kind;
        this.storyModeNextLevelScene = storyModeNextLevelScene;
    }
}