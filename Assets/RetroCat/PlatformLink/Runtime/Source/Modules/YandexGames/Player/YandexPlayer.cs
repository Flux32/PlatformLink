using System;
using System.Runtime.InteropServices;
using UnityEngine;
using RetroCat.PlatformLink.Runtime.Source.Common.Modules.Player;

namespace PlatformLink.Platform.YandexGames
{
    public class YandexPlayer : MonoBehaviour, IPlayer
    {
        private Action<bool> AuthorizeCompleted;

        public bool Authorized { get; private set; }

        public void Authorize(Action<bool> onCompleted)
        {
            AuthorizeCompleted = onCompleted;
        }

        //[DllImport("__Internal")]
        //private static extern string jslib_getPlayerName();

        public string Name => "test";//jslib_getPlayerName();

        #region Called from PlatformLink.js
        private void fjs_onAuthorized()
        {
            AuthorizeCompleted?.Invoke(true);
        }

        private void fjs_onAuthorizedFailed()
        {
            AuthorizeCompleted?.Invoke(false);
        }
        #endregion
    }
}