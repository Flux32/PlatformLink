using System;

namespace RetroCat.PlatformLink.Runtime.Source.Common.Modules.Advertisement.Adapters
{
    public interface IInterstitialAdAdapter
    {
        event Action Opened;
        event Action Closed;
        event Action Failed;

        bool IsOpened { get; }
        bool NoAdMode { get; set; }

        void Show();
        bool CanShow();
    }
}
