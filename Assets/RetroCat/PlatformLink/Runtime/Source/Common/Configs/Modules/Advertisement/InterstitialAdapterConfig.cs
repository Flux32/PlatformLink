using System;
using RetroCat.PlatformLink.Runtime.Source.Common.Modules.Advertisement.Adapters;
using ILogger = PlatformLink.PluginDebug.ILogger;

[Serializable]
public abstract class InterstitialAdapterConfig : IModuleAdapterConfig
{
    public abstract string DisplayName { get; }
    public abstract IInterstitialAdAdapter CreateAdapter(ILogger logger);
}
