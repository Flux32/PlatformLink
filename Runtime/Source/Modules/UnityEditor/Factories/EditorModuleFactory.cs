#if UNITY_EDITOR
using PlatformLink.Platform.UnityEditor;
using PlatformLink.PluginDebug;
using PlatformLink.Common;
using UnityEngine;

public class EditorModuleFactory : IModuleFactory
{
    private const string InterstitialViewPath = "Prefabs/Ad/interstetial_editor_ad";

    public IInterstitialAd CreateInterstitialAd()
    {
        EditorInterstitialView interstitialViewPrefab = Resources.Load<EditorInterstitialView>(InterstitialViewPath);
        EditorInterstitialView interstitialView = Object.Instantiate(interstitialViewPrefab);
        Object.DontDestroyOnLoad(interstitialView.gameObject);
        return new EditorInterstitialAd(new PLinkLogger(), interstitialView);
    }

    public IRewardedAd CreateRewardedAd()
    {
        return new EditorRewardedAd(new PLinkLogger());
    }

    public IEnvironment CreateEnvironment()
    {
        return new EditorEnvironment("ru", PlatformLink.DeviceType.Mobile);
    }

    public IStorage CreateStorage()
    {
        return new EditorStorage(new PLinkLogger());
    }
}
#endif