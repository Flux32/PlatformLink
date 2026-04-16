using System;

namespace RetroCat.PlatformLink.Runtime.Source.Common.Modules.Advertisement.Adapters
{
    public interface IRewardedAdAdapter
    {
        event Action Opened;
        event Action Closed;
        event Action Failed;
        event Action<Reward> Rewarded;

        bool IsOpened { get; }

        void Show();
        bool CanShow();
    }
}
