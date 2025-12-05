using System;

namespace RetroCat.PlatformLink.Runtime.Source.Common.Modules.Leaderboards
{
    public interface ILeaderboard
    {
        public void SetScore(string leaderboardId, int score);
        public void GetPlayerEntry(string leaderboardId, Action<bool, LeaderboardEntry> onCompleted);
        public void GetEntries(
            string leaderboardId,
            int quantityTop,
            bool includeUser,
            int quantityAround,
            Action<bool, LeaderboardEntries> onCompleted);
    }
}
