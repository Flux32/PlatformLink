using System;
using RetroCat.PlatformLink.Runtime.Source.Common.Modules.Advertisement.Adapters;
using ILogger = PlatformLink.PluginDebug.ILogger;
#if !UNITY_EDITOR && UNITY_WEBGL
using PlatformLink;
using PlatformLink.Platform.YandexGames;
#endif

[Serializable]
public class YandexGamesInterstitialAdapterConfig : InterstitialAdapterConfig
{
    public override string DisplayName => "Yandex Games";

    public override IInterstitialAdAdapter CreateAdapter(ILogger logger)
    {
#if !UNITY_EDITOR && UNITY_WEBGL
        return PlatformLinkObject.AddComponent<YandexGamesInterstitialAdapter>();
#else
        logger.LogWarning("YandexGamesInterstitialAdapter is only available in WebGL builds.");
        return new RetroCat.PlatformLink.Runtime.Source.Common.Modules.Advertisement.Adapters.Stub.StubInterstitialAdAdapter(logger);
#endif
    }
}
