using System;
using RetroCat.PlatformLink.Runtime.Source.Common.Modules.Advertisement.Adapters;
using ILogger = PlatformLink.PluginDebug.ILogger;

namespace PlatformLink.Platform.Android
{
    public class YandexMobileAdsInterstitialAdapter : IInterstitialAdAdapter
    {
        private const string NotImplementedMessage = "YandexMobileAdsInterstitialAdapter is not implemented.";

        private readonly ILogger _logger;

#pragma warning disable CS0067
        public event Action Opened;
        public event Action Closed;
        public event Action Failed;
#pragma warning restore CS0067

        public bool IsOpened => false;
        public bool NoAdMode { get; set; }

        public YandexMobileAdsInterstitialAdapter(ILogger logger)
        {
            _logger = logger;
        }

        public void Show()
        {
            _logger.LogWarning(NotImplementedMessage);
        }

        public bool CanShow()
        {
            return false;
        }
    }
}
