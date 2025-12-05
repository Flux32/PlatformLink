using System;

namespace RetroCat.PlatformLink.Runtime.Source.Common.Modules.Leaderboards
{
    [Serializable]
    public class LeaderboardEntries
    {
        public string LeaderboardId { get; }
        public int UserRank { get; }
        public LeaderboardRange[] Ranges { get; }
        public LeaderboardEntry[] Entries { get; }

        public LeaderboardEntries(
            string leaderboardId,
            int userRank,
            LeaderboardRange[] ranges,
            LeaderboardEntry[] entries)
        {
            LeaderboardId = leaderboardId;
            UserRank = userRank;
            Ranges = ranges ?? Array.Empty<LeaderboardRange>();
            Entries = entries ?? Array.Empty<LeaderboardEntry>();
        }
    }
}

