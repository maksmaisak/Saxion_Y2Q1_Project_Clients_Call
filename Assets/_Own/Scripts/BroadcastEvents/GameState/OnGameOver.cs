
public class OnGameOver : BroadcastEvent<OnGameOver>
{
    public readonly bool isVictory;

    public OnGameOver(bool isVictory = false)
    {
        this.isVictory = isVictory;
    }
}