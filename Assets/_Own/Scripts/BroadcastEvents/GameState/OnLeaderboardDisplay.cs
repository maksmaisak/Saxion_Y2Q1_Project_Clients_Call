public class OnLeaderboardDisplay : BroadcastEvent<OnLeaderboardDisplay>
{
    public Leaderboard leaderboard { get; }
    public LeaderboardViewInfo viewInfo { get; }

    public OnLeaderboardDisplay(Leaderboard leaderboard, LeaderboardViewInfo viewInfo)
    {
        this.leaderboard = leaderboard;
        this.viewInfo = viewInfo;
    }
}
