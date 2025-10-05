using System;
using System.Runtime.InteropServices;
using UnityEngine;
using PlatformLink.Common;

namespace PlatformLink.Platform.YandexGames
{
    public class YandexRewardedAd : MonoBehaviour, IRewardedAd
    {
        public event Action Opened;
        public event Action Closed;
        public event Action Failed;
        public event Action<Reward> Rewarded;

        public bool IsOpened => throw new NotImplementedException();

        [DllImport("__Internal")]
        private static extern void jslib_showRewardedAd();

        public bool CanShow()
        {
            return true;
        }

        public void Show()
        {
            jslib_showRewardedAd();
        }

        #region Called from PlatformLink.js
        private void fjs_onRewardedAdOpened()
        {
            Opened?.Invoke();
        }

        private void fjs_onRewardedAdClosed()
        {
            Closed?.Invoke();
        }

        private void fjs_onRewardedAdFailed()
        {
            Failed?.Invoke();
        }

        private void fjs_onRewarded()
        {
            Rewarded?.Invoke(new Reward("Reward", 1));
        }
        #endregion
    }
}