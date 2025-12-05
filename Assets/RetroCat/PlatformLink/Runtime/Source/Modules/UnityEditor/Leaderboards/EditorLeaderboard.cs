#if UNITY_EDITOR
using RetroCat.PlatformLink.Runtime.Source.Common.Modules.Leaderboards;
using UnityEngine;
using ILogger = PlatformLink.PluginDebug.ILogger;

namespace RetroCat.PlatformLink.Runtime.Source.Modules.UnityEditor.Leaderboards
{
    public class EditorLeaderboard : ILeaderboard
    {
        private readonly ILogger _logger;
        private readonly LeaderboardEditorSettings _settings;

        public EditorLeaderboard(ILogger logger, LeaderboardEditorSettings settings)
        {
            _logger = logger;
            _settings = settings;
        }
        
        public void SetScore(string leaderboardId, int score)
        {
            PlayerPrefs.SetInt(leaderboardId, score);
            _logger.Log($"Set score: {score}");
        }

        public void GetPlayerEntry(string leaderboardId, System.Action<bool, LeaderboardEntry> onCompleted)
        {
            int score = PlayerPrefs.GetInt(leaderboardId, 0);

            int rank = 1;
            if (_settings != null && _settings.OtherPlayers != null)
            {
                foreach (var mock in _settings.OtherPlayers)
                {
                    if (mock == null)
                        continue;

                    if (mock.Score > score)
                        rank++;
                }
            }

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
                rank,
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
            int userScore = PlayerPrefs.GetInt(leaderboardId, 0);

            var userPlayer = new LeaderboardPlayer(
                Application.systemLanguage.ToString(),
                "Editor Player",
                SystemInfo.deviceUniqueIdentifier,
                string.Empty,
                "granted",
                "granted");

            var userEntry = new LeaderboardEntry(
                userScore,
                string.Empty,
                0,
                userPlayer,
                userScore.ToString());

            var entriesList = new System.Collections.Generic.List<LeaderboardEntry>();

            if (_settings != null && _settings.OtherPlayers != null)
            {
                foreach (var mock in _settings.OtherPlayers)
                {
                    if (mock == null)
                        continue;

                    var player = new LeaderboardPlayer(
                        string.IsNullOrEmpty(mock.Lang) ? Application.systemLanguage.ToString() : mock.Lang,
                        string.IsNullOrEmpty(mock.PublicName) ? "Player" : mock.PublicName,
                        string.IsNullOrEmpty(mock.UniqueId) ? System.Guid.NewGuid().ToString() : mock.UniqueId,
                        string.Empty,
                        "granted",
                        "granted");

                    var entry = new LeaderboardEntry(
                        mock.Score,
                        mock.ExtraData ?? string.Empty,
                        0,
                        player,
                        mock.Score.ToString());

                    entriesList.Add(entry);
                }
            }

            if (includeUser)
            {
                entriesList.Add(userEntry);
            }

            entriesList.Sort((a, b) => b.Score.CompareTo(a.Score));

            int userRank = -1;
            if (includeUser)
            {
                for (int i = 0; i < entriesList.Count; i++)
                {
                    if (entriesList[i].Player.UniqueId == userPlayer.UniqueId)
                    {
                        userRank = i + 1;
                        break;
                    }
                }
            }

            var finalEntries = new LeaderboardEntry[entriesList.Count];
            for (int i = 0; i < entriesList.Count; i++)
            {
                var entry = entriesList[i];
                int rank = i + 1;

                finalEntries[i] = new LeaderboardEntry(
                    entry.Score,
                    entry.ExtraData,
                    rank,
                    entry.Player,
                    entry.FormattedScore);
            }
            var ranges = new[]
            {
                new LeaderboardRange(0, finalEntries.Length)
            };

            var result = new LeaderboardEntries(
                leaderboardId,
                userRank,
                ranges,
                finalEntries);

            onCompleted?.Invoke(true, result);
        }
    }
}
#endif
