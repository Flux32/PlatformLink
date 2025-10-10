using System;
using System.Runtime.InteropServices;
using UnityEngine;
using PlatformLink.Common;

namespace PlatformLink.Platform.YandexGames
{
    public class YandexInterstitialAd : MonoBehaviour, IInterstitialAd
    {
        public event Action Opened;
        public event Action Closed;
        public event Action Failed;

        public bool IsOpened { get; private set; }

        [DllImport("__Internal")]
        private static extern void jslib_showInterstitialAd();

        public void Show()
        {
            IsOpened = true;
            jslib_showInterstitialAd();
        }

        public bool CanShow()
        {
            return true;
        }

        #region Called from PlatformLink.js
        private void fjs_onInterstetialAdOpened()
        {
            Opened?.Invoke();
        }

        private void fjs_onInterstetialAdClosed()
        {
            IsOpened = false;
            Closed?.Invoke();
        }

        private void fjs_onInterstetialAdFailed()
        {
            IsOpened = false;
            Failed?.Invoke();
        }
        #endregion
    }
}
