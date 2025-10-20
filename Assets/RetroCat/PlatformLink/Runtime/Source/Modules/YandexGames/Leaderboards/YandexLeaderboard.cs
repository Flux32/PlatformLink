using System;
using System.Runtime.InteropServices;
using RetroCat.PlatformLink.Runtime.Source.Common.Modules.Leaderboards;
using UnityEngine;

namespace RetroCat.PlatformLink.Runtime.Source.Modules.YandexGames.Leaderboards
{
    public class YandexLeaderboard : MonoBehaviour, ILeaderboard
    {
        private Action<bool, int> GetScoreCompleted;
        
        [DllImport("__Internal")]
        private static extern void jslib_setLeaderboardScore(string leaderboardId, int score);
        
        public void SetScore(string leaderboardId, int score)
        {
            jslib_setLeaderboardScore(leaderboardId, score);
        }

        private void fjs_onGetScoreSuccess(int score)
        {
            GetScoreCompleted?.Invoke(true, score);
        }

        private void fjs_onGetScoreFailed()
        {
            GetScoreCompleted?.Invoke(true, 0);
        }
    }
}