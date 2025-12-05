using System;
using System.Runtime.InteropServices;
using RetroCat.PlatformLink.Runtime.Source.Common.Modules.Leaderboards;
using UnityEngine;

namespace RetroCat.PlatformLink.Runtime.Source.Modules.YandexGames.Leaderboards
{
    public class YandexLeaderboard : MonoBehaviour, ILeaderboard
    {
        private Action<bool, int> GetScoreCompleted;
        private Action<bool, LeaderboardEntry> _getPlayerEntryCompleted;
        private Action<bool, LeaderboardEntries> _getEntriesCompleted;
        
        [DllImport("__Internal")]
        private static extern void jslib_setLeaderboardScore(string leaderboardId, int score);
        [DllImport("__Internal")]
        private static extern void jslib_getLeaderboardPlayerEntry(string leaderboardId);
        [DllImport("__Internal")]
        private static extern void jslib_getLeaderboardEntries(string leaderboardId, int includeUser, int quantityAround, int quantityTop);
        
        public void SetScore(string leaderboardId, int score)
        {
            jslib_setLeaderboardScore(leaderboardId, score);
        }

        public void GetPlayerEntry(string leaderboardId, Action<bool, LeaderboardEntry> onCompleted)
        {
            _getPlayerEntryCompleted = onCompleted;
            jslib_getLeaderboardPlayerEntry(leaderboardId);
        }

        public void GetEntries(
            string leaderboardId,
            int quantityTop,
            bool includeUser,
            int quantityAround,
            Action<bool, LeaderboardEntries> onCompleted)
        {
            _getEntriesCompleted = onCompleted;
            jslib_getLeaderboardEntries(leaderboardId, includeUser ? 1 : 0, quantityAround, quantityTop);
        }

        private void fjs_onGetScoreSuccess(int score)
        {
            GetScoreCompleted?.Invoke(true, score);
        }

        private void fjs_onGetScoreFailed()
        {
            GetScoreCompleted?.Invoke(true, 0);
        }

        private void fjs_onGetPlayerEntrySuccess(string json)
        {
            try
            {
                var dto = JsonUtility.FromJson<PlayerEntryJson>(json);
                if (dto == null)
                {
                    _getPlayerEntryCompleted?.Invoke(false, null);
                    return;
                }

                var playerDto = dto.player ?? new PlayerJson();
                var permissions = playerDto.scopePermissions ?? new PlayerScopePermissionsJson();

                var player = new LeaderboardPlayer(
                    playerDto.lang ?? string.Empty,
                    playerDto.publicName ?? string.Empty,
                    playerDto.uniqueID ?? string.Empty,
                    playerDto.avatarUrl ?? string.Empty,
                    permissions.avatar ?? string.Empty,
                    permissions.public_name ?? string.Empty);

                var entry = new LeaderboardEntry(
                    dto.score,
                    dto.extraData ?? string.Empty,
                    dto.rank,
                    player,
                    dto.formattedScore ?? string.Empty);

                _getPlayerEntryCompleted?.Invoke(true, entry);
            }
            catch (Exception e)
            {
                Debug.LogWarning($"YandexLeaderboard: Failed to parse player entry payload. {e.Message}");
                _getPlayerEntryCompleted?.Invoke(false, null);
            }
        }

        private void fjs_onGetPlayerEntryNotPresent()
        {
            _getPlayerEntryCompleted?.Invoke(true, null);
        }

        private void fjs_onGetPlayerEntryFailed()
        {
            _getPlayerEntryCompleted?.Invoke(false, null);
        }

        private void fjs_onGetLeaderboardEntriesSuccess(string json)
        {
            try
            {
                var dto = JsonUtility.FromJson<LeaderboardEntriesWrapperJson>(json);
                if (dto == null)
                {
                    _getEntriesCompleted?.Invoke(false, null);
                    return;
                }

                LeaderboardRange[] ranges = Array.Empty<LeaderboardRange>();
                if (dto.ranges != null && dto.ranges.Length > 0)
                {
                    ranges = new LeaderboardRange[dto.ranges.Length];
                    for (int i = 0; i < dto.ranges.Length; i++)
                    {
                        var r = dto.ranges[i];
                        ranges[i] = new LeaderboardRange(r.start, r.size);
                    }
                }

                LeaderboardEntry[] entries = Array.Empty<LeaderboardEntry>();
                if (dto.entries != null && dto.entries.Length > 0)
                {
                    entries = new LeaderboardEntry[dto.entries.Length];
                    for (int i = 0; i < dto.entries.Length; i++)
                    {
                        var e = dto.entries[i];
                        var playerDto = e.player ?? new PlayerJson();
                        var permissions = playerDto.scopePermissions ?? new PlayerScopePermissionsJson();

                        var player = new LeaderboardPlayer(
                            playerDto.lang ?? string.Empty,
                            playerDto.publicName ?? string.Empty,
                            playerDto.uniqueID ?? string.Empty,
                            playerDto.avatarUrl ?? string.Empty,
                            permissions.avatar ?? string.Empty,
                            permissions.public_name ?? string.Empty);

                        entries[i] = new LeaderboardEntry(
                            e.score,
                            e.extraData ?? string.Empty,
                            e.rank,
                            player,
                            e.formattedScore ?? string.Empty);
                    }
                }

                var result = new LeaderboardEntries(
                    dto.leaderboardId ?? string.Empty,
                    dto.userRank,
                    ranges,
                    entries);

                _getEntriesCompleted?.Invoke(true, result);
            }
            catch (Exception e)
            {
                Debug.LogWarning($"YandexLeaderboard: Failed to parse leaderboard entries payload. {e.Message}");
                _getEntriesCompleted?.Invoke(false, null);
            }
        }

        private void fjs_onGetLeaderboardEntriesFailed()
        {
            _getEntriesCompleted?.Invoke(false, null);
        }

        [Serializable]
        private class PlayerScopePermissionsJson
        {
            public string avatar;
            public string public_name;
        }

        [Serializable]
        private class PlayerJson
        {
            public string lang;
            public string publicName;
            public string uniqueID;
            public string avatarUrl;
            public PlayerScopePermissionsJson scopePermissions;
        }

        [Serializable]
        private class PlayerEntryJson
        {
            public int score;
            public string extraData;
            public int rank;
            public PlayerJson player;
            public string formattedScore;
        }

        [Serializable]
        private class LeaderboardRangeJson
        {
            public int start;
            public int size;
        }

        [Serializable]
        private class LeaderboardEntriesWrapperJson
        {
            public string leaderboardId;
            public LeaderboardRangeJson[] ranges;
            public int userRank;
            public PlayerEntryJson[] entries;
        }
    }
}
