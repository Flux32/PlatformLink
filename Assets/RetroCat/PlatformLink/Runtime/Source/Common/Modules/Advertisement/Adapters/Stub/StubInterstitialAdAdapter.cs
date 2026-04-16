using System;
using ILogger = PlatformLink.PluginDebug.ILogger;

namespace RetroCat.PlatformLink.Runtime.Source.Common.Modules.Advertisement.Adapters.Stub
{
    public class StubInterstitialAdAdapter : IInterstitialAdAdapter
    {
        private const string NotConfiguredMessage = "InterstitialAd module is not configured for the active platform.";

        private readonly ILogger _logger;

#pragma warning disable CS0067
        public event Action Opened;
        public event Action Closed;
        public event Action Failed;
#pragma warning restore CS0067

        public bool IsOpened => false;
        public bool NoAdMode { get; set; }

        public StubInterstitialAdAdapter(ILogger logger)
        {
            _logger = logger;
        }

        public void Show()
        {
            _logger.LogWarning(NotConfiguredMessage);
        }

        public bool CanShow()
        {
            return false;
        }
    }
}
