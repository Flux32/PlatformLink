using System;
using RetroCat.PlatformLink.Runtime.Source.Common.Modules.Advertisement.Adapters;
using ILogger = PlatformLink.PluginDebug.ILogger;
#if !UNITY_EDITOR && UNITY_WEBGL
using PlatformLink;
using PlatformLink.Platform.YandexGames;
#endif

[Serializable]
public class YandexGamesRewardedAdapterConfig : RewardedAdapterConfig
{
    public override string DisplayName => "Yandex Games";

    public override IRewardedAdAdapter CreateAdapter(ILogger logger)
    {
#if !UNITY_EDITOR && UNITY_WEBGL
        return PlatformLinkObject.AddComponent<YandexGamesRewardedAdapter>();
#else
        logger.LogWarning("YandexGamesRewardedAdapter is only available in WebGL builds.");
        return new RetroCat.PlatformLink.Runtime.Source.Common.Modules.Advertisement.Adapters.Stub.StubRewardedAdAdapter(logger);
#endif
    }
}
