
public class OnGameModeSelect : BroadcastEvent<OnGameModeSelect>
{
    public readonly GameMode selectedGameMode;
    public readonly Difficulty selectedDifficulty;

    public OnGameModeSelect(GameMode selectedGameMode, Difficulty selectedDifficulty)
    {
        this.selectedGameMode = selectedGameMode;
        this.selectedDifficulty = selectedDifficulty;
    }
}

