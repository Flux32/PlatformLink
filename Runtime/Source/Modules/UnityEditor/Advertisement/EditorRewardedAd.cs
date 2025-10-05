#if UNITY_EDITOR
using System;
using PlatformLink.PluginDebug;
using PlatformLink.Common;

namespace PlatformLink.Platform.UnityEditor
{
    public class EditorRewardedAd : IRewardedAd
    {
        public event Action Opened;
        public event Action Closed;
        public event Action Failed;
        public event Action<Reward> Rewarded;

        private readonly ILogger _logger;

        public bool IsOpened { get; private set; }

        public EditorRewardedAd(ILogger logger)
        {
            _logger = logger;
        }

        public bool CanShow()
        {
            return true;
        }

        public void Show()
        {
            _logger.Log("rewarded ad opened");
            Opened?.Invoke();
            _logger.Log("rewarded");
            Rewarded?.Invoke(new Reward("reward", 1));
            _logger.Log("rewarded ad closed");
            Closed?.Invoke();
        }
    }
}
#endif