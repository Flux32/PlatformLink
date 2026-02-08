#if UNITY_EDITOR
using PlatformLink.Platform.UnityEditor;
using RetroCat.PlatformLink.Runtime.Source.Common.Modules;
using RetroCat.PlatformLink.Runtime.Source.Common.Modules.Advertisement;
using RetroCat.PlatformLink.Runtime.Source.Common.Modules.Analytics;
using RetroCat.PlatformLink.Runtime.Source.Common.Modules.Device;
using RetroCat.PlatformLink.Runtime.Source.Common.Modules.Environment;
using RetroCat.PlatformLink.Runtime.Source.Common.Modules.Leaderboards;
using RetroCat.PlatformLink.Runtime.Source.Common.Modules.Player;
using RetroCat.PlatformLink.Runtime.Source.Common.Modules.Purchases;
using RetroCat.PlatformLink.Runtime.Source.Common.Modules.RemoteConfig;
using RetroCat.PlatformLink.Runtime.Source.Common.Modules.Social;
using RetroCat.PlatformLink.Runtime.Source.Common.Modules.Storage;
using RetroCat.PlatformLink.Runtime.Source.Modules.UnityEditor.Analytics;
using RetroCat.PlatformLink.Runtime.Source.Modules.UnityEditor.Leaderboards;
using RetroCat.PlatformLink.Runtime.Source.Modules.UnityEditor.Purchases;
using RetroCat.PlatformLink.Runtime.Source.Modules.UnityEditor.RemoteConfig;
using RetroCat.PlatformLink.Runtime.Source.Modules.UnityEditor.Social;
using UnityEngine;
using DeviceType = RetroCat.PlatformLink.Runtime.Source.Common.Modules.Environment.DeviceType;
using ILogger = PlatformLink.PluginDebug.ILogger;

namespace RetroCat.PlatformLink.Runtime.Source.Modules.UnityEditor.Factories
{
    public class EditorModuleFactory : IModuleFactory
    {
        private const string InterstitialViewPath = "Prefabs/Ad/interstetial_editor_ad";
        private const string RewardedViewPath = "Prefabs/Ad/rewarded_editor_ad";
        
        private readonly ILogger _logger;
            
        public EditorModuleFactory(ILogger logger)
        {
            _logger = logger;
        }
        
        private EditorSettings EditorSettings => PlatformLinkSettings.Instance.Editor;
        
        public IInterstitialAd CreateInterstitialAd()
        {
            EditorInterstitialView interstitialViewPrefab = Resources.Load<EditorInterstitialView>(InterstitialViewPath);
            EditorInterstitialView interstitialView = Object.Instantiate(interstitialViewPrefab);
        
            Object.DontDestroyOnLoad(interstitialView.gameObject);
            return new EditorInterstitialAd(_logger, interstitialView);
        }

        public IRewardedAd CreateRewardedAd()
        {
            EditorRewardedView rewardedViewPrefab = Resources.Load<EditorRewardedView>(RewardedViewPath);
            EditorRewardedView rewardedView = Object.Instantiate(rewardedViewPrefab);
        
            return new EditorRewardedAd(_logger, rewardedView);
        }

        public IEnvironment CreateEnvironment()
        {
            return new EditorEnvironment(EditorSettings.Environment.Language, DeviceType.Mobile);
        }

        public IStorage CreateStorage()
        {
            return new EditorStorage(_logger, EditorSettings.Storage.SaveFilePath);
        }

        public IPurchases CreatePurchases()
        {
            return new EditorPurchases(_logger);
        }

        public IAnalytics CreateAnalytics()
        {
            return new Common.Modules.Analytics.Analytics(_logger, new IAnalyticsService[]
                { new EditorAnalyticsService(_logger) });
        }

        public ILeaderboard CreateLeaderboard()
        {
            return new EditorLeaderboard(_logger, EditorSettings.Leaderboard);
        }

        public ISocial CreateSocial()
        {
            IShareDialogAdapter shareDialogAdapter = new EditorShareDialogAdapter(_logger);
            return new Common.Modules.Social.Social(shareDialogAdapter);
        }

        public IPlayer CreatePlayer()
        {
            return new EditorPlayer(_logger);
        }

        public IRemoteConfig CreateRemoteConfig()
        {
            return new EditorRemoteConfig(_logger);
        }

        public IDevice CreateDevice()
        {
            return new DefaultDevice();
        }
    }
}
#endif
