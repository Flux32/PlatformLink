using System;

namespace RetroCat.PlatformLink.Runtime.Source.Common.Modules.Leaderboards
{
    [Serializable]
    public class LeaderboardRange
    {
        public int Start { get; }
        public int Size { get; }

        public LeaderboardRange(int start, int size)
        {
            Start = start;
            Size = size;
        }
    }
}

