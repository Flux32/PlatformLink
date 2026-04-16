using System;
using PlatformLink.Platform.Android;
using RetroCat.PlatformLink.Runtime.Source.Common.Modules.Advertisement.Adapters;
using ILogger = PlatformLink.PluginDebug.ILogger;

[Serializable]
public class YandexMobileAdsInterstitialAdapterConfig : InterstitialAdapterConfig
{
    public override string DisplayName => "Yandex Mobile Ads";

    public override IInterstitialAdAdapter CreateAdapter(ILogger logger)
    {
        return new YandexMobileAdsInterstitialAdapter(logger);
    }
}
