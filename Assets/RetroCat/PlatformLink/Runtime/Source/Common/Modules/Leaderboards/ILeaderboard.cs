using System;

namespace RetroCat.PlatformLink.Runtime.Source.Common.Modules.Leaderboards
{
    public interface ILeaderboard
    {
        public void SetScore(string leaderBoardName, int score, Action<bool> onCompleted = null);
        public void GetScore(string leaderBoardName, Action<bool, int> onCompleted);
    }
}