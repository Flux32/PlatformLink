using System;
using RetroCat.PlatformLink.Runtime.Source.Common.Modules.Advertisement.Adapters;

namespace RetroCat.PlatformLink.Runtime.Source.Common.Modules.Advertisement
{
    public class RewardedAdModule : IRewardedAd
    {
        private readonly IRewardedAdAdapter _adapter;

        public event Action Opened;
        public event Action Closed;
        public event Action Failed;
        public event Action<Reward> Rewarded;

        public bool IsOpened => _adapter.IsOpened;

        public RewardedAdModule(IRewardedAdAdapter adapter)
        {
            _adapter = adapter;
            _adapter.Opened += OnOpened;
            _adapter.Closed += OnClosed;
            _adapter.Failed += OnFailed;
            _adapter.Rewarded += OnRewarded;
        }

        public void Show()
        {
            _adapter.Show();
        }

        public bool CanShow()
        {
            return _adapter.CanShow();
        }

        private void OnOpened() => Opened?.Invoke();
        private void OnClosed() => Closed?.Invoke();
        private void OnFailed() => Failed?.Invoke();
        private void OnRewarded(Reward reward) => Rewarded?.Invoke(reward);
    }
}
