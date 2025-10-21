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
    }
}
#endif