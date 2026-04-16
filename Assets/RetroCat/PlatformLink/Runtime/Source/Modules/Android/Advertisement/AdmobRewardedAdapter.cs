using System;
using RetroCat.PlatformLink.Runtime.Source.Common.Modules.Advertisement;
using RetroCat.PlatformLink.Runtime.Source.Common.Modules.Advertisement.Adapters;
using ILogger = PlatformLink.PluginDebug.ILogger;

namespace PlatformLink.Platform.Android
{
    public class AdmobRewardedAdapter : IRewardedAdAdapter
    {
        private const string NotImplementedMessage = "AdmobRewardedAdapter is not implemented.";

        private readonly ILogger _logger;

#pragma warning disable CS0067
        public event Action Opened;
        public event Action Closed;
        public event Action Failed;
        public event Action<Reward> Rewarded;
#pragma warning restore CS0067

        public bool IsOpened => false;

        public AdmobRewardedAdapter(ILogger logger)
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
