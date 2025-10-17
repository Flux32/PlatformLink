#if UNITY_EDITOR
using System;
using RetroCat.PlatformLink.Runtime.Source.Common.Modules.Advertisement;
using UnityEditor.PackageManager;
using ILogger = PlatformLink.PluginDebug.ILogger;

namespace PlatformLink.Platform.UnityEditor
{
    public class EditorInterstitialAd : IInterstitialAd
    {
        private readonly ILogger _logger;

        private const string InterstitialOpenedMessage = "interstitial opened";
        private const string InterstitialClosedMessage = "interstitial closed";

        private readonly EditorInterstitialView _interstitialClient;
        
        public EditorInterstitialAd(ILogger logger, 
            EditorInterstitialView editorClient)
        {
            _logger = logger;
            _interstitialClient = editorClient;

            _interstitialClient.Opened += OnOpened;
            _interstitialClient.Closed += OnClosed;
        }
        
        public event Action Opened;
        public event Action Closed;
        public event Action Failed;

        public bool IsOpened { get; private set; }
        public bool NoAdMode { get; set; }

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
            if (NoAdMode)
            {
                Failed?.Invoke();
                return;
            }
            
            if (IsOpened)
                _logger.LogWarning("Attempt to show interstitial on top of open interstitial");
            else
                _interstitialClient.Open();
        }

        public bool CanShow()
        {
            return IsOpened == false && NoAdMode == false;
        }
    }
}
#endif