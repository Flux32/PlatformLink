#if UNITY_EDITOR
using System;
using RetroCat.PlatformLink.Runtime.Source.Common.Modules.Advertisement;
using ILogger = PlatformLink.PluginDebug.ILogger;

namespace PlatformLink.Platform.UnityEditor
{
    public class EditorRewardedAd : IRewardedAd
    {
        private const string RewardedOpenedMessage = "rewarded opened";
        private const string RewardedClosedMessage = "rewarded closed";
        private const string RewardReceivedMessage = "reward received";
        private const string RewardedRejectMessage = "reward rejected";
        
        private readonly ILogger _logger;
        private readonly EditorRewardedView _rewardedClient;
        
        public EditorRewardedAd(ILogger logger, EditorRewardedView rewardedClient)
        {
            _logger = logger;
            _rewardedClient = rewardedClient;
            
            _rewardedClient.Opened += OnOpened;
            _rewardedClient.Rewarded += OnRewarded;
            _rewardedClient.RewardRejected += OnRewardRejected;
            _rewardedClient.Closed += OnClosed;
        }

        public event Action Opened;
        public event Action Closed;
        public event Action Failed;
        public event Action<Reward> Rewarded;
        
        public bool IsOpened { get; private set; }

        private void OnRewarded()
        {
            Rewarded?.Invoke(new Reward("reward", 1));
            _logger.Log(RewardReceivedMessage);
        }
        
        private void OnRewardRejected()
        {
            _logger.Log(RewardedRejectMessage);
            Failed?.Invoke();
        }
        
        private void OnOpened()
        {
            IsOpened = true;
            _logger.Log(RewardedOpenedMessage);
            Opened?.Invoke();
        }

        private void OnClosed()
        {
            IsOpened = false;
            _logger.Log(RewardedClosedMessage);
            Closed?.Invoke();
        }

        public bool CanShow()
        {
            return true;
        }

        public void Show()
        {
            _rewardedClient.Open();
        }
    }
}
#endif