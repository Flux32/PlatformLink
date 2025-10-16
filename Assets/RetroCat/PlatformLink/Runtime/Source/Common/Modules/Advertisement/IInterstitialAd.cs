using System;

namespace RetroCat.PlatformLink.Runtime.Source.Common.Modules.Advertisement
{
    public interface IInterstitialAd
    {
        public event Action Opened;
        public event Action Closed;
        public event Action Failed;

        public void Show();
        public bool CanShow();
        public bool IsOpened { get; }
    }
}