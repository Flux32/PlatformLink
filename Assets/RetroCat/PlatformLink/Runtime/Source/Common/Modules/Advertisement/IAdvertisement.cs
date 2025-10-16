using System;

namespace RetroCat.PlatformLink.Runtime.Source.Common.Modules.Advertisement
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