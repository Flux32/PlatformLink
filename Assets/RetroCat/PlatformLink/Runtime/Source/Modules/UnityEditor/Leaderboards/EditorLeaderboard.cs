#if UNITY_EDITOR
using RetroCat.PlatformLink.Runtime.Source.Common.Modules.Leaderboards;
using UnityEngine;
using ILogger = PlatformLink.PluginDebug.ILogger;

namespace RetroCat.PlatformLink.Runtime.Source.Modules.UnityEditor.Leaderboards
{
    public class EditorLeaderboard : ILeaderboard
    {
        private readonly ILogger _logger;

        public EditorLeaderboard(ILogger logger)
        {
            _logger = logger;
        }
        
        public void SetScore(string leaderboardId, int score)
        {
            PlayerPrefs.SetInt(leaderboardId, score);
            _logger.Log($"Set score: {score}");
        }

        public void GetPlayerEntry(string leaderboardId, System.Action<bool, LeaderboardEntry> onCompleted)
        {
            int score = PlayerPrefs.GetInt(leaderboardId, 0);
            var player = new LeaderboardPlayer(
                Application.systemLanguage.ToString(),
                "Editor Player",
                SystemInfo.deviceUniqueIdentifier,
                string.Empty,
                "granted",
                "granted");

            var entry = new LeaderboardEntry(
                score,
                string.Empty,
                0,
                player,
                score.ToString());

            onCompleted?.Invoke(true, entry);
        }

        public void GetEntries(
            string leaderboardId,
            int quantityTop,
            bool includeUser,
            int quantityAround,
            System.Action<bool, LeaderboardEntries> onCompleted)
        {
            int score = PlayerPrefs.GetInt(leaderboardId, 0);
            var player = new LeaderboardPlayer(
                Application.systemLanguage.ToString(),
                "Editor Player",
                SystemInfo.deviceUniqueIdentifier,
                string.Empty,
                "granted",
                "granted");

            var entry = new LeaderboardEntry(
                score,
                string.Empty,
                0,
                player,
                score.ToString());

            var ranges = new[]
            {
                new LeaderboardRange(0, 1)
            };

            var entries = new[]
            {
                entry
            };

            var result = new LeaderboardEntries(
                leaderboardId,
                0,
                ranges,
                entries);

            onCompleted?.Invoke(true, result);
        }
    }
}
#endif
