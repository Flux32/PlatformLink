using System;

namespace RetroCat.PlatformLink.Runtime.Source.Common.Modules.Advertisement
{
    public interface IRewardedAd
    {
        public event Action Opened;
        public event Action Closed;
        public event Action Failed;
        public event Action<Reward> Rewarded;

        public bool IsOpened { get; }
        public bool CanShow();
        public void Show();
    }
}