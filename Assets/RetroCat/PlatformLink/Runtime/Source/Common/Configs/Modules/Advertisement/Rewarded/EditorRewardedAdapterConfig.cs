using System;
using RetroCat.PlatformLink.Runtime.Source.Common.Modules.Advertisement.Adapters;
using RetroCat.PlatformLink.Runtime.Source.Common.Modules.Advertisement.Adapters.Stub;
using ILogger = PlatformLink.PluginDebug.ILogger;
#if UNITY_EDITOR
using PlatformLink.Platform.UnityEditor;
using UnityEngine;
#endif

[Serializable]
public class EditorRewardedAdapterConfig : RewardedAdapterConfig
{
    private const string RewardedViewPath = "Prefabs/Ad/rewarded_editor_ad";

    public override string DisplayName => "Editor";

    public override IRewardedAdAdapter CreateAdapter(ILogger logger)
    {
#if UNITY_EDITOR
        EditorRewardedView prefab = Resources.Load<EditorRewardedView>(RewardedViewPath);
        EditorRewardedView view = UnityEngine.Object.Instantiate(prefab);
        UnityEngine.Object.DontDestroyOnLoad(view.gameObject);
        return new EditorRewardedAdapter(logger, view);
#else
        return new StubRewardedAdAdapter(logger);
#endif
    }
}
