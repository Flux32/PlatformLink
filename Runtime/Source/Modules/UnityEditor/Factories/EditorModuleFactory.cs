#if UNITY_EDITOR
using PlatformLink.Platform.UnityEditor;
using PlatformLink.PluginDebug;
using PlatformLink.Common;
using UnityEngine;

public class EditorModuleFactory : IModuleFactory
{
    private const string InterstitialViewPath = "Prefabs/Ad/interstetial_editor_ad";
    private const string RewardedViewPath = "Prefabs/Ad/rewarded_editor_ad";
    
    private EditorSettings EditorSettings => PlatformLinkSettings.Instance.Editor;
    
    public IInterstitialAd CreateInterstitialAd()
    {
        EditorInterstitialView interstitialViewPrefab = Resources.Load<EditorInterstitialView>(InterstitialViewPath);
        EditorInterstitialView interstitialView = Object.Instantiate(interstitialViewPrefab);
        
        Object.DontDestroyOnLoad(interstitialView.gameObject);
        return new EditorInterstitialAd(new PLinkLogger(), interstitialView);
    }

    public IRewardedAd CreateRewardedAd()
    {
        EditorRewardedView rewardedViewPrefab = Resources.Load<EditorRewardedView>(RewardedViewPath);
        EditorRewardedView rewardedView = Object.Instantiate(rewardedViewPrefab);
        
        return new EditorRewardedAd(new PLinkLogger(), rewardedView);
    }

    public IEnvironment CreateEnvironment()
    {
        return new EditorEnvironment(EditorSettings.Environment.Language, PlatformLink.DeviceType.Mobile);
    }

    public IStorage CreateStorage()
    {
        return new EditorStorage(new PLinkLogger(), EditorSettings.Storage.SaveFilePath);
    }
}
#endif