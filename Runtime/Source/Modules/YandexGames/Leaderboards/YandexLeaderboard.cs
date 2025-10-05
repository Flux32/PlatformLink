/*
using System;
using System.Runtime.InteropServices;
using UnityEngine;
using PlatformLink.Common;

namespace PlatformLink.Platform
{
    public class YandexLeaderboard : MonoBehaviour, ILeaderboard
    {
        private Action<bool> SetScoreCompleted;
        private Action<bool, int> GetScoreCompleted;

        [DllImport("__Internal")]
        private static extern void jslib_getLeaderBoardScore();

        public void GetScore(string leaderBoardName, Action<bool, int> onCompleted)
        {
            GetScoreCompleted = onCompleted;
            jslib_getLeaderBoardScore();
        }

        [DllImport("__Internal")]
        private static extern void jslib_setLeaderBoardScore(int score);

        public void SetScore(string leaderBoardName, int score, Action<bool> onCompleted = null)
        {
            SetScoreCompleted = onCompleted;
            jslib_setLeaderBoardScore(score);
        }

        private void fjs_onGetScoreSuccess(int score)
        {
            GetScoreCompleted?.Invoke(true, score);
        }

        private void fjs_onGetScoreFailed()
        {
            GetScoreCompleted?.Invoke(true, 0);
        }

        private void fjs_onSetScoreSuccess()
        {
            SetScoreCompleted?.Invoke(true);
        }

        private void fjs_onSetScoreFailed()
        {
            SetScoreCompleted?.Invoke(false);
        }
    }
}
*/