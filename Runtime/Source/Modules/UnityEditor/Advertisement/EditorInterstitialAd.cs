#if UNITY_EDITOR
using System;
using PlatformLink.Common;
using ILogger = PlatformLink.PluginDebug.ILogger;

namespace PlatformLink.Platform.UnityEditor
{
    public class EditorInterstitialAd : IInterstitialAd
    {
        public event Action Opened;
        public event Action Closed;
        public event Action Failed;

        private readonly ILogger _logger;

        private const string InterstitialOpenedMessage = "interstitial opened";
        private const string InterstitialClosedMessage = "interstitial closed";

        private readonly EditorInterstitialView _interstitialClient;

        public bool IsOpened { get; private set; }

        public EditorInterstitialAd(ILogger logger, EditorInterstitialView editorClient)
        {
            _logger = logger;
            _interstitialClient = editorClient;

            _interstitialClient.Opened += OnOpened;
            _interstitialClient.Closed += OnClosed;
        }

        private void OnClosed()
        {
            IsOpened = false;
            _logger.Log(InterstitialClosedMessage);
            Closed?.Invoke();
        }

        private void OnOpened()
        {
            IsOpened = true;
            _logger.Log(InterstitialOpenedMessage);
            Opened?.Invoke();
        }

        public void Show()
        {
            if (IsOpened == true)
                _logger.LogWarning("Attempt to show interstitial on top of open interstitial");
            else
                _interstitialClient.Open();
        }

        public bool CanShow()
        {
            return IsOpened == false;
        }
    }
}
#endif