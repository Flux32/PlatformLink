using System;
using System.Runtime.InteropServices;
using RetroCat.PlatformLink.Runtime.Source.Common.Modules.Platform;
using UnityEngine;

namespace RetroCat.PlatformLink.Runtime.Source.Modules.YandexGames.Platform
{
    public class YandexPlatform : MonoBehaviour, IPlatform
    {
        private Action<bool> _authorizeCompleted;
        private Action<bool, AvailableGames> _getAllGamesCompleted;

        [DllImport("__Internal")]
        private static extern int jslib_isPlayerAuthorized();

        [DllImport("__Internal")]
        private static extern void jslib_openAuthDialog();

        [DllImport("__Internal")]
        private static extern void jslib_getAllGames();

        public bool Authorized => IsAuthorizedInternal();

        public void Authorize(Action<bool> onCompleted)
        {
            _authorizeCompleted = onCompleted;

            if (IsAuthorizedInternal())
            {
                _authorizeCompleted?.Invoke(true);
                return;
            }

            jslib_openAuthDialog();
        }

        //[DllImport("__Internal")]
        //private static extern string jslib_getPlayerName();

        public string Name => string.Empty; //jslib_getPlayerName();

        public void GetAllGames(Action<bool, AvailableGames> onCompleted)
        {
            _getAllGamesCompleted = onCompleted;
            jslib_getAllGames();
        }

        private bool IsAuthorizedInternal()
        {
            return jslib_isPlayerAuthorized() == 1;
        }

        #region Called from PlatformLink.js
        private void fjs_onAuthorized()
        {
            _authorizeCompleted?.Invoke(true);
        }

        private void fjs_onAuthorizedFailed()
        {
            _authorizeCompleted?.Invoke(false);
        }

        private void fjs_onGetAllGamesSuccess(string json)
        {
            try
            {
                var payload = JsonUtility.FromJson<AllGamesPayload>(json);
                if (payload == null)
                {
                    _getAllGamesCompleted?.Invoke(false, new AvailableGames(Array.Empty<AvailableGame>(), string.Empty));
                    return;
                }

                var gamesJson = payload.games ?? Array.Empty<GameJson>();
                var games = new AvailableGame[gamesJson.Length];
                for (int i = 0; i < gamesJson.Length; i++)
                {
                    var g = gamesJson[i];
                    games[i] = new AvailableGame(
                        g.appID ?? string.Empty,
                        g.title ?? string.Empty,
                        g.url ?? string.Empty,
                        g.coverURL ?? string.Empty,
                        g.iconURL ?? string.Empty);
                }

                _getAllGamesCompleted?.Invoke(true, new AvailableGames(games, payload.developerURL ?? string.Empty));
            }
            catch (Exception exception)
            {
                Debug.LogWarning($"YandexPlatform: Failed to parse games payload. {exception.Message}");
                _getAllGamesCompleted?.Invoke(false, new AvailableGames(Array.Empty<AvailableGame>(), string.Empty));
            }
        }

        private void fjs_onGetAllGamesFailed()
        {
            _getAllGamesCompleted?.Invoke(false, new AvailableGames(Array.Empty<AvailableGame>(), string.Empty));
        }
        #endregion

        [Serializable]
        private class AllGamesPayload
        {
            public GameJson[] games;
            public string developerURL;
        }

        [Serializable]
        private class GameJson
        {
            public string appID;
            public string title;
            public string url;
            public string coverURL;
            public string iconURL;
        }
    }
}
