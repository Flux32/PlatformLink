using System;
using RetroCat.PlatformLink.Runtime.Source.Common.Modules.Advertisement.Adapters;

namespace RetroCat.PlatformLink.Runtime.Source.Common.Modules.Advertisement
{
    public class InterstitialAdModule : IInterstitialAd
    {
        private readonly IInterstitialAdAdapter _adapter;

        public event Action Opened;
        public event Action Closed;
        public event Action Failed;

        public bool IsOpened => _adapter.IsOpened;

        public bool NoAdMode
        {
            get => _adapter.NoAdMode;
            set => _adapter.NoAdMode = value;
        }

        public InterstitialAdModule(IInterstitialAdAdapter adapter)
        {
            _adapter = adapter;
            _adapter.Opened += OnOpened;
            _adapter.Closed += OnClosed;
            _adapter.Failed += OnFailed;
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
    }
}
