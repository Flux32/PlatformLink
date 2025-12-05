using System;

namespace RetroCat.PlatformLink.Runtime.Source.Common.Modules.Leaderboards
{
    [Serializable]
    public class LeaderboardEntry
    {
        public int Score { get; }
        public string ExtraData { get; }
        public int Rank { get; }
        public LeaderboardPlayer Player { get; }
        public string FormattedScore { get; }

        public LeaderboardEntry(
            int score,
            string extraData,
            int rank,
            LeaderboardPlayer player,
            string formattedScore)
        {
            Score = score;
            ExtraData = extraData;
            Rank = rank;
            Player = player;
            FormattedScore = formattedScore;
        }
    }
}

