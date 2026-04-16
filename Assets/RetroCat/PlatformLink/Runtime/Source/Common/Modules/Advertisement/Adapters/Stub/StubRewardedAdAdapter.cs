using System;
using ILogger = PlatformLink.PluginDebug.ILogger;

namespace RetroCat.PlatformLink.Runtime.Source.Common.Modules.Advertisement.Adapters.Stub
{
    public class StubRewardedAdAdapter : IRewardedAdAdapter
    {
        private const string NotConfiguredMessage = "RewardedAd module is not configured for the active platform.";

        private readonly ILogger _logger;

#pragma warning disable CS0067
        public event Action Opened;
        public event Action Closed;
        public event Action Failed;
        public event Action<Reward> Rewarded;
#pragma warning restore CS0067

        public bool IsOpened => false;

        public StubRewardedAdAdapter(ILogger logger)
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
