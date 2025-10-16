using System;

namespace RetroCat.PlatformLink.Runtime.Source.Common.Modules.Advertisement
{
    public class Advertisement : IAdvertisement
    {
        public event Action AdOpened;
        public event Action AdClosed;
        public event Action Failed;

        public IInterstitialAd InterstetialAd { get; private set; }
        public IRewardedAd RewardedAd { get; private set; }

        public bool IsAdOpened => RewardedAd.IsOpened == true || InterstetialAd.IsOpened == true;

        public Advertisement(IInterstitialAd interstetialAd, IRewardedAd rewardedAd)
        {
            InterstetialAd = interstetialAd;
            RewardedAd = rewardedAd;

            interstetialAd.Opened += OnAdOpened;
            rewardedAd.Opened += OnAdOpened;

            interstetialAd.Closed += OnAdClosed;
            rewardedAd.Closed += OnAdClosed;

            interstetialAd.Failed += OnAdFailed;
            rewardedAd.Failed += OnAdFailed;
        }

        private void OnAdOpened()
        {
            AdOpened?.Invoke();
        }

        private void OnAdClosed()
        {
            AdClosed?.Invoke();
        }

        private void OnAdFailed()
        {
            Failed?.Invoke();
        }
    }
}