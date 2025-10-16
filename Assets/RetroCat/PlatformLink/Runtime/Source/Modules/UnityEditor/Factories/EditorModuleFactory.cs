#if UNITY_EDITOR
using PlatformLink.Platform.UnityEditor;
using PlatformLink.PluginDebug;
using RetroCat.PlatformLink.Runtime.Source.Common.Modules;
using RetroCat.PlatformLink.Runtime.Source.Common.Modules.Advertisement;
using RetroCat.PlatformLink.Runtime.Source.Common.Modules.Environment;
using RetroCat.PlatformLink.Runtime.Source.Common.Modules.Purchases;
using RetroCat.PlatformLink.Runtime.Source.Common.Modules.Storage;
using RetroCat.PlatformLink.Runtime.Source.Modules.UnityEditor.Purchases;
using UnityEngine;
using DeviceType = RetroCat.PlatformLink.Runtime.Source.Common.Modules.Environment.DeviceType;

namespace RetroCat.PlatformLink.Runtime.Source.Modules.UnityEditor.Factories
{
    public class EditorModuleFactory : IModuleFactory
    {
        private const string InterstitialViewPath = "Prefabs/Ad/interstetial_editor_ad";
        private const string RewardedViewPath = "Prefabs/Ad/rewarded_editor_ad";
    
        private EditorSettings EditorSettings => PlatformLinkSettings.Instance.Editor;
    
        private PLinkLogger _logger = new PLinkLogger();
    
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
    }
}
#endif