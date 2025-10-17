using System;
using System.Runtime.InteropServices;
using UnityEngine;
using RetroCat.PlatformLink.Runtime.Source.Common.Modules.Advertisement;

namespace PlatformLink.Platform.YandexGames
{
    public class YandexInterstitialAd : MonoBehaviour, IInterstitialAd
    {
        public event Action Opened;
        public event Action Closed;
        public event Action Failed;

        public bool IsOpened { get; private set; }
        public bool NoAdMode { get; set; }

        [DllImport("__Internal")]
        private static extern void jslib_showInterstitialAd();

        public void Show()
        {
            if (NoAdMode)
            {
                Failed?.Invoke();
                return;
            }
            IsOpened = true;
            jslib_showInterstitialAd();
        }

        public bool CanShow()
        {
            return NoAdMode == false;
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
