#if UNITY_EDITOR
using RetroCat.PlatformLink.Runtime.Source.Common.Modules.Leaderboards;
using UnityEngine;

namespace RetroCat.PlatformLink.Runtime.Source.Modules.UnityEditor.Leaderboards
{
    public class EditorLeaderboard : ILeaderboard
    {
        public void SetScore(string leaderBoardName, int score)
        {
            PlayerPrefs.SetInt(leaderBoardName, score);
        }
    }
}
#endif