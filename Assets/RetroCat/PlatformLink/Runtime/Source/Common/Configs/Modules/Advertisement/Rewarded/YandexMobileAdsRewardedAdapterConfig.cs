using System;
using PlatformLink.Platform.Android;
using RetroCat.PlatformLink.Runtime.Source.Common.Modules.Advertisement.Adapters;
using ILogger = PlatformLink.PluginDebug.ILogger;

[Serializable]
public class YandexMobileAdsRewardedAdapterConfig : RewardedAdapterConfig
{
    public override string DisplayName => "Yandex Mobile Ads";

    public override IRewardedAdAdapter CreateAdapter(ILogger logger)
    {
        return new YandexMobileAdsRewardedAdapter(logger);
    }
}
