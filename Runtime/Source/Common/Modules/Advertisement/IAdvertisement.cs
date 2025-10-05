using System;

namespace PlatformLink.Common
{
    public interface IAdvertisement
    {
        public event Action AdOpened;
        public event Action AdClosed;
        public event Action Failed;
        public IInterstitialAd InterstetialAd { get; }
        public IRewardedAd RewardedAd { get; }
    }
}