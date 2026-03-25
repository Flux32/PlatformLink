using System;
using System.Runtime.InteropServices;
using UnityEngine;
using RetroCat.PlatformLink.Runtime.Source.Common.Modules.Advertisement;

namespace PlatformLink.Platform.YandexGames
{
    public class YandexRewardedAd : MonoBehaviour, IRewardedAd
    {
        public event Action Opened;
        public event Action Closed;
        public event Action Failed;
        public event Action<Reward> Rewarded;

        public bool IsOpened { get; private set; }

        [DllImport("__Internal")]
        private static extern void jslib_showRewardedAd();

        public bool CanShow()
        {
            return IsOpened == false;
        }

        public void Show()
        {
            if (IsOpened)
            {
                return;
            }

            IsOpened = true;
            jslib_showRewardedAd();
        }

        #region Called from PlatformLink.js
        private void fjs_onRewardedAdOpened()
        {
            Opened?.Invoke();
        }

        private void fjs_onRewardedAdClosed()
        {
            IsOpened = false;
            Closed?.Invoke();
        }

        private void fjs_onRewardedAdError()
        {
            IsOpened = false;
            Failed?.Invoke();
        }

        private void fjs_onRewarded()
        {
            Rewarded?.Invoke(new Reward("Reward", 1));
        }
        #endregion
    }
}
