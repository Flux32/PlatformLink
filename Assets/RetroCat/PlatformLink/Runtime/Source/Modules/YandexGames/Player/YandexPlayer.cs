using System;
using System.Runtime.InteropServices;
using RetroCat.PlatformLink.Runtime.Source.Common.Modules.Player;
using UnityEngine;

namespace RetroCat.PlatformLink.Runtime.Source.Modules.YandexGames.Player
{
    public class YandexPlayer : MonoBehaviour, IPlayer
    {
        private Action<bool> _authorizeCompleted;

        [DllImport("__Internal")]
        private static extern int jslib_isPlayerAuthorized();

        [DllImport("__Internal")]
        private static extern void jslib_openAuthDialog();

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

        public string Name => string.Empty;//jslib_getPlayerName();

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
        #endregion
    }
}
