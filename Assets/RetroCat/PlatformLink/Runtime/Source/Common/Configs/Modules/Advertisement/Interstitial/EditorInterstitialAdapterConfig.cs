using System;
using RetroCat.PlatformLink.Runtime.Source.Common.Modules.Advertisement.Adapters;
using RetroCat.PlatformLink.Runtime.Source.Common.Modules.Advertisement.Adapters.Stub;
using ILogger = PlatformLink.PluginDebug.ILogger;
#if UNITY_EDITOR
using PlatformLink.Platform.UnityEditor;
using UnityEngine;
#endif

[Serializable]
public class EditorInterstitialAdapterConfig : InterstitialAdapterConfig
{
    private const string InterstitialViewPath = "Prefabs/Ad/interstetial_editor_ad";

    public override string DisplayName => "Editor";

    public override IInterstitialAdAdapter CreateAdapter(ILogger logger)
    {
#if UNITY_EDITOR
        EditorInterstitialView prefab = Resources.Load<EditorInterstitialView>(InterstitialViewPath);
        EditorInterstitialView view = UnityEngine.Object.Instantiate(prefab);
        UnityEngine.Object.DontDestroyOnLoad(view.gameObject);
        return new EditorInterstitialAdapter(logger, view);
#else
        return new StubInterstitialAdAdapter(logger);
#endif
    }
}
