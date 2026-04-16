using System;
using RetroCat.PlatformLink.Runtime.Source.Common.Modules.Advertisement.Adapters;
using ILogger = PlatformLink.PluginDebug.ILogger;

[Serializable]
public abstract class RewardedAdapterConfig : IModuleAdapterConfig
{
    public abstract string DisplayName { get; }
    public abstract IRewardedAdAdapter CreateAdapter(ILogger logger);
}
