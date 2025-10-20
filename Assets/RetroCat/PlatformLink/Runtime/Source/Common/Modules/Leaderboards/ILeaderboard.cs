using System;

namespace RetroCat.PlatformLink.Runtime.Source.Common.Modules.Leaderboards
{
    public interface ILeaderboard
    {
        public void SetScore(string leaderBoardId, int score);
    }
}