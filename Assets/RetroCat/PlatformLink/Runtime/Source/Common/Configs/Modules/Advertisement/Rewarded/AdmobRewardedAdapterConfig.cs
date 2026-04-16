using System;
using PlatformLink.Platform.Android;
using RetroCat.PlatformLink.Runtime.Source.Common.Modules.Advertisement.Adapters;
using UnityEngine;
using ILogger = PlatformLink.PluginDebug.ILogger;

[Serializable]
public class AdmobRewardedAdapterConfig : RewardedAdapterConfig
{
    [SerializeField] private string _appID;
    [SerializeField] private string _adUnitID;

    public string AppID => _appID;
    public string AdUnitID => _adUnitID;

    public override string DisplayName => "Admob";

    public override IRewardedAdAdapter CreateAdapter(ILogger logger)
    {
        return new AdmobRewardedAdapter(logger);
    }
}
